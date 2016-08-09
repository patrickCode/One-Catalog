using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Catalog.Domain.ProjectContext.Aggregates;

namespace Microsoft.Catalog.Web.Api
{
    [Route("api/projects")]
    public class ProjectsController: Controller
    {
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
        public IEnumerable<Project> Search([FromQuery]string q, [FromQuery]string technologies, [FromQuery]string contacts)
        {
            return new List<Project>();
        }

        [HttpGet]
        [Route("suggest")]
        public IEnumerable<Project> Suggest(string q, string technologies, int top)
        {
            return new List<Project>();
        } 
    }
}