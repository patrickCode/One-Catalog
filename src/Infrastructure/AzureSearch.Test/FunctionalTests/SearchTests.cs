using Microsoft.Catalog.Azure.Search.Interfaces;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Catalog.Common.Models.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Catalog.Azure.Search.Test.FunctionalTests
{
    [TestClass]
    public class SearchTests
    {
        private IAzureSearchContext _searchContext;
        public SearchTests()
        {
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
            _searchContext = new AzureSearchContext(config, "test-index-name");
        }
        [TestMethod]
        public void Search_GetDocuments()
        {
            //Arrange
            var searchParams = new SearchParameters()
            {
                SearchText = "DB*"
            };

            //Act
            var searchRes = _searchContext.Search(searchParams);

            //Assert
            Assert.IsNotNull(searchRes);
        }
    }
}
