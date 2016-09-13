using System.Collections.Generic;
using Microsoft.Catalog.Common.Enums;

namespace Microsoft.Catalog.Common.Models.Search
{
    public class SearchParameters
    {
        public string SearchText { get; set; }
        public string FilterExpression { get; set; }
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
}