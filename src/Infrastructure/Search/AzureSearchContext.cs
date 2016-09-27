using System.Net.Http;
using Microsoft.Azure.Search;
using Microsoft.Catalog.Common;
using Microsoft.Catalog.Common.Converters;
using Microsoft.Catalog.Azure.Search.Models;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Catalog.Common.Models.Search;
using Microsoft.Catalog.Azure.Search.Interfaces;

namespace Microsoft.Catalog.Azure.Search
{
    public class AzureSearchContext: IAzureSearchContext
    {
        private SearchServiceClient _client;
        private IConverter<SearchResponse> _resposneConverter;
        private IConverter<SuggestionResponse> _suggestionResponseConverter;
        private AzureSearchConfiguration _configuration;
        private AzureSearchHttpClient _httpClient;
        private readonly string _index;
        public AzureSearchContext(AzureSearchConfiguration configuration, IConverter<SearchResponse> converter, IConverter<SuggestionResponse> suggestionResponseConverter)
        {
            _configuration = configuration;
            _resposneConverter = converter;
            _suggestionResponseConverter = suggestionResponseConverter;
            _httpClient = new AzureSearchHttpClient(_configuration);
        }

        public SearchResponse Search(string index, SearchParameters searchParameters)
        {   
            var searchBody = SearchBody.FromParameters(searchParameters);
            var endpoint = _configuration.SearchApi.Replace(Constants.Search.IndexName, index);
            var searchResult = _httpClient.SendRequest(endpoint, HttpMethod.Post, searchBody.ToJson());
            return _resposneConverter.Deserialize(searchResult);
        }

        public SuggestionResponse Suggest(string index, SearchParameters searchParameters)
        {
            var suggestBody = SuggestionBody.FromParameters(searchParameters);
            var endpoint = _configuration.SuggestApi.Replace(Constants.Search.IndexName, index);
            var suggestionResult = _httpClient.SendRequest(endpoint, HttpMethod.Post, suggestBody.ToJson());
            return _suggestionResponseConverter.Deserialize(suggestionResult);
        }
    }
}