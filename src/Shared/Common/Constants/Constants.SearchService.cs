namespace Microsoft.Catalog.Common
{
    public static partial class Constants
    {
		public static class Search
        {
            public const string IndexName = "[index name]";
            public const string ListIndexApi = "https://{0}.search.windows.net/indexes?api-version={1}";
            public const string GetIndexApi = "https://{0}.search.windows.net/indexes/[index name]?api-version={1}";
            public const string CreateIndexApi = "https://{0}.search.windows.net/indexes?api-version={1}";
            public const string UpdateIndexApi = "https://{0}.search.windows.net/indexes/[index name]?api-version={1}";
            public const string DeleteIndexApi = "https://{0}.search.windows.net/indexes/[index name]?api-version={1}";

            public const string DataSourceName = "[datasource name]";
            public const string ListDataSourceApi = "https://{0}.search.windows.net/datasources?api-version={1}";
            public const string GetDataSourceApi = "https://{0}.search.windows.net/datasources/[datasource name]?api-version={1}";
            public const string CreateDataSourceApi = "https://{0}.search.windows.net/datasources?api-version={1}";
            public const string UpdateDataSourceApi = "https://{0}.search.windows.net/datasources/[datasource name]?api-version={1}";
            public const string DeleteDataSourceApi = "https://{0}.search.windows.net/datasources/[datasource name]?api-version={1}";

            public const string IndexerName = "[indexer name]";
            public const string GetIndexerApi = "https://{0}.search.windows.net/indexers/[indexer name]?api-version={1}";
            public const string ListIndexerApi = "https://{0}.search.windows.net/indexers?api-version={1}";
            public const string CreateIndexerApi = "https://{0}.search.windows.net/indexers?api-version={1}";
            public const string UpdateIndexerApi = "https://{0}.search.windows.net/indexers/[indexer name]?api-version={1}";
            public const string DeleteIndexerApi = "https://{0}.search.windows.net/indexers/[indexer name]?api-version={1}";
            public const string GetIndexerStatusApi = "https://{0}.search.windows.net/indexers/[indexer name]/status?api-version={1}";
            public const string ResetIndexerApi = "https://{0}.search.windows.net/indexers/[indexer name]/reset?api-version={1}";
            public const string RunIndexerApi = "https://{0}.search.windows.net/indexers/[indexer name]/run?api-version={1}";
            
            public const string CreateDocumentApi = "https://{0}.search.windows.net/indexes/[index name]/docs/index?api-version={1}";

            public const string SearchApi = "https://{0}.search.windows.net/indexes/[index name]/docs/search?api-version={1}";
            
            public const string AzureSearchApiKey = "api-key";
        }
    }
}