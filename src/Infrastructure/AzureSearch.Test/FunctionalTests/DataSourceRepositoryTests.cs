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
            const string AzureSearchServiceName = "phxsearch2-basic-dev";
            const string AzureSearchSecretKey = "0531BEDE05D5E35F30CAC3751F300A25";
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
            _dataSourceRepository = new DataSourceRepository(config, dataSourceConverter, dataSourcesConverter);
        }

        [TestMethod]
        public void CreatePhoenixDataSource()
        {
            //Arrange
            const string DataSourceName = "ds-servicetype";
            var dataSourcePayload = GetPhxDataSource();

            //Act
            _dataSourceRepository.Create(DataSourceName, dataSourcePayload, false);

            //Assert
            Assert.IsTrue(_dataSourceRepository.Exists(DataSourceName));
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
                        'credentials' : { 'connectionString' : 'Server=tcp:sql-msonecatalogdev.database.windows.net,1433;Database=dbmsonecatalogdev;Trusted_Connection=False;User ID=catalogdevadmin;Password=CltgServerdev#312' },  
                        'container' : { 'name' : 'Project' },  
                        'dataChangeDetectionPolicy' : { '@odata.type' : '#Microsoft.Azure.Search.SqlIntegratedChangeTrackingPolicy' }
                    }";
        }

        private string GetPhxDataSource()
        {
            return @"
                    {   
                        'name' : 'ds-servicetype',  
                        'description' : 'Testing Change Tracking',
                        'type' : 'azuresql',
                        'credentials' : { 'connectionString' : 'Data Source=tcp:gh5y65pnnh.database.windows.net,1433;Initial Catalog=PhoenixPublish;user id=PhoenixAdmin2013@gh5y65pnnh;password=Pho3n1x@dmin17' },  
                        'container' : { 'name' : 'API.ServiceType' },  
                        'dataChangeDetectionPolicy' : { '@odata.type' : '#Microsoft.Azure.Search.SqlIntegratedChangeTrackingPolicy' }
                    }";
        }
        #endregion
    }
}