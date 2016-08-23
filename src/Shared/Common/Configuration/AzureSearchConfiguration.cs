using System;

namespace Microsoft.Catalog.Common.Configuration
{
    public class AzureSearchConfiguration
    {
        public string ServiceName { get; set; }
        public string ServiceSecretKey { get; set; }
        public string Version { get; set; }
        public string ApiKey
        {
            get
            {
                return Constants.Search.AzureSearchApiKey;
            }
        }

        #region Retry Properties
        public int MaxRetryCount { get; set; }
        public TimeSpan RetryInterval { get; set; }
        public bool IsExponentialRetry { get; set; }
        #endregion

        #region API Endpoints
        #region Index
        public string GetIndexApi
        {
            get
            {
                return string.Format(Constants.Search.GetIndexApi, ServiceName, Version);
            }
        }
        public string ListIndexApi
        {
            get
            {
                return string.Format(Constants.Search.ListIndexApi, ServiceName, Version);
            }
        }
        public string BuildIndexApi
        {
            get
            {
                return string.Format(Constants.Search.CreateIndexApi, ServiceName, Version);
            }
        }
        public string UpdateIndexApi
        {
            get
            {
                return string.Format(Constants.Search.UpdateIndexApi, ServiceName, Version);
            }
        }
        public string DeleteIndexApi
        {
            get
            {
                return string.Format(Constants.Search.DeleteIndexApi, ServiceName, Version);
            }
        }
        #endregion

        #region Data Source
        public string GetDataSourceApi
        {
            get
            {
                return string.Format(Constants.Search.GetDataSourceApi, ServiceName, Version);
            }
        }
        public string ListDataSourceApi
        {
            get
            {
                return string.Format(Constants.Search.ListDataSourceApi, ServiceName, Version);
            }
        }
        public string CreateDataSourceApi
        {
            get
            {
                return string.Format(Constants.Search.CreateDataSourceApi, ServiceName, Version);
            }
        }
        public string UpdateDataSourceApi
        {
            get
            {
                return string.Format(Constants.Search.UpdateDataSourceApi, ServiceName, Version);
            }
        }
        #endregion

        #region Indexer
        public string GetIndexerApi
        {
            get
            {
                return string.Format(Constants.Search.GetIndexerApi, ServiceName, Version);
            }
        }
        public string ListIndexerApi
        {
            get
            {
                return string.Format(Constants.Search.ListIndexerApi, ServiceName, Version);
            }
        }
        public string CreateIndexerApi
        {
            get
            {
                return string.Format(Constants.Search.CreateIndexerApi, ServiceName, Version);
            }
        }
        public string UpdateIndexerApi
        {
            get
            {
                return string.Format(Constants.Search.UpdateIndexerApi, ServiceName, Version);
            }
        }
        public string DeleteIndexerApi
        {
            get
            {
                return string.Format(Constants.Search.DeleteIndexerApi, ServiceName, Version);
            }
        }
        public string GetIndexerStatusApi
        {
            get
            {
                return string.Format(Constants.Search.GetIndexerStatusApi, ServiceName, Version);
            }
        }
        public string ResetIndexerApi
        {
            get
            {
                return string.Format(Constants.Search.ResetIndexerApi, ServiceName, Version);
            }
        }
        public string RunIndexerApi
        {
            get
            {
                return string.Format(Constants.Search.RunIndexerApi, ServiceName, Version);
            }
        }
        #endregion
        #endregion

    }
}