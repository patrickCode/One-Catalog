using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Catalog.Domain.ProjectContext.Aggregates;
using Microsoft.Catalog.Domain.ProjectContext.Interfaces;
using System.Linq;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;

namespace Microsoft.Catalog.Web.Api
{
    [Route("api/projects")]
    public class ProjectsController: Controller
    {
        private readonly IProjectSearchService _projectSearchService;
        public ProjectsController(IProjectSearchService projectSearchService)
        {
            _projectSearchService = projectSearchService;
        }
        [HttpGet]
        [Route("{id}")]
        public Project Get([FromRoute] int id)
        {
            return new Project();
        }   

        [HttpPost]
        public int Create([FromBody] Project project)
        {
            return -1;
        }

        [HttpPut]
        public void Modify([FromBody] Project project)
        {

        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete([FromRoute] int id)
        {

        }

        [HttpGet]
        [Route("~/api/users/{userid}/projects")]
        public IEnumerable<Project> GetByContact([FromRoute] string userid)
        {
            return new List<Project>();
        }

        [HttpGet]
        [Route("search")]
        public ProjectSearchResult Search([FromQuery]string q, [FromQuery]string[] technologies, [FromQuery]string[] contacts, [FromQuery]int skip=0, [FromQuery]int top=10)
        {
            var techFilters = technologies.Select(tech => new Technology() { Name = tech }).ToList();
            var contactFilters = contacts.Select(contact => new User() { Alias = contact });
            return _projectSearchService.Search(q, techFilters, skip, top);
        }

        [HttpGet]
        [Route("suggest")]
        public IEnumerable<Project> Suggest(string q, string technologies, int top)
        {
            return new List<Project>();
        } 

        
    }
}