using Microsoft.Azure.Search.Models;
using Microsoft.Catalog.Azure.Search;
using Microsoft.Catalog.Azure.Search.Interfaces;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Catalog.Common.Converters;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectIndex
{
    public class Program
    {
        private static IIndexRepository _indexRepository;
        const string AzureSearchServiceName = "srch-onecatalog";
        const string AzureSearchSecretKey = "3304CCABCBCDBDE38790BBB4049A2300";
        public static void Main(string[] args)
        {
            var config = new AzureSearchConfiguration()
            {
                ServiceName = AzureSearchServiceName,
                ServiceSecretKey = AzureSearchSecretKey,
                Version = "2015-02-28",
                IsExponentialRetry = true,
                MaxRetryCount = 3,
                RetryInterval = TimeSpan.FromSeconds(1)
            };
            
            var indexConverter = new JsonConverter<Index>();
            var indexesConverter = new JsonConverter<List<Index>>();
            _indexRepository = new IndexRepository(config, indexConverter, indexesConverter);

            var file = File.ReadAllText("index-project.json");
            _indexRepository.Create("index-project", file, true);
        }
    }
}
