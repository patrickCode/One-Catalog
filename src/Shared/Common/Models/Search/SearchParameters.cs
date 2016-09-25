using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Catalog.Common.Enums;

namespace Microsoft.Catalog.Common.Models.Search
{
    public class SearchParameters
    {
        public string SearchText { get; set; }
        public string FilterExpression
        {
            get
            {
                return AppliedFacets.ToOData();
            }
        }
        public AppliedFacets AppliedFacets { get; set; }
        public List<string> Facets { get; set; }
        public Metadata SearchMetadata { get; set; }
        public ScoringInfo Scoring { get; set; }
        public HighlightInfo Highlight{ get; set; }
    }

    public class HighlightInfo
    {
        public List<string> Fields { get; set; }
        public string PreTag { get; set; }
        public string PostTag { get; set; }
    }
    public class ScoringInfo
    {
        public string ProfileName { get; set; }
        public List<string> Parameters { get; set; }

    }
    public class Metadata
    {
        public bool IncludeCount { get; set; }
        public int? Skip { get; set; }
        public int? Top { get; set; }
        public int? MinimumCoverage { get; set; }
        public List<string> OrderByExpressions { get; set; }
        public SearchMode SearchMode { get; set; }
        public QueryType QueryType { get; set; }
        public List<string> SearchFields { get; set; }
        public List<string> ReturnFields { get; set; }
    }
    public class AppliedFacets
    {
        public List<AppliedFilters> Filters;
        public string ToOData()
        {
            if (Filters == null || Filters.Count == 0)
                return null;
            Filters.RemoveAll(f => f.Values == null || f.Values.Count == 0);
            Filters.RemoveAll(f => string.IsNullOrEmpty(f.FacetName) || string.IsNullOrWhiteSpace(f.FacetName));
            if (!Filters.Any())
                return null;
            var lastFacet = Filters.Last();
            var format = "({0})";
            var oDataBuilder = new StringBuilder();
            foreach (var filter in Filters)
            {
                oDataBuilder.Append(string.Format(format, filter.ToOData()));
                if (!Equals(filter, lastFacet))
                    oDataBuilder.Append(" and ");
            }
            return oDataBuilder.ToString() != string.Empty ? oDataBuilder.ToString() : null;
        }
    }
    public class AppliedFilters
    {
        public string FacetName { get; set; }
        public bool IsMultiple { get; set; }
        public List<string> Values { get; set; }
        public string ToOData()
        {
            var oDataBuilder = new StringBuilder();
            var lastFilterValue = Values.Last();
            var format = IsMultiple ? "{0}/any(t:t eq '{1}')" : "{0} eq '{1}'";
            foreach (var value in Values)
            {
                oDataBuilder.Append(string.Format(format, FacetName, value));
                if (value != lastFilterValue)
                    oDataBuilder.Append(" or ");
            }
            return oDataBuilder.ToString();
        }
    }
}