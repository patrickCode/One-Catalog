using Microsoft.Catalog.Domain.ProjectContext.Interfaces;
using System;
using System.Linq;
using Microsoft.Catalog.Domain.ProjectContext.Aggregates;
using Model = Microsoft.Catalog.Database.Models;
using Microsoft.Catalog.Common.Interfaces.Repository;
using Microsoft.Catalog.Common.Exceptions;

namespace Microsoft.Catalog.Domain.ProjectContext.ApplicationServices
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Model.Project> _projectRepository;
        private readonly IRepository<Model.ProjectTechnologies> _projTechRepository;
        private readonly IReadOnlyRepository<Model.Technology> _technologyRepository;
        public ProjectService(IRepository<Model.Project> projectRepository, IRepository<Model.ProjectTechnologies> projTechRepository, IReadOnlyRepository<Model.Technology> technologyRepository)
        {
            _projectRepository = projectRepository;
            _projTechRepository = projTechRepository;
            _technologyRepository = technologyRepository;
        }

        public int AddProject(Project project)
        {
            int projectId = -1;
            var projEntity = new Model.Project()
            {
                Abstract = project.Abstract,
                AdditionalDetail = project.AdditionalDetails,
                AdditionalLinks = (project.AdditionalLinks == null || !project.AdditionalLinks.Any()) 
                                    ? null : string.Join(",", project.AdditionalLinks.Select(link => link.Href)),
                CodeLink = project.CodeLink != null ? project.CodeLink.Href.ToString() : null,
                Contacts = (project.Contacts == null || !project.Contacts.Any())
                            ? "[]" : string.Format("[{0}]", string.Join(",", project.Contacts.Select(contact => string.Format("\"{0}\"", contact.Alias)))),
                CreatedBy = project.CreatedBy.Alias,
                CreatedOn = DateTime.UtcNow,
                Description = project.Description,
                IsDeleted = false,
                LastModifiedBy = project.CreatedBy.Alias,
                LastModifiedOn = DateTime.UtcNow,
                Name = project.Name,
                PreviewLink = project.PreviewLink != null ? project.PreviewLink.Href.ToString(): null,
                Technologies = (project.Technologies == null || !project.Technologies.Any())
                                ? "[]": string.Format("[{0}]", string.Join(",", project.Technologies.Select(tech => string.Format("\"{0}\"", tech.Name))))
            };
            foreach (var technology in project.Technologies)
            {
                if (!(_technologyRepository.Exists(t => t.Id == technology.Id)))
                    throw new DomainException("Technology doesn't exist", new Exception());
            }
            projectId = _projectRepository.CreateAndSave(projEntity, projEntity.CreatedBy);
            var projTechEntities = project.Technologies.Select(tech =>
            {
                return new Model.ProjectTechnologies()
                {
                    ProjectId = projectId,
                    TechnologyId = tech.Id,
                    CreatedBy = project.CreatedBy.Alias,
                    CreatedOn = DateTime.UtcNow,
                    LastModifiedBy = project.CreatedBy.Alias,
                    LastModifiedOn = DateTime.UtcNow
                };
            });
            projTechEntities.ToList().ForEach(entity => _projTechRepository.Create(entity, entity.CreatedBy));
            _projTechRepository.Save();
            return projectId;
        }

        public void DeleteProject(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateProject(Project project)
        {
            throw new NotImplementedException();
        }
    }
}
