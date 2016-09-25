using Microsoft.Catalog.Domain.ProjectContext.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Catalog.Domain.ProjectContext.Aggregates;
using Microsoft.Catalog.Common.Interfaces.Repository;
using Model = Microsoft.Catalog.Database.Models;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;
using Newtonsoft.Json.Linq;

namespace Microsoft.Catalog.Domain.ProjectContext.ApplicationServices
{
    public class ProjectQueryService : IProjectQueryService
    {
        private readonly IReadOnlyRepository<Model.Project> _projectRepository;
        private readonly IReadOnlyRepository<Model.ProjectTechnologies> _projTechRepository;
        private readonly IReadOnlyRepository<Model.Technology> _techRepository;
        public ProjectQueryService(IReadOnlyRepository<Model.Project> projectRepository, IReadOnlyRepository<Model.ProjectTechnologies> projTechRepository, IReadOnlyRepository<Model.Technology> techRepository)
        {
            _projectRepository = projectRepository;
            _projTechRepository = projTechRepository;
            _techRepository = techRepository;
        }
        public Project Get(int id)
        {
            var project = _projectRepository.Get(id);
            return new Project()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Abstract = project.Abstract,
                AdditionalDetails = project.AdditionalDetail,
                PreviewLink = string.IsNullOrEmpty(project.PreviewLink)? null : new PreviewLink(new Uri(project.PreviewLink)),
                Contacts = GetContacts(project).ToList(),
                CodeLink = string.IsNullOrEmpty(project.CodeLink) ? null: new CodeLink(new Uri(project.CodeLink)),
                Technologies = GetTechnologies(project).ToList(),
                AdditionalLinks = GetLinks(project).ToList(),
                CreatedBy = new User(project.CreatedBy, string.Empty),
                CreatedOn = project.CreatedOn
            };
        }

        private IEnumerable<User> GetContacts(Model.Project project)
        {
            if (string.IsNullOrEmpty(project.Contacts))
                yield break;
            var aliasList = JArray.Parse(project.Contacts);
            foreach(var alias in aliasList)
            {
                yield return new User(alias.ToString(), string.Empty);
            }
        }

        private IEnumerable<Technology> GetTechnologies(Model.Project project)
        {
            var projTechMap = _projTechRepository.Get(filter: (e) => e.ProjectId == project.Id);
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
            if (string.IsNullOrEmpty(project.AdditionalLinks))
                yield break;
            foreach (var link in project.AdditionalLinks.Split(','))
            {
                yield return new Link("Additional Link", new Uri(link));
            }
        }
    }
}