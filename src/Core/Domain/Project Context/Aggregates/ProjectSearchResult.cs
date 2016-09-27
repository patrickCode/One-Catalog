using System.Collections.Generic;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;

namespace Microsoft.Catalog.Domain.ProjectContext.Aggregates
{
    public class ProjectSearchResult
    {
        public List<Project> Results { get; set; }
        public long TotalCount { get; set; }
        public FacetInfo Technologies { get; set; }
        public ProjectSearchResult()
        {
            Results = new List<Project>();
            Technologies = new FacetInfo()
            {
                Name = "Technology"
            };
        }
        public ProjectSearchResult(List<Project> searchResults, FacetInfo technologies)
        {
            Results = searchResults;
            Technologies = technologies;
        }
    }
}