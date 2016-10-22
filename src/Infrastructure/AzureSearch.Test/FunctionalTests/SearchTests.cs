using System;
using System.Collections.Generic;
using Microsoft.Catalog.Common.Converters;
using Microsoft.Catalog.Azure.Search.Models;
using Microsoft.Catalog.Common.Configuration;
using Microsoft.Catalog.Common.Models.Search;
using Microsoft.Catalog.Azure.Search.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Catalog.Azure.Search.Test.FunctionalTests
{
    [TestClass]
    public class SearchTests
    {
        private IAzureSearchContext _searchContext;
        public SearchTests()
        {
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
            var converter = new JsonConverter<SearchResponse>();
            _searchContext = new AzureSearchContext(config, converter, null);
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
            var searchRes = _searchContext.Search("test-index-name", searchParams);

            //Assert
            Assert.IsNotNull(searchRes);
        }

        [TestMethod]
        public void Search_GetDocuments_WithPagination()
        {
            //Arrange
            var searchParams = new SearchParameters()
            {
                SearchText = "DB*",
                SearchMetadata = new Metadata()
                {
                    Skip = 0,
                    Top = 1,
                    IncludeCount = true
                }
            };

            //Act
            var searchRes = _searchContext.Search("test-index-name", searchParams);

            //Assert
            Assert.IsNotNull(searchRes);
        }

        [TestMethod]
        public void Search_GetDocuments_WithFacet()
        {
            //Arrange
            var searchParams = new SearchParameters()
            {
                SearchText = "DB*",
                Facets = new List<string>() { "Name" },
                SearchMetadata = new Metadata()
                {
                    Skip = 0,
                    Top = 1,
                    IncludeCount = true
                }
            };

            //Act
            var searchRes = _searchContext.Search("index-project", searchParams);

            //Assert
            Assert.IsNotNull(searchRes);
        }

        [TestMethod]
        public void Search_GetDocuments_WithFilters()
        {
            //Arrange
            var searchParams = new SearchParameters()
            {
                SearchText = "*",
                Facets = new List<string>() { "Technologies" },
                SearchMetadata = new Metadata()
                {
                    Skip = 0,
                    Top = 100,
                    IncludeCount = true,
                },
                AppliedFacets = new AppliedFacets()
                {
                    Filters = new List<AppliedFilters>()
                    {
                        new AppliedFilters()
                        {
                            FacetName = "Id",
                            IsMultiple = false,
                            Values = new List<string>() { "2" }
                        }
                    }
                }
            };

            //Act
            var searchRes = _searchContext.Search("index-project", searchParams);

            //Assert
            Assert.IsNotNull(searchRes);
        }

        [TestMethod]
        public void Search_GetDocuments_AllFilters()
        {
            //Arrange
            var searchParams = new SearchParameters()
            {
                SearchText = "*",
                Facets = new List<string>() { "Id", "Name" }
            };

            //Act
            var searchRes = _searchContext.Search("test-index-name", searchParams);

            //Assert
            Assert.IsNotNull(searchRes);
            Assert.IsNotNull(searchRes.Results);
            Assert.IsNotNull(searchRes.Facets);
        }
    }
}