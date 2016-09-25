using Microsoft.Catalog.Domain.ProjectContext.Aggregates;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;
using System.Collections.Generic;

namespace Microsoft.Catalog.Domain.ProjectContext.Interfaces
{
    public interface IProjectSearchService
    {
        ProjectSearchResult Search(string searchText, List<Technology> technologies, int skip, int top);
    }
}