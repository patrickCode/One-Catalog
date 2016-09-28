using System.Collections.Generic;
using Microsoft.Catalog.Domain.ProjectContext.Aggregates;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;
    
namespace Microsoft.Catalog.Domain.ProjectContext.Interfaces
{
    public interface IProjectSearchService
    {
        ProjectSearchResult Search(string searchText, List<Technology> technologies, List<User> contacts, int skip, int top);
        IEnumerable<Project> GetSuggestions(string searchText, List<Technology> technologies, int top);
    }
}