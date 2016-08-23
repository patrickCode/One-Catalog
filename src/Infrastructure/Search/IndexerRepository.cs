using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Search;
using System.Threading.Tasks;
using Microsoft.Catalog.Common;
using System.Collections.Generic;
using Microsoft.Azure.Search.Models;
using Microsoft.Catalog.Common.Converters;
using Microsoft.Catalog.Common.Exceptions;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Catalog.Azure.Search.Interfaces;

namespace Microsoft.Catalog.Azure.Search
{
    public class IndexerRepository : IIndexerRepository
    {
        private SearchServiceClient _client;
        private AzureSearchConfiguration _configuration;
        private IConverter<Indexer> _indexerConverter;
        private IConverter<List<Indexer>> _indexersConverter;
        private AzureSearchHttpClient _httpClient;

        public IndexerRepository(AzureSearchConfiguration configuration, IConverter<Indexer> indexerConverter, IConverter<List<Indexer>> indexersConverter)
        {
            _configuration = configuration;
            _indexerConverter = indexerConverter;
            _indexersConverter = indexersConverter;
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

        public bool Exists(string indexerName)
        {
            return _client.Indexers.Exists(indexerName);
        }

        public Indexer Get(string indexerName)
        {
            if (!Exists(indexerName))
                throw new AzureSearchException(
                    string.Format(Constants.ExceptionMessages.AzureSearch.IndexerNotFound, indexerName),
                    (int)Constants.ExceptionCodes.AzureSearch.IndexerNotFound);
            //using .NET SDK (not supported in .NET Standard)
            //return _client.Indexers.Get(indexerName);
            var endpoint = _configuration.GetIndexerApi.Replace(Constants.Search.IndexerName, indexerName);
            var response = _httpClient.SendRequest(endpoint, HttpMethod.Get);
            return _indexerConverter.Deserialize(response);
        }

        public List<Indexer> Get()
        {
            //using .NET SDK (not supported in .NET Standard)
            //return _client.Indexers.List();
            var endpoint = _configuration.ListIndexerApi;
            var response = JObject.Parse(_httpClient.SendRequest(endpoint, HttpMethod.Get));
            var indexerList = response["value"].ToString();
            return _indexersConverter.Deserialize(indexerList);
        }

        public void Create(string indexerName, string indexerJsonStr, bool force = false)
        {
            if (_client.Indexers.Exists(indexerName))
            {
                if (force)
                    Delete(indexerName);
                else
                    return;
            }
            var endpoint = _configuration.CreateIndexerApi;
            _httpClient.SendRequest(endpoint, HttpMethod.Post, indexerJsonStr);
        }

        public void Create(Indexer indexer, bool force = false)
        {
            if (_client.Indexers.Exists(indexer.Name))
            {
                if (force)
                    Delete(indexer.Name);
                else
                    return;
            }
            var IndexerJsonStr = _indexerConverter.Serialize(indexer);
            var endpoint = _configuration.CreateIndexerApi;
            _httpClient.SendRequest(endpoint, HttpMethod.Post, IndexerJsonStr);
        }

        public void Update(string indexerName, string indexerJsonStr, bool createIfNotPresent = true)
        {
            if (!_client.Indexers.Exists(indexerName) && createIfNotPresent)
                Create(indexerName, indexerJsonStr, false);
            else
                throw new AzureSearchException(
                    string.Format(Constants.ExceptionMessages.AzureSearch.IndexerNotFound, indexerName),
                    (int)Constants.ExceptionCodes.AzureSearch.IndexerNotFound);
            var endpoint = _configuration.UpdateIndexerApi.Replace(Constants.Search.IndexerName, indexerName);
            _httpClient.SendRequest(endpoint, HttpMethod.Put, indexerJsonStr);
        }

        public void Delete(string indexerName)
        {
            if (!_client.Indexers.Exists(indexerName))
                throw new AzureSearchException(
                    string.Format(Constants.ExceptionMessages.AzureSearch.IndexerNotFound, indexerName),
                    (int)Constants.ExceptionCodes.AzureSearch.IndexerNotFound);
            _client.Indexers.Delete(indexerName);
        }
    }
}