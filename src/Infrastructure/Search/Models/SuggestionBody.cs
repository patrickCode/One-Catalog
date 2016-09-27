using System.Linq;
using Newtonsoft.Json;
using Microsoft.Catalog.Common.Models.Search;
using Microsoft.Catalog.Common.Exceptions;

namespace Microsoft.Catalog.Azure.Search.Models
{
    public class SuggestionBody
    {
        [JsonProperty("filter")]
        public string Filter { get; set; }
        [JsonProperty("fuzzy")]
        public bool FuzzyEnabled { get; set; }
        [JsonProperty("highlightPreTag")]
        public string HighlightPreTag { get; set; }
        [JsonProperty("highlightPostTag")]
        public string HighlightPostTag { get; set; }
        [JsonProperty("minimumCoverage")]
        public int MinimumCoverage { get; set; }
        [JsonProperty("orderby")]
        public string Orderby { get; set; }
        [JsonProperty("search")]
        public string SearchText { get; set; }
        [JsonProperty("searchFields")]
        public string SearchFields { get; set; }
        [JsonProperty("select")]
        public string Select { get; set; }
        [JsonProperty("suggesterName")]
        public string SuggesterName { get; set; }
        [JsonProperty("top")]
        public int Top { get; set; }

        public static SuggestionBody FromParameters(SearchParameters parameters)
        {
            var suggestionBody = new SuggestionBody
            {
                SearchText = string.IsNullOrEmpty(parameters.SearchText) ? "*" : parameters.SearchText,
                Filter = string.IsNullOrEmpty(parameters.FilterExpression) ? null : parameters.FilterExpression
            };

            //Suggester
            if (parameters.Suggester == null)
                throw new AzureSearchException("No Suggester Information available", 1000);
            if (string.IsNullOrEmpty(parameters.Suggester.Name))
                throw new AzureSearchException("No Suggester name available", 1000);
            suggestionBody.SuggesterName = parameters.Suggester.Name;
            suggestionBody.FuzzyEnabled = parameters.Suggester.IsFuzzyEnabled;

            //Setting up Metadata
            if (parameters.SearchMetadata != null)
            {   
                suggestionBody.Top = parameters.SearchMetadata.Top != null
                            ? (int)parameters.SearchMetadata.Top : 50;
                suggestionBody.MinimumCoverage = parameters.SearchMetadata.MinimumCoverage != null
                    ? (int)parameters.SearchMetadata.MinimumCoverage : 100;
                suggestionBody.Orderby = parameters.SearchMetadata.OrderByExpressions != null && parameters.SearchMetadata.OrderByExpressions.Any()
                    ? string.Join(",", parameters.SearchMetadata.OrderByExpressions) : string.Empty;
                suggestionBody.SearchFields = parameters.SearchMetadata.SearchFields != null && parameters.SearchMetadata.SearchFields.Any()
                    ? string.Join(",", parameters.SearchMetadata.SearchFields) : string.Empty;
                suggestionBody.Select = parameters.SearchMetadata.ReturnFields != null && parameters.SearchMetadata.ReturnFields.Any()
                    ? string.Join(",", parameters.SearchMetadata.ReturnFields) : string.Empty;
            }
            else
            {   
                suggestionBody.Top = 50;
                suggestionBody.MinimumCoverage = 100;
                suggestionBody.Orderby = null;
                suggestionBody.SearchFields = null;
                suggestionBody.Select = null;
            }
            
            //Highlight Tags
            if (parameters.Highlight != null)
            {
                suggestionBody.HighlightPreTag = parameters.Highlight.PreTag;
                suggestionBody.HighlightPostTag = parameters.Highlight.PostTag;
            }
            return suggestionBody;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}