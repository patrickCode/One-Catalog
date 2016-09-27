using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.Catalog.Azure.Search.Models;
using Microsoft.Catalog.Common.Models.Search;
using Microsoft.Catalog.Azure.Search.Interfaces;
using Microsoft.Catalog.Domain.ProjectContext.Interfaces;
using Microsoft.Catalog.Domain.ProjectContext.Aggregates;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;
using Microsoft.Catalog.Azure.Search.Models.SearchResponseMetadata;

namespace Microsoft.Catalog.Domain.ProjectContext.ApplicationServices
{
    public class ProjectSearchService : IProjectSearchService
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
            var facets = GetFacets(response, new List<string>() { "Technologies" }).ToList();
            return new ProjectSearchResult()
            {
                Results = GetProjects(response).ToList(),
                TotalCount = response.Count,
                Technologies = facets.FirstOrDefault(facet => facet.Name.Equals("Technologies"))
            };
        }

        private IEnumerable<Project> GetProjects(SearchResponse response)
        {
            foreach (var result in response.Results)
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
            foreach (var technology in technologies)
            {
                yield return new Technology()
                {
                    Id = 0,
                    Name = technology.ToString()
                };
            }
        }

        private IEnumerable<FacetInfo> GetFacets(SearchResponse response, List<string> facetNames)
        {
            var facets = response.Facets;
            if (facets == null)
                yield break;
            foreach (var facetName in facetNames)
            {
                if (!response.Facets.ContainsKey(facetName))
                    continue;
                var filterArray = (JArray)response.Facets[facetName];
                var filters = GetFilters(filterArray, false);
                yield return new FacetInfo()
                {
                    Name = facetName,
                    Filters = filters != null && filters.Any() ? filters.ToList() : null
                };
            }
        }

        private IEnumerable<Filter> GetFilters(JArray filterArray, bool range)
        {
            foreach (JObject filter in filterArray)
            {
                yield return new Filter()
                {
                    Name = filter["value"].ToString(),
                    Count = (int)filter["count"],
                    From = range ? filter["from"] : null,
                    To = range ? filter["to"] : null
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

        public IEnumerable<Project> GetSuggestions(string searchText, List<Technology> technologies, int top)
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
                Suggester = new SuggesterInfo()
                {
                    Name = "project_suggester",
                    IsFuzzyEnabled = true
                },
                SearchMetadata = new Metadata()
                {
                    ReturnFields = new List<string>() { "Name", "Abstract" },
                    IncludeCount = true,
                    Top = top
                }
            };
            var response = _searchContext.Suggest("index-project", searchParameter);
            return GetSuggestedProjects(response);
        }

        private IEnumerable<Project> GetSuggestedProjects(SuggestionResponse response)
        {
            foreach (var result in response.Results)
            {
                yield return new Project()
                {
                    Name = Get<string>(result, "Name"),
                    Abstract = Get<string>(result, "Abstract")
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