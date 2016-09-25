using Newtonsoft.Json;

namespace Microsoft.Catalog.Azure.Search.Models.SearchResponseMetadata
{
    public class Facet
    {
        public Facet() { }
        [JsonProperty(PropertyName = "count")]
        public long Count { get; set; }
        [JsonProperty(PropertyName = "value")]
        public object Value { get; set; }
        [JsonProperty(PropertyName = "from")]
        public object From { get; set; }
        [JsonProperty(PropertyName = "to")]
        public object To { get; set; }

        //TODO
        //Add FacetType as enum
    }
}