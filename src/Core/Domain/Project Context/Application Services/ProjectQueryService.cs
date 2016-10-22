using System;
using System.Linq;
using System.Collections.Generic;
using Model = Microsoft.Catalog.Database.Models;
using Microsoft.Catalog.Common.Interfaces.Repository;
using Microsoft.Catalog.Domain.ProjectContext.Interfaces;
using Microsoft.Catalog.Domain.ProjectContext.Aggregates;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;

namespace Microsoft.Catalog.Domain.ProjectContext.ApplicationServices
{
    public class ProjectQueryService : IProjectQueryService
    {
        private readonly IReadOnlyRepository<Model.Project> _projectRepository;
        private readonly IReadOnlyRepository<Model.ProjectTechnologies> _projTechRepository;
        private readonly IReadOnlyRepository<Model.Technology> _techRepository;
        private readonly IReadOnlyRepository<Model.Link> _linkRepository;
        private readonly IReadOnlyRepository<Model.ProjectContact> _contactRepository;

        public ProjectQueryService(
            IReadOnlyRepository<Model.Project> projectRepository,
            IReadOnlyRepository<Model.ProjectTechnologies> projTechRepository,
            IReadOnlyRepository<Model.Technology> techRepository,
            IReadOnlyRepository<Model.Link> linkRepository,
            IReadOnlyRepository<Model.ProjectContact> contactRepository)
        {
            _projectRepository = projectRepository;
            _projTechRepository = projTechRepository;
            _techRepository = techRepository;
            _linkRepository = linkRepository;
            _contactRepository = contactRepository;
        }

        public Project Get(int id)
        {
            var project = _projectRepository.Get(id);
            var links = GetLinks(project);
            return new Project()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Abstract = project.Abstract,
                AdditionalDetails = project.AdditionalDetail,
                PreviewLink = links.Any(link => link.LinkType.Equals("Preview")) ? new PreviewLink(links.First(link => link.LinkType.Equals("Preview"))) : null,
                Contacts = GetContacts(project).ToList(),
                CodeLink = links.Any(link => link.LinkType.Equals("Code")) ? new CodeLink(links.First(link => link.LinkType.Equals("Code"))) : null,
                Technologies = GetTechnologies(project).ToList(),
                AdditionalLinks = links.ToList(),
                CreatedBy = new User(project.CreatedBy, string.Empty),
                CreatedOn = project.CreatedOn
            };
        }

        private IEnumerable<User> GetContacts(Model.Project project)
        {
            var contacts = _contactRepository.Get(contact => contact.ProjectId == project.Id && !contact.IsDeleted);
            if (contacts == null || !contacts.Any())
                yield break;
            foreach (var contact in contacts)
                yield return new User(contact.Alias, string.Empty);
        }

        private IEnumerable<Technology> GetTechnologies(Model.Project project)
        {
            var projTechMap = _projTechRepository.Get(filter: e => e.ProjectId == project.Id && !e.IsDeleted);
            if (projTechMap == null || !projTechMap.Any())
                yield break;
            foreach (var map in projTechMap)
            {
                var technologyId = map.TechnologyId;
                var technology = _techRepository.Get(technologyId);
                yield return new Technology(technology.Id, technology.Name);
            }
        }
        private IEnumerable<Link> GetLinks(Model.Project project)
        {
            var links = _linkRepository.Get(link => link.ProjectId == project.Id);
            foreach (var link in links)
            {
                yield return new Link(link.Id, link.Type, new Uri(link.Href), link.Description);
            }
        }
    }
}