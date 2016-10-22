using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Azure.Search.Models;
using Microsoft.Catalog.Azure.Search;
using Microsoft.Catalog.Common.Converters;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Catalog.Azure.Search.Interfaces;

namespace ProjectIndexer
{
    public class Program
    {
        private static IDataSourceRepository _dataSourceRespository;
        private static IIndexerRepository _indexerRepository;
        const string AzureSearchServiceName = "srch-onecatalog";
        const string AzureSearchSecretKey = "3304CCABCBCDBDE38790BBB4049A2300";
        public static void Main(string[] args)
        {
            var config = new AzureSearchConfiguration()
            {
                ServiceName = AzureSearchServiceName,
                ServiceSecretKey = AzureSearchSecretKey,
                Version = "2015-02-28-Preview",
                IsExponentialRetry = true,
                MaxRetryCount = 3,
                RetryInterval = TimeSpan.FromSeconds(1)
            };
            var dataSourceConverter = new JsonConverter<DataSource>();
            var dataSourcesConverter = new JsonConverter<List<DataSource>>();
            _dataSourceRespository = new DataSourceRepository(config, dataSourceConverter, dataSourcesConverter);

            var indexerConverter = new JsonConverter<Indexer>();
            var indexeresConverter = new JsonConverter<List<Indexer>>();
            _indexerRepository = new IndexerRepository(config, indexerConverter, indexeresConverter);

            var dataSourceJsonStr = File.ReadAllText("datasource-project.json");
            var indexerJsonStr = File.ReadAllText("indexer-project.json");

            _dataSourceRespository.Create("datasource-project-sql", dataSourceJsonStr, true);
            _indexerRepository.Create("indexer-project", indexerJsonStr, true);
        }
    }
}