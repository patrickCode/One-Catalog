using System.Linq;
using Newtonsoft.Json;
using Microsoft.Catalog.Common.Models.Search;

namespace Microsoft.Catalog.Azure.Search.Models
{
    public class SearchBody
    {
        [JsonProperty("count")]
        public bool IncludeCount { get; set; }
        [JsonProperty("facets")]
        public string[] Facets { get; set; }
        [JsonProperty("filter")]
        public string Filter { get; set; }
        [JsonProperty("highlight")]
        public string Highlight { get; set; }
        [JsonProperty("highlightPreTag")]
        public string HighlightPreTag { get; set; }
        [JsonProperty("highlightPostTag")]
        public string HighlightPostTag { get; set; }
        [JsonProperty("minimumCoverage")]
        public int MinimumCoverage { get; set; }
        [JsonProperty("orderby")]
        public string Orderby { get; set; }
        [JsonProperty("scoringParameters")]
        public string[] ScoringParameters { get; set; }
        [JsonProperty("scoringProfile")]
        public string ScoringProfile { get; set; }
        [JsonProperty("search")]
        public string SearchText { get; set; }
        [JsonProperty("searchFields")]
        public string SearchFields { get; set; }
        [JsonProperty("searchMode")]
        public string SearchMode { get; set; }
        [JsonProperty("select")]
        public string Select { get; set; }
        [JsonProperty("skip")]
        public int Skip { get; set; }
        [JsonProperty("top")]
        public int Top { get; set; }
        public SearchBody() { }
        public static SearchBody FromParameters(SearchParameters parameters)
        {
            var searchBody = new SearchBody
            {
                SearchText = string.IsNullOrEmpty(parameters.SearchText) ? "*" : parameters.SearchText,
                Filter = string.IsNullOrEmpty(parameters.FilterExpression) ? null : parameters.FilterExpression,
                Facets = parameters.Facets != null && parameters.Facets.Any() ? parameters.Facets.ToArray() : new string[0]
            };

            //Setting up Metadata
            if (parameters.SearchMetadata != null)
            {
                searchBody.IncludeCount = parameters.SearchMetadata.IncludeCount;
                searchBody.Skip = parameters.SearchMetadata.Skip != null
                            ? (int)parameters.SearchMetadata.Skip : 0;
                searchBody.Top = parameters.SearchMetadata.Top != null
                            ? (int)parameters.SearchMetadata.Top : 50;
                searchBody.MinimumCoverage = parameters.SearchMetadata.MinimumCoverage != null
                    ? (int)parameters.SearchMetadata.MinimumCoverage : 100;
                searchBody.Orderby = parameters.SearchMetadata.OrderByExpressions != null && parameters.SearchMetadata.OrderByExpressions.Any()
                    ? string.Join(",", parameters.SearchMetadata.OrderByExpressions) : string.Empty;
                searchBody.SearchFields = parameters.SearchMetadata.SearchFields != null && parameters.SearchMetadata.SearchFields.Any()
                    ? string.Join(",", parameters.SearchMetadata.SearchFields) : string.Empty;
                searchBody.SearchMode = parameters.SearchMetadata.SearchMode == Common.Enums.SearchMode.All ? "all" : "any";
                searchBody.Select = parameters.SearchMetadata.ReturnFields != null && parameters.SearchMetadata.ReturnFields.Any()
                    ? string.Join(",", parameters.SearchMetadata.ReturnFields) : string.Empty;
            }
            else
            {
                searchBody.IncludeCount = false;
                searchBody.Skip = 0;
                searchBody.Top = 50;
                searchBody.MinimumCoverage = 100;
                searchBody.Orderby = null;
                searchBody.SearchFields = null;
                searchBody.SearchMode = "any";
                searchBody.Select = null;
            }

            //Setting up Scoring Profiles
            if (parameters.Scoring != null)
            {
                searchBody.ScoringProfile = parameters.Scoring.ProfileName;
                searchBody.ScoringParameters = parameters.Scoring.Parameters != null && parameters.Scoring.Parameters.Any()
                    ? parameters.Scoring.Parameters.ToArray()
                    : new string[0];
            }
            else
            {
                searchBody.ScoringProfile = null;
                searchBody.ScoringParameters = new string[0];
            }

            //Highlight Tags
            if (parameters.Highlight != null)
            {
                searchBody.Highlight = parameters.Highlight.Fields != null && parameters.Highlight.Fields.Any()
                    ? string.Join(",", parameters.Highlight.Fields)
                    : null;
                searchBody.HighlightPreTag = parameters.Highlight.PreTag;
                searchBody.HighlightPostTag = parameters.Highlight.PostTag;
            }
            return searchBody;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}