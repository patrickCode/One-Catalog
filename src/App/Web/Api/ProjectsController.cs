using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Catalog.Domain.ProjectContext.Aggregates;
using Microsoft.Catalog.Domain.ProjectContext.Interfaces;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;

namespace Microsoft.Catalog.Web.Api
{
    [Route("api/projects")]
    public class ProjectsController: Controller
    {
        private readonly IProjectSearchService _searchService;
        private readonly IProjectQueryService _queryService;
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectSearchService projectSearchService, IProjectQueryService projectQueryService, IProjectService projectService)
        {
            _searchService = projectSearchService;
            _queryService = projectQueryService;
            _projectService = projectService;
        }

        [HttpGet]
        [Route("{id}")]
        public Project Get([FromRoute] int id)
        {
            return _queryService.Get(id);
        }   

        [HttpPost]
        public int Create([FromBody] Project project)
        {
            return _projectService.AddProject(project);
        }

        [HttpPut]
        public void Modify([FromBody] Project project)
        {
            _projectService.UpdateProject(project);
        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete([FromRoute] int id)
        {
            _projectService.DeleteProject(id);
        }

        [HttpGet]
        [Route("~/api/users/{userid}/projects")]
        public IEnumerable<Project> GetByContact([FromRoute] string userid)
        {
            return new List<Project>();
        }

        [HttpGet]
        public ProjectSearchResult Search([FromQuery]string q, [FromQuery]string[] technologies, [FromQuery]string[] contacts, [FromQuery]int skip=0, [FromQuery]int top=10)
        {
            var techFilters = technologies.Select(tech => new Technology() { Name = tech }).ToList();
            var contactFilters = contacts.Select(contact => new User() { Alias = contact });
            return _searchService.Search(q, techFilters, skip, top);
        }

        [HttpGet]
        [Route("suggest")]
        public IEnumerable<Project> Suggest(string q, string[] technologies, int top = 5)
        {
            var techFilters = technologies.Select(tech => new Technology() { Name = tech }).ToList();
            return _searchService.GetSuggestions(q, techFilters, top);
        } 
    }
}