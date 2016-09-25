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
    public class IndexRepositoryTests
    {
        private IIndexRepository _indexRepository;
        public IndexRepositoryTests()
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
            var indexConverter = new JsonConverter<Index>();
            var indexesConverter = new JsonConverter<List<Index>>();
            _indexRepository = new IndexRepository(config, indexConverter, indexesConverter);
        }

        [TestMethod]
        public void Search_Index_Create()
        {
            //Arrange
            const string IndexName = "test-index-name";
            var indexPayload = GetTestindex();

            //Act
            _indexRepository.Create(IndexName, indexPayload);

            //Assert
            Assert.IsTrue(_indexRepository.Exists(IndexName));

            //Cleanup
            _indexRepository.Delete(IndexName);
            Assert.IsFalse(_indexRepository.Exists(IndexName));
        }

        [TestMethod]
        public void Search_Index_Delete()
        {
            //Arrange
            const string IndexName = "test-index-name";
            var indexPayload = GetTestindex();
            _indexRepository.Create(IndexName, indexPayload);
            Assert.IsTrue(_indexRepository.Exists(IndexName));

            //Act
            _indexRepository.Delete(IndexName);

            //Assert
            Assert.IsFalse(_indexRepository.Exists(IndexName));
        }

        [TestMethod]
        public void Search_Index_Get()
        {
            //Arrange
            const string IndexName = "test-index-name";
            var indexPayload = GetTestindex();
            _indexRepository.Create(IndexName, indexPayload);
            Assert.IsTrue(_indexRepository.Exists(IndexName));

            //Act
            var index = _indexRepository.Get(IndexName);

            //Assert
            Assert.IsNotNull(index);
            Assert.AreEqual(IndexName, index.Name);
            Assert.IsNotNull(index.Fields);

            //Cleanup
            _indexRepository.Delete(IndexName);
            Assert.IsFalse(_indexRepository.Exists(IndexName));
        }

        [TestMethod]
        public void Search_Index_GetAll()
        {
            //Arrange
            const string IndexName = "test-index-name";
            var indexPayload = GetTestindex();
            _indexRepository.Create(IndexName, indexPayload);
            Assert.IsTrue(_indexRepository.Exists(IndexName));

            //Act
            var indexes = _indexRepository.Get();

            //Assert
            Assert.IsNotNull(indexes);
            Assert.AreEqual(IndexName, indexes.First(index => index.Name.Equals(IndexName)).Name);
            Assert.IsNotNull(indexes.First(index => index.Name.Equals(IndexName)).Fields);

            //Cleanup
            _indexRepository.Delete(IndexName);
            Assert.IsFalse(_indexRepository.Exists(IndexName));
        }

        #region Helper
        private string GetTestindex()
        {
            return @"
                    {
                        'name': 'test-index-name', 
                        'corsOptions': null,
                        'fields': [{
                            'name': 'Test_Field_One',
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
                            'name': 'Test_Field_Two',
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
                            'sourceFields': ['Test_Field_One']
                        }]
                    }";
        }
        #endregion
    }
}
