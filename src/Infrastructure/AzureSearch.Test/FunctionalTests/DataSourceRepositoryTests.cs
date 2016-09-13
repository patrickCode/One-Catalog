using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Azure.Search.Models;
using Microsoft.Catalog.Common.Converters;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Catalog.Azure.Search.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Catalog.Azure.Search.Test.FunctionalTests
{
    [TestClass]
    [TestCategory("Functional")]
    public class DataSourceRepositoryTests
    {
        private IDataSourceRepository _dataSourceRepository;
        public DataSourceRepositoryTests()
        {
            //Arrange
            const string AzureSearchServiceName = "srch-onecatalog";
            const string AzureSearchSecretKey = "5FC27BDF967051C61830D831988B5211";
            var config = new AzureSearchConfiguration()
            {
                ServiceName = AzureSearchServiceName,
                ServiceSecretKey = AzureSearchSecretKey,
                Version = "2015-02-28",
                IsExponentialRetry = true,
                MaxRetryCount = 3,
                RetryInterval = TimeSpan.FromSeconds(1)
            };
            var dataSourceConverter = new JsonConverter<DataSource>();
            var dataSourcesConverter = new JsonConverter<List<DataSource>>();
            _dataSourceRepository = new DataSourceRepository(config, dataSourceConverter, dataSourcesConverter);
        }

        [TestMethod]
        public void Search_DataSource_Create()
        {
            //Arrange
            const string DataSourceName = "testdatasource";
            var dataSourcePayload = GetDataSource();

            //Act
            _dataSourceRepository.Create(DataSourceName, dataSourcePayload, false);

            //Assert
            Assert.IsTrue(_dataSourceRepository.Exists(DataSourceName));

            //Cleanup
            _dataSourceRepository.Delete(DataSourceName);
        }

        [TestMethod]
        public void Search_DataSource_Delete()
        {
            //Arrange
            const string DataSourceName = "testdatasource";
            var dataSourcePayload = GetDataSource();
            _dataSourceRepository.Create(DataSourceName, dataSourcePayload, false);
            _dataSourceRepository.Exists(DataSourceName);

            //Act
            _dataSourceRepository.Delete(DataSourceName);

            //Assert
            Assert.IsFalse(_dataSourceRepository.Exists(DataSourceName));
        }

        [TestMethod]
        public void Search_DataSource_Get()
        {
            //Arrange
            const string DataSourceName = "testdatasource";
            var dataSourcePayload = GetDataSource();
            _dataSourceRepository.Create(DataSourceName, dataSourcePayload, false);
            _dataSourceRepository.Exists(DataSourceName);

            //Act
            var dataSource = _dataSourceRepository.Get(DataSourceName);

            //Assert
            Assert.IsNotNull(dataSource);
            Assert.AreEqual(DataSourceName, dataSource.Name);

            //Cleanup
            _dataSourceRepository.Delete(DataSourceName);
            Assert.IsFalse(_dataSourceRepository.Exists(DataSourceName));
        }

        [TestMethod]
        public void Search_DataSource_GetAll()
        {
            //Arrange
            const string DataSourceName = "testdatasource";
            var dataSourcePayload = GetDataSource();
            _dataSourceRepository.Create(DataSourceName, dataSourcePayload, false);
            _dataSourceRepository.Exists(DataSourceName);

            //Act
            var dataSourceList = _dataSourceRepository.Get();

            //Assert
            Assert.IsNotNull(dataSourceList);
            Assert.IsNotNull(dataSourceList.FirstOrDefault(dataSource => dataSource.Name.Equals(DataSourceName)));

            //Cleanup
            _dataSourceRepository.Delete(DataSourceName);
            Assert.IsFalse(_dataSourceRepository.Exists(DataSourceName));
        }

        #region Helpers
        private string GetDataSource()
        {
            return @"
                    {   
                        'name' : 'testdatasource',  
                        'description' : 'Test Description',
                        'type' : 'azuresql',
                        'credentials' : { 'connectionString' : 'Server=tcp:sql-msonecatalogdev.database.windows.net,1433;Database=db-msonecatalogdev;Trusted_Connection=False;User ID=catalogdevadmin;Password=CltgServerdev#312' },  
                        'container' : { 'name' : 'Project' },  
                        'dataChangeDetectionPolicy' : { '@odata.type' : '#Microsoft.Azure.Search.SqlIntegratedChangeTrackingPolicy' }
                    }";
        }
        #endregion
    }
}