using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Catalog.Azure.Search.Interfaces;
using Microsoft.Catalog.Common;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Catalog.Common.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Microsoft.Catalog.Azure.Search
{
    public class DocumentRepository : IDocumentRepository
    {
        private SearchServiceClient _client;
        private IConverter<Document> _documentConverter;
        private AzureSearchConfiguration _configuration;
        private AzureSearchHttpClient _httpClient;
        public DocumentRepository(AzureSearchConfiguration configuration, IConverter<Document> documentConverter)
        {
            _configuration = configuration;
            _documentConverter = documentConverter;
            _httpClient = new AzureSearchHttpClient(_configuration);
        }

        public void CreateBulk(string indexName, string documentJsonStr)
        {
            var document = JObject.Parse(documentJsonStr);
            var values = document["value"].ToArray();
            foreach(var value in values)
            {
                value["@search.action"] = "upload";
            }
            var endpoint = _configuration.CreateDocumentApi.Replace(Constants.Search.IndexName, indexName);
            _httpClient.SendRequest(endpoint, HttpMethod.Post, document.ToString());
        }

        public void Create(string indexName, Dictionary<string, object> document)
        {
            JObject docJson = new JObject();
            document.Add("@search.action", "upload");
            docJson.Add("value", new JArray() { JObject.Parse(JsonConvert.SerializeObject(document)) });
            var endpoint = _configuration.CreateDocumentApi.Replace(Constants.Search.IndexName, indexName);
            _httpClient.SendRequest(endpoint, HttpMethod.Post, docJson.ToString());
        }   
    }
}
