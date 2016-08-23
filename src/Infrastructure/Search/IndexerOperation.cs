using Microsoft.Catalog.Azure.Search.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Search.Models;
using Microsoft.Catalog.Common.Converters;
using Microsoft.Azure.Search;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Catalog.Common.Exceptions;
using Microsoft.Catalog.Common;
using System.Net.Http;

namespace Microsoft.Catalog.Azure.Search
{
    public class IndexerOperation : IIndexerOperation
    {
        private SearchServiceClient _client;
        private AzureSearchConfiguration _configuration;
        private IConverter<IndexerExecutionInfo> _statusConverter;
        private AzureSearchHttpClient _httpClient;
        private IIndexerRepository _indexerRepository;
        public IndexerOperation(AzureSearchConfiguration configuration, IConverter<IndexerExecutionInfo> statusConverter, IndexerRepository indexerRepository)
        {   
            _configuration = configuration;
            _statusConverter = statusConverter;
            _indexerRepository = indexerRepository;
            _httpClient = new AzureSearchHttpClient(_configuration);
            Connect();
        }

        private void Connect(int retry = 1)
        {
            try
            {
                var credentials = new SearchCredentials(_configuration.ServiceSecretKey);
                _client = new SearchServiceClient(_configuration.ServiceName, credentials);
            }
            catch (Exception error)
            {
                if (retry >= _configuration.MaxRetryCount)
                    throw new AzureSearchException(message: Constants.ExceptionMessages.AzureSearch.UnableToConnect,
                        exception: error,
                        correlationId: Guid.NewGuid().ToString(),
                        exceptionCode: (int)Constants.ExceptionCodes.AzureSearch.UnableToConnect);

                var randomDelay = 0;
                if (_configuration.IsExponentialRetry)
                    randomDelay = new Random().Next(3);
                Task.Delay(_configuration.RetryInterval.Add(new TimeSpan(0, 0, randomDelay)));
                Connect(retry + 1);
            }
        }

        public IndexerExecutionInfo GetLastRunStatus(string indexerName)
        {
            if (!_indexerRepository.Exists(indexerName))
                throw new AzureSearchException(
                    string.Format(Constants.ExceptionMessages.AzureSearch.IndexerNotFound, indexerName),
                    (int)Constants.ExceptionCodes.AzureSearch.IndexerNotFound);

            var endpoint = _configuration.GetIndexerStatusApi.Replace(Constants.Search.IndexerName, indexerName);
            var response = _httpClient.SendRequest(endpoint, HttpMethod.Get);
            return _statusConverter.Deserialize(response);
        }

        public void Reset(string indexerName)
        {
            if (!_indexerRepository.Exists(indexerName))
                throw new AzureSearchException(
                    string.Format(Constants.ExceptionMessages.AzureSearch.IndexerNotFound, indexerName),
                    (int)Constants.ExceptionCodes.AzureSearch.IndexerNotFound);

            var endpoint = _configuration.ResetIndexerApi.Replace(Constants.Search.IndexerName, indexerName);
            _httpClient.SendRequest(endpoint, HttpMethod.Post);
        }

        public void Run(string indexerName)
        {
            if (!_indexerRepository.Exists(indexerName))
                throw new AzureSearchException(
                    string.Format(Constants.ExceptionMessages.AzureSearch.IndexerNotFound, indexerName),
                    (int)Constants.ExceptionCodes.AzureSearch.IndexerNotFound);

            var endpoint = _configuration.RunIndexerApi.Replace(Constants.Search.IndexerName, indexerName);
            _httpClient.SendRequest(endpoint, HttpMethod.Post);
        }
    }
}