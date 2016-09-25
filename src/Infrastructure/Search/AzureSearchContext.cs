using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Catalog.Azure.Search.Interfaces;
using Microsoft.Catalog.Azure.Search.Models;
using Microsoft.Catalog.Common;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Catalog.Common.Converters;
using Microsoft.Catalog.Common.Exceptions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Catalog.Azure.Search
{
    public class AzureSearchContext: IAzureSearchContext
    {
        private SearchServiceClient _client;
        private IConverter<SearchResponse> _resposneConverter;
        private AzureSearchConfiguration _configuration;
        private AzureSearchHttpClient _httpClient;
        private readonly string _index;
        public AzureSearchContext(AzureSearchConfiguration configuration, IConverter<SearchResponse> converter)
        {
            _configuration = configuration;
            _resposneConverter = converter;
            _httpClient = new AzureSearchHttpClient(_configuration);
        }

        private void Connect(int retry = 1)
        {
            try
            {
                var credentials = new SearchCredentials(_configuration.ServiceSecretKey);
                _client = new SearchServiceClient(_configuration.ServiceName, credentials);
                //TODO
                //Set Retry Policy
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

        public SearchResponse Search(string index, Common.Models.Search.SearchParameters searchParameters)
        {   
            var searchBody = SearchBody.FromParameters(searchParameters);
            var endpoint = _configuration.SearchApi.Replace(Constants.Search.IndexName, index);
            var searchResult = _httpClient.SendRequest(endpoint, HttpMethod.Post, searchBody.ToJson());
            return _resposneConverter.Deserialize(searchResult);
        }
    }
}