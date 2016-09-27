using Microsoft.Catalog.Azure.Search.Models.SearchResponseMetadata;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.Catalog.Azure.Search.Models
{
    public class SuggestionResponse
    {
        [JsonProperty(PropertyName = "value")]
        public List<Document> Results { get; set; }
    }
}