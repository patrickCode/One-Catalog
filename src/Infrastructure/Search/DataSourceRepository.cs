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
    public class DataSourceRepository: IDataSourceRepository
    {
        private SearchServiceClient _client;
        private AzureSearchConfiguration _configuration;
        private IConverter<DataSource> _dataSourceConverter;
        private IConverter<List<DataSource>> _dataSourcesConverter;
        private AzureSearchHttpClient _httpClient;
        public DataSourceRepository(AzureSearchConfiguration configuration, IConverter<DataSource> dataSourceConverter, IConverter<List<DataSource>> dataSourcesConverter)
        {
            _configuration = configuration;
            _dataSourceConverter = dataSourceConverter;
            _dataSourcesConverter = dataSourcesConverter;
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

        public DataSource Get(string dataSourceName)
        {
            if (!Exists(dataSourceName))
                throw new AzureSearchException(
                    string.Format(Constants.ExceptionMessages.AzureSearch.DataSourceNotFound, dataSourceName),
                    (int)Constants.ExceptionCodes.AzureSearch.DataSourceNotFound);
            //using .NET SDK (not supported in .NET Standard)
            //return _client.DataSources.Get(dataSourceName);
            var endpoint = _configuration.GetDataSourceApi.Replace(Constants.Search.DataSourceName, dataSourceName);
            var response = _httpClient.SendRequest(endpoint, HttpMethod.Get);
            return _dataSourceConverter.Deserialize(response);
        }

        public List<DataSource> Get()
        {
            //using .NET SDK (not supported in .NET Standard)
            //return _client.DataSources.List();
            var endpoint = _configuration.ListDataSourceApi;
            var response = JObject.Parse(_httpClient.SendRequest(endpoint, HttpMethod.Get));
            var dataSourceList = response["value"].ToString();
            return _dataSourcesConverter.Deserialize(dataSourceList);
        }

        public bool Exists(string dataSourceName)
        {
            return _client.DataSources.Exists(dataSourceName);
        }

        public void Create(string dataSourceName, string dataSourceJsonStr, bool force = false)
        {
            if (_client.DataSources.Exists(dataSourceName))
            {
                if (force)
                    Delete(dataSourceName);
                else
                    return;
            }
            var endpoint = _configuration.CreateDataSourceApi;
            _httpClient.SendRequest(endpoint, HttpMethod.Post, dataSourceJsonStr);
        }

        public void Create(DataSource dataSource, bool force)
        {
            if (_client.DataSources.Exists(dataSource.Name))
            {
                if (force)
                    Delete(dataSource.Name);
                else
                    return;
            }
            var dataSourceJsonStr = _dataSourceConverter.Serialize(dataSource);
            var endpoint = _configuration.CreateDataSourceApi;
            _httpClient.SendRequest(endpoint, HttpMethod.Post, dataSourceJsonStr);
        }

        public void Update(string dataSourceName, string dataSourceJsonStr, bool createIfNotPresent = true)
        {
            if (!_client.DataSources.Exists(dataSourceName) && createIfNotPresent)
                Create(dataSourceName, dataSourceJsonStr, false);
            else
                throw new AzureSearchException(
                    string.Format(Constants.ExceptionMessages.AzureSearch.DataSourceNotFound, dataSourceName),
                    (int)Constants.ExceptionCodes.AzureSearch.DataSourceNotFound);
            var endpoint = _configuration.UpdateDataSourceApi.Replace(Constants.Search.DataSourceName, dataSourceName);
            _httpClient.SendRequest(endpoint, HttpMethod.Put, dataSourceJsonStr);
        }

        public void Delete(string dataSourceName)
        {
            if (!_client.DataSources.Exists(dataSourceName))
                throw new AzureSearchException(
                    string.Format(Constants.ExceptionMessages.AzureSearch.DataSourceNotFound, dataSourceName),
                    (int)Constants.ExceptionCodes.AzureSearch.DataSourceNotFound);
            _client.DataSources.Delete(dataSourceName);
        }
    }
}