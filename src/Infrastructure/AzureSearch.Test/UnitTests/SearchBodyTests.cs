using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Catalog.Common.Enums;
using Microsoft.Catalog.Azure.Search.Models;
using Microsoft.Catalog.Common.Models.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Catalog.Azure.Search.Test.UnitTests
{
    [TestClass]
    public class SearchBodyTests
    {
        [TestMethod]
        public void SearchBody_AllParameters()
        {
            //Arrange
            var defSearchParameters = new SearchParameters()
            {
                SearchText = "search",
                AppliedFacets = new AppliedFacets()
                {
                    Filters = new List<AppliedFilters>()
                    {
                        new AppliedFilters()
                        {
                             FacetName = "field_1",
                             Values = new List<string>() { "val" },
                             IsMultiple = false
                        }
                    }
                },
                Facets = new List<string>() { "field_1", "field_1" },
                SearchMetadata = new Metadata()
                {
                    IncludeCount = true,
                    MinimumCoverage = 100,
                    OrderByExpressions = new List<string>() { "field_1 asc" },
                    QueryType = QueryType.Simple,
                    ReturnFields = new List<string>() { "field_1", "field_2" },
                    SearchFields = new List<string>() { "field_1", "field_2" },
                    SearchMode = SearchMode.Any,
                    Skip = 10,
                    Top = 15
                },
                Highlight = new HighlightInfo()
                {
                    Fields = new List<string>() { "field_1" },
                    PreTag = "<b>",
                    PostTag = "</b>"
                },
                Scoring = new ScoringInfo()
                {
                    ProfileName = "DefaultScorer",
                    Parameters = new List<string>() { "mytag-'Hello, O''Brien',Smith" }
                }
            };

            //Act
            var searchBody = SearchBody.FromParameters(defSearchParameters);
            var searchBodyStr = searchBody.ToJson();

            //Assert
            Assert.IsNotNull(searchBody);
            Assert.IsNotNull(searchBodyStr);

            //Root Parameters
            Assert.IsTrue(defSearchParameters.Facets.ToArray().SequenceEqual(searchBody.Facets));
            Assert.AreEqual(defSearchParameters.SearchText, searchBody.SearchText);
            Assert.AreEqual(defSearchParameters.FilterExpression, searchBody.Filter);

            //Metadata
            Assert.AreEqual(defSearchParameters.SearchMetadata.IncludeCount, searchBody.IncludeCount);
            Assert.AreEqual(defSearchParameters.SearchMetadata.MinimumCoverage, searchBody.MinimumCoverage);
            Assert.AreEqual(string.Join(",", defSearchParameters.SearchMetadata.OrderByExpressions), searchBody.Orderby);
            Assert.AreEqual("any", searchBody.SearchMode);
            Assert.AreEqual(defSearchParameters.SearchMetadata.Skip, searchBody.Skip);
            Assert.AreEqual(defSearchParameters.SearchMetadata.Top, searchBody.Top);

            //Scoring Parameters
            Assert.AreEqual(defSearchParameters.Scoring.ProfileName, searchBody.ScoringProfile);
            Assert.IsTrue(defSearchParameters.Scoring.Parameters.ToArray().SequenceEqual(searchBody.ScoringParameters));

            //Highlight
            Assert.AreEqual(string.Join(",", defSearchParameters.Highlight.Fields), searchBody.Highlight);
            Assert.AreEqual(defSearchParameters.Highlight.PreTag, searchBody.HighlightPreTag);
            Assert.AreEqual(defSearchParameters.Highlight.PostTag, searchBody.HighlightPostTag);

            //Analyze
            Debug.WriteLine(searchBodyStr);
        }
    }
}