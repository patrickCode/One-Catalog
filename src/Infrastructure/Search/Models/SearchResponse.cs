using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Catalog.Azure.Search.Models.SearchResponseMetadata;

namespace Microsoft.Catalog.Azure.Search.Models
{
    public class SearchResponse
    {
        public SearchResponse() { }
        [JsonProperty(PropertyName = "@odata.count")]
        public long Count { get; set; }
        [JsonProperty(PropertyName = "@search.coverage")]
        public double Coverage { get; set; }
        [JsonProperty(PropertyName = "@search.facets")]
        public FacetResponse Facets { get; set; }
        [JsonProperty(PropertyName = "value")]
        public List<Document> Results { get; set; }
    }
}