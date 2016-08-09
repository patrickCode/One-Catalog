namespace Microsoft.Catalog.Common
{
    public static partial class Constants
    {
		public static class ExceptionNames
        {
            public const string DomainException = "Domain Validation Exception";
            public const string DatabaseException = "Database Exception";
            public const string AzureSearchException = "Azure Search Exception";
            public const string AzureStorageException = "Azure Storage Exception";
            public const string RedisCacheException = "Redis Cache Exception";
        }
		public static class ExceptionCodes
        {
			public enum Database
            {
				UnableToConnect = 10001,
				UnableToUpdate
            }
        }

		public static class ExceptionMessages
        {
			public static class Database
            {
                public const string UnableToConnect = "Unable to connect to the database";
                public const string UnableToUpdate = "Unable to update database. Details - {0}";
            }
        }
    }
}