using System;
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
    public class IndexerRepositoryTests
    {
        private IIndexerRepository _indexerRepository;
        private IDataSourceRepository _dataSourceRepository;
        private IIndexRepository _indexRepository;
        public IndexerRepositoryTests()
        {
            //Arrange
            const string AzureSearchServiceName = "srch-onecatalog";
            const string AzureSearchSecretKey = "3304CCABCBCDBDE38790BBB4049A2300";
            var config = new AzureSearchConfiguration()
            {
                ServiceName = AzureSearchServiceName,
                ServiceSecretKey = AzureSearchSecretKey,
                Version = "2015-02-28",
                IsExponentialRetry = true,
                MaxRetryCount = 3,
                RetryInterval = TimeSpan.FromSeconds(1)
            };
            var indexerConverter = new JsonConverter<Indexer>();
            var indexeresConverter = new JsonConverter<List<Indexer>>();
            _indexerRepository = new IndexerRepository(config, indexerConverter, indexeresConverter);
            _dataSourceRepository = new DataSourceRepository(config, null, null);
            _indexRepository = new IndexRepository(config, null, null);
        }

        [TestMethod]
        public void Search_Indexer_Create()
        {
            //Arrange
            const string IndexerName = "testindexername";
            var indexerPayload = GetTestIndexer();
            _dataSourceRepository.Create("testdatasource", GetDataSource());
            _indexRepository.Create("test-index-name", GetTestindex());
            
            //Act
            _indexerRepository.Create(IndexerName, indexerPayload);

            //Assert
            Assert.IsTrue(_indexerRepository.Exists(IndexerName));

            //Cleanup
            _indexerRepository.Delete(IndexerName);
            _dataSourceRepository.Delete("testdatasource");
            _indexRepository.Delete("test-index-name");
            Assert.IsFalse(_indexerRepository.Exists(IndexerName));
        }

        [TestMethod]
        public void Search_Indexer_Get()
        {
            //Arrange
            const string IndexerName = "testindexername";
            var indexerPayload = GetTestIndexer();
            _dataSourceRepository.Create("testdatasource", GetDataSource());
            _indexRepository.Create("test-index-name", GetTestindex());
            _indexerRepository.Create(IndexerName, indexerPayload);
            Assert.IsTrue(_indexerRepository.Exists(IndexerName));


            //Act
            var indexer = _indexerRepository.Get(IndexerName);

            //Assert
            Assert.IsNotNull(indexer);
            Assert.AreEqual(IndexerName, indexer.Name);

            //Cleanup
            _indexerRepository.Delete(IndexerName);
            _dataSourceRepository.Delete("testdatasource");
            _indexRepository.Delete("test-index-name");
            Assert.IsFalse(_indexerRepository.Exists(IndexerName));
        }

        #region Helper
        private string GetTestIndexer()
        {
            return @"
                    {
                        'name' : 'testindexername',  
                        'description' : 'Test Indexer',
                        'dataSourceName' : 'testdatasource',
                        'targetIndexName' : 'test-index-name',
                        'schedule' : { 'interval' : 'PT1H', 'startTime' : '2015-02-01T00:00:00Z' },  
                        'parameters' : { 'maxFailedItems' : 10, 'maxFailedItemsPerBatch' : 5, 'base64EncodeKeys': false }
                    }";
        }

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
        private string GetTestindex()
        {
            return @"
                    {
                        'name': 'test-index-name', 
                        'corsOptions': null,
                        'fields': [{
                            'name': 'Id',
                            'type': 'Edm.String',
                            'searchable': true,
                            'filterable': true,
                            'sortable': true,
                            'facetable': true,
                            'key': true,
                            'retrievable': true,
                            'analyzer': null
                        },
                        {
                            'name': 'Name',
                            'type': 'Edm.String',
                            'searchable': true,
                            'filterable': true,
                            'sortable': false,
                            'facetable': true,
                            'key': false,
                            'retrievable': true,
                            'analyzer': null
                        }],
                        'suggesters': [{
                            'name': 'test_suggester',
                            'searchMode': 'analyzingInfixMatching',
                            'sourceFields': ['Name']
                        }]
                    }";
        }
        #endregion
    }
}
