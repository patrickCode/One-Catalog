using System.Collections.Generic;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;

namespace Microsoft.Catalog.Domain.ProjectContext.Aggregates
{
    public class ProjectSearchResult
    {
        public List<Project> Results { get; set; }
        public int TotalCount { get; set; }
        public Facet Technologies { get; set; }
        public ProjectSearchResult()
        {
            Results = new List<Project>();
            Technologies = new Facet()
            {
                Name = "Technology"
            };
        }
        public ProjectSearchResult(List<Project> searchResults, Facet technologies)
        {
            Results = searchResults;
            Technologies = technologies;
        }
    }
}