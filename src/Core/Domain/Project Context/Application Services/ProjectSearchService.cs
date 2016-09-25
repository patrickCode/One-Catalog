using Microsoft.Catalog.Azure.Search.Interfaces;
using Microsoft.Catalog.Azure.Search.Models;
using Microsoft.Catalog.Azure.Search.Models.SearchResponseMetadata;
using Microsoft.Catalog.Common.Models.Search;
using Microsoft.Catalog.Domain.ProjectContext.Interfaces;
using Microsoft.Catalog.Domain.ProjectContext.Aggregates;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Microsoft.Catalog.Domain.ProjectContext.ApplicationServices
{
    public class ProjectSearchService: IProjectSearchService
    {
        private readonly IAzureSearchContext _searchContext;
        public ProjectSearchService(IAzureSearchContext searchContext)
        {
            _searchContext = searchContext;
        }

        public ProjectSearchResult Search(string searchText, List<Technology> technologies, int skip, int top)
        {
            var searchParameter = new SearchParameters()
            {
                SearchText = searchText,
                AppliedFacets = new AppliedFacets()
                {
                    Filters = new List<AppliedFilters>()
                    {
                        new AppliedFilters()
                        {
                            FacetName = "Technologies",
                            IsMultiple = true,
                            Values = technologies.Select(tech => tech.Name).ToList()
                        }
                    }
                },
                Facets = new List<string>() { "Technologies" },
                SearchMetadata = new Metadata()
                {
                    IncludeCount = true,
                    Skip = skip,
                    Top = top
                }
            };
            var response = _searchContext.Search("index-project", searchParameter);
            return new ProjectSearchResult()
            {
                Results = GetProjects(response).ToList()
            };
        }

        private IEnumerable<Project> GetProjects(SearchResponse response)
        {
            foreach(var result in response.Results)
            {
                yield return new Project()
                {
                    Id = int.Parse(Get<string>(result, "Id")),
                    Name = Get<string>(result, "Name"),
                    Abstract = Get<string>(result, "Abstract"),
                    Description = Get<string>(result, "Description"),
                    Technologies = GetTechnologies(result).ToList(),
                    Contacts = GetContacts(result).ToList()
                };
            }
        }

        private IEnumerable<Technology> GetTechnologies(Document result)
        {
            var technologies = Get<JArray>(result, "Technologies");
            foreach(var technology in technologies)
            {
                yield return new Technology()
                {
                    Id = 0,
                    Name = technology.ToString()
                };
            }
        }

        private IEnumerable<User> GetContacts(Document result)
        {
            var contacts = Get<JArray>(result, "Contacts");
            foreach (var user in contacts)
            {
                yield return new User()
                {
                    Alias = user.ToString()
                };
            }
        }

        private T Get<T>(Document doc, string fieldName)
        {
            if (doc.ContainsKey(fieldName) && doc[fieldName] is T)
                return (T)doc[fieldName];
            return default(T);
        }
    }
}