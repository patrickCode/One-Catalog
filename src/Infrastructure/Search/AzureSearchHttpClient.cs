using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Catalog.Common;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Microsoft.Catalog.Common.Exceptions;
using Microsoft.Catalog.Common.Configuration;
using System.Threading.Tasks;

namespace Microsoft.Catalog.Azure.Search
{
    public class AzureSearchHttpClient
    {
        private readonly AzureSearchConfiguration _configuration;
        private readonly JsonSerializerSettings _jsonSettings;
        public AzureSearchHttpClient(AzureSearchConfiguration configuration)
        {
            _configuration = configuration;
            _jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            _jsonSettings.Converters.Add(new StringEnumConverter());
        }

        private HttpClient CreateSearchClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(_configuration.ApiKey, _configuration.ServiceSecretKey);
            return httpClient;
        }

        private bool IsRequestSuccessful(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return true;
            var error = response.Content == null ? null : response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Message: {0}", error);
            return false;
        }

        public string SendRequest(string uri, HttpMethod method, string payload = null)
        {
            var builder = new UriBuilder(uri);
            var separator = string.IsNullOrWhiteSpace(builder.Query) ? string.Empty : "&";
            builder.Query = String.Format("{0}{1}{2}", builder.Query.TrimStart('?'), separator, _configuration.Version);

            var request = new HttpRequestMessage(method, builder.Uri);

            if (!string.IsNullOrEmpty(payload))
                request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            var client = CreateSearchClient();
            var response = client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsStringAsync().Result;
            var error = response.Content == null ? null : response.Content.ReadAsStringAsync().Result;
            throw new AzureSearchException(string.Format(Constants.ExceptionMessages.AzureSearch.RequestFailed, error), (int)Constants.ExceptionCodes.AzureSearch.RequestFailed);
        }

        public async Task<string> SendRequestAsync(string uri, HttpMethod method, string payload = null)
        {
            var builder = new UriBuilder(uri);
            var separator = string.IsNullOrWhiteSpace(builder.Query) ? string.Empty : "&";
            builder.Query = string.Format("{0}{1}{2}", builder.Query.TrimStart('?'), separator, _configuration.Version);

            var request = new HttpRequestMessage(method, builder.Uri);

            if (!string.IsNullOrEmpty(payload))
                request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            var client = CreateSearchClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            var error = response.Content == null ? null : response.Content.ReadAsStringAsync().Result;
            throw new AzureSearchException(string.Format(Constants.ExceptionMessages.AzureSearch.RequestFailed, error), (int)Constants.ExceptionCodes.AzureSearch.RequestFailed);
        }
    }
}