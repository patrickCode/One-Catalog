namespace Microsoft.Catalog.Common
{
    public static partial class Constants
    {
		public static class ExceptionNames
        {
            public const string GeneralException = "General Application Exception";
            public const string DomainException = "Domain Validation Exception";
            public const string DatabaseException = "Database Exception";
            public const string AzureSearchException = "Azure Search Exception";
            public const string AzureStorageException = "Azure Storage Exception";
            public const string RedisCacheException = "Redis Cache Exception";
        }
		public static class ExceptionCodes
        {
            public enum Application
            {
                UnableToConvert = 10001
            }
            public enum AzureSearch
            {
                UnableToConnect = 20001,
                RequestFailed,
                IndexNotFound,
                DataSourceNotFound,
                IndexerNotFound
            }
            public enum Database
            {
				UnableToConnect = 30001,
				UnableToUpdate
            }
        }

		public static class ExceptionMessages
        {
            public static class Application
            {
                public const string UnableToConvert = "The specified object cannot be converted to the target type";
            }
			public static class Database
            {
                public const string UnableToConnect = "Unable to connect to the database";
                public const string UnableToUpdate = "Unable to update database. Details - {0}";
            }
			public static class AzureSearch
            {
                public const string UnableToConnect = "Unable to connect to Azure Search Service";
                public const string RequestFailed = "The request to Azure Search Service failed. {0}";
                public const string IndexNotFound = "The index - {0} is not present in the Azure Search Service";
                public const string DataSourceNotFound = "The data source - {0} is not present in the Azure Search Service";
                public const string IndexerNotFound = "The indexer - {0} is not present in the Azure Search Service";
            }
        }
    }
}