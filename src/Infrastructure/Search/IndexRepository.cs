using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Search;
using System.Threading.Tasks;
using Microsoft.Catalog.Common;
using System.Collections.Generic;
using Microsoft.Azure.Search.Models;
using Microsoft.Catalog.Common.Exceptions;
using Microsoft.Catalog.Common.Converters;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Catalog.Azure.Search.Interfaces;

namespace Microsoft.Catalog.Azure.Search
{
    public class IndexRepository: IIndexRepository
    {
        private SearchServiceClient _client;
        private AzureSearchConfiguration _configuration;
        private IConverter<Index> _indexConverter;
        private IConverter<List<Index>> _indexesConverter;
        private AzureSearchHttpClient _httpClient;
        public IndexRepository(AzureSearchConfiguration configuration, IConverter<Index> converter, IConverter<List<Index>> indexesConverter)
        {
            _configuration = configuration;
            _indexConverter = converter;
            _httpClient = new AzureSearchHttpClient(_configuration);
            _indexesConverter = indexesConverter;
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

        //Retrofitted for Azure Search

        public Index Get(string indexName)
        {
            if (!Exists(indexName))
                throw new AzureSearchException(
                    string.Format(Constants.ExceptionMessages.AzureSearch.IndexNotFound, indexName),
                    (int)Constants.ExceptionCodes.AzureSearch.IndexNotFound);
            //using .NET SDK (not supported in .NET Standard)
            //return _client.Indexes.Get(indexName);
            var endpoint = _configuration.GetIndexApi.Replace("[index name]", indexName);
            var response = _httpClient.SendRequest(endpoint, HttpMethod.Get);
            return _indexConverter.Deserialize(response);
        }

        public List<Index> Get()
        {
            //using .NET SDK (not supported in .NET Standard)
            //return _client.Indexes.List();
            var endpoint = _configuration.ListIndexApi;
            var response = JObject.Parse(_httpClient.SendRequest(endpoint, HttpMethod.Get));
            var indexList = response["value"].ToString();
            return _indexesConverter.Deserialize(indexList);
        }

        public bool Exists(string indexName)
        {
            return _client.Indexes.Exists(indexName);
        }

        public void Create(string indexName, string indexJsonStr, bool force = false)
        {   
            if (Exists(indexName))
            {
                if (force)
                    Delete(indexName);
                else
                    return;
            }
            var endpoint = _configuration.BuildIndexApi;
            var response =_httpClient.SendRequest(endpoint, HttpMethod.Post, indexJsonStr); 
        }

        public void Create(Index index, bool force = false)
        {
            if (Exists(index.Name))
            {
                if (force)
                    _client.Indexes.Delete(index.Name);
                else
                    return;
            }
            var endpoint = _configuration.BuildIndexApi;
            var indexJsonStr = _indexConverter.Serialize(index);
            var response = _httpClient.SendRequest(endpoint, HttpMethod.Post, indexJsonStr);
        }

        public void Update(string indexName, string indexJsonStr, bool createIfNotPresent)
        {
            if (!Exists(indexName) && createIfNotPresent)
                Create(indexName, indexJsonStr, false);
            else
                throw new AzureSearchException(Constants.ExceptionMessages.AzureSearch.IndexNotFound, (int)Constants.ExceptionCodes.AzureSearch.IndexNotFound);

            var endpoint = _configuration.UpdateIndexApi.Replace("[index name]", indexName);
            _httpClient.SendRequest(endpoint, HttpMethod.Put, indexJsonStr);
        }

        public void Delete(string indexName)
        {
            if (!Exists(indexName))
                throw new AzureSearchException(
                    string.Format(Constants.ExceptionMessages.AzureSearch.IndexNotFound, indexName),
                    (int)Constants.ExceptionCodes.AzureSearch.IndexNotFound);
            _client.Indexes.Delete(indexName);
            //Using REST API Endpoints
            //var endpoint = _configuration.DeleteIndexApi.Replace("[index name]", indexName);
            //_httpClient.SendRequest(endpoint, HttpMethod.Delete);
        }
    }
}