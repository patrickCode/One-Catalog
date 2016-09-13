using Microsoft.Azure.Search.Models;
using Microsoft.Catalog.Azure.Search.Interfaces;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Catalog.Common.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Catalog.Azure.Search.Test.FunctionalTests
{
    [TestClass]
    [TestCategory("Functional")]
    public class DocumentRepositoryTests
    {
        private IDocumentRepository _documentRepository;
        private IIndexRepository _indexRepository;
        public DocumentRepositoryTests()
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
            var documentConverter = new JsonConverter<Document>();
            _indexRepository = new IndexRepository(config, null, null);
            _documentRepository = new DocumentRepository(config, documentConverter);
        }

        [TestMethod]
        public void Search_Document_CreateBulk()
        {
            //Arrange
            const string IndexName = "test-index-name";
            _indexRepository.Create("test-index-name", GetTestindex());

            //Act
            _documentRepository.CreateBulk(IndexName, GetTestDocument());

            //Assert
        }

        [TestMethod]
        public void Search_Document_CreateSingle()
        {
            //Arrange
            const string IndexName = "test-index-name";
            _indexRepository.Create("test-index-name", GetTestindex());

            //Act
            _documentRepository.Create(IndexName, GetTestDocumentDict());

            //Assert
        }

        #region Helpers
        private string GetTestDocument()
        {
            return @" {'value': [
                    {
                        'Id': '100',
                        'Name': 'Test Field'
                    }]}";
        }

        private Dictionary<string, object> GetTestDocumentDict()
        {
            var random = new Random();
            return new Dictionary<string, object>()
            {
                {"Id", random.Next(10, 10000).ToString() },
                { "Name", "Test Field" + Guid.NewGuid().ToString() }
            };
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
