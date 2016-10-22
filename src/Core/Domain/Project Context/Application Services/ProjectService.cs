using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Catalog.Common.Exceptions;
using Model = Microsoft.Catalog.Database.Models;
using Microsoft.Catalog.Common.Interfaces.Repository;
using Microsoft.Catalog.Domain.ProjectContext.Aggregates;
using Microsoft.Catalog.Domain.ProjectContext.Interfaces;
using Microsoft.Catalog.Domain.ProjectContext.ValueObjects;

namespace Microsoft.Catalog.Domain.ProjectContext.ApplicationServices
{
    public class ProjectService : IProjectService
    {
        private readonly IReadOnlyRepository<Model.Project> _projectReadRepository;
        private readonly IRepository<Model.Project> _projectRepository;
        private readonly IReadOnlyRepository<Model.ProjectSummary> _projectSummaryReadRepository;
        private readonly IRepository<Model.ProjectSummary> _projectSummaryRepository;
        private readonly IRepository<Model.ProjectTechnologies> _projTechRepository;
        private readonly IReadOnlyRepository<Model.Link> _linkReadRepository;
        private readonly IRepository<Model.Link> _linkRepository;
        private readonly IReadOnlyRepository<Model.ProjectTechnologies> _projTechReadRepository;
        private readonly IReadOnlyRepository<Model.Technology> _technologyRepository;
        private readonly IReadOnlyRepository<Model.ProjectContact> _contactReadRepository;
        private readonly IRepository<Model.ProjectContact> _contactRepository;

        public ProjectService(IReadOnlyRepository<Model.Project> projectReadRepository,
            IRepository<Model.Project> projectRepository,
            IReadOnlyRepository<Model.ProjectSummary> projectSummaryReadRepository,
            IRepository<Model.ProjectSummary> projectSummaryRepository,
            IRepository<Model.ProjectTechnologies> projTechRepository,
            IReadOnlyRepository<Model.Link> linkReadRepository,
            IRepository<Model.Link> linkRepository,
            IReadOnlyRepository<Model.ProjectTechnologies> projTechReadRepository,
            IReadOnlyRepository<Model.Technology> technologyRepository,
            IReadOnlyRepository<Model.ProjectContact> contactReadRepository,
            IRepository<Model.ProjectContact> contactRepository)
        {
            _projectReadRepository = projectReadRepository;
            _projectRepository = projectRepository;
            _projectSummaryReadRepository = projectSummaryReadRepository;
            _projectSummaryRepository = projectSummaryRepository;
            _projTechRepository = projTechRepository;
            _linkReadRepository = linkReadRepository;
            _linkRepository = linkRepository;
            _projTechReadRepository = projTechReadRepository;
            _technologyRepository = technologyRepository;
            _contactReadRepository = contactReadRepository;
            _contactRepository = contactRepository;
        }

        public int AddProject(Project project)
        {
            #region Add Project
            int projectId = -1;
            //Check technologies
            foreach (var technology in project.Technologies)
            {
                var tech = _technologyRepository.Get(t => t.Name.Equals(technology.Name, StringComparison.CurrentCultureIgnoreCase) && !t.IsDeleted).FirstOrDefault();
                if (tech == null)
                    throw new DomainException("Technology doesn't exist", project.CreatedBy.Alias, 001);
                technology.Id = tech.Id;
            }

            var projEntity = new Model.Project()
            {
                Abstract = project.Abstract,
                AdditionalDetail = project.AdditionalDetails,
                CreatedBy = project.CreatedBy.Alias,
                CreatedOn = DateTime.UtcNow,
                Description = project.Description,
                IsDeleted = false,
                LastModifiedBy = project.CreatedBy.Alias,
                LastModifiedOn = DateTime.UtcNow,
                Name = project.Name
            };

            projectId = _projectRepository.CreateAndSave(projEntity, projEntity.CreatedBy);
            #endregion
            
            #region Add Technologies
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
            #endregion

            #region Add Links
            if (project.CodeLink != null && project.CodeLink.Href != null)
            {
                var codeLink = new Model.Link()
                {
                    Type = "Code",
                    Href = project.CodeLink.Href != null ? project.CodeLink.Href.ToString() : null,
                    Description = "Link to Code for the Project",
                    ProjectId = projectId,
                    CreatedBy = project.CreatedBy.Alias,
                    CreatedOn = DateTime.UtcNow,
                    LastModifiedBy = project.CreatedBy.Alias,
                    LastModifiedOn = DateTime.UtcNow,
                    IsDeleted = false
                };
                _linkRepository.Create(codeLink, project.CreatedBy.Alias);
            }

            if (project.PreviewLink != null && project.PreviewLink.Href != null)
            {
                var codeLink = new Model.Link()
                {
                    Type = "Preview",
                    Href = project.PreviewLink.Href != null ? project.PreviewLink.Href.ToString() : null,
                    Description = "Link to Preview the Project",
                    ProjectId = projectId,
                    CreatedBy = project.CreatedBy.Alias,
                    CreatedOn = DateTime.UtcNow,
                    LastModifiedBy = project.CreatedBy.Alias,
                    LastModifiedOn = DateTime.UtcNow,
                    IsDeleted = false
                };
                _linkRepository.Create(codeLink, project.CreatedBy.Alias);
            }

            if (project.AdditionalLinks != null && project.AdditionalLinks.Any())
            {
                foreach (var additionalLink in project.AdditionalLinks)
                {
                    if (additionalLink != null && additionalLink.Href != null)
                    {
                        var link = new Model.Link()
                        {
                            Type = additionalLink.LinkType,
                            Href = additionalLink.Href != null ? additionalLink.Href.ToString() : null,
                            Description = additionalLink.Desciption,
                            ProjectId = projectId,
                            CreatedBy = project.CreatedBy.Alias,
                            CreatedOn = DateTime.UtcNow,
                            LastModifiedBy = project.CreatedBy.Alias,
                            LastModifiedOn = DateTime.UtcNow,
                            IsDeleted = false
                        };
                        _linkRepository.Create(link);
                    }
                }
            }
            #endregion

            #region Add Contacts
            var contacts = project.Contacts.Select(contact =>
            {
                return new Model.ProjectContact()
                {
                    ProjectId = projectId,
                    Alias = contact.Alias,
                    IsDeleted = false,
                    CreatedBy = project.CreatedBy.Alias,
                    CreatedOn = DateTime.UtcNow,
                    LastModifiedBy = project.CreatedBy.Alias,
                    LastModifiedOn = DateTime.UtcNow
                };
            });
            contacts.ToList().ForEach(contact => _contactRepository.Create(contact));
            #endregion

            #region Add Project Summary
            var projSummary = new Model.ProjectSummary()
            {
                ProjectId = projectId,
                Abstract = project.Abstract,
                AdditionalDetail = project.AdditionalDetails,
                CodeLink = project.CodeLink != null && project.CodeLink.Href != null ? project.CodeLink.Href.ToString() : null,
                Contacts = (project.Contacts == null || !project.Contacts.Any())
                            ? "[]" : string.Format("[{0}]", string.Join(",", project.Contacts.Select(contact => string.Format("\"{0}\"", contact.Alias)))),
                CreatedBy = project.CreatedBy.Alias,
                CreatedOn = DateTime.UtcNow,
                Description = project.Description,
                IsDeleted = false,
                LastModifiedBy = project.CreatedBy.Alias,
                LastModifiedOn = DateTime.UtcNow,
                Name = project.Name,
                PreviewLink = project.PreviewLink != null && project.PreviewLink.Href != null ? project.PreviewLink.Href.ToString() : null,
                Technologies = (project.Technologies == null || !project.Technologies.Any())
                                ? "[]" : string.Format("[{0}]", string.Join(",", project.Technologies.Select(tech => string.Format("\"{0}\"", tech.Name))))
            };
            _projectSummaryRepository.Create(projSummary);
            #endregion

            //Saving the context
            _projectRepository.Save();
            return projectId;
        }

        public void DeleteProject(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateProject(Project project)
        {
            #region Update Project
            //Check Technology
            foreach (var technology in project.Technologies)
            {
                var tech = _technologyRepository.Get(t => t.Name.Equals(technology.Name, StringComparison.CurrentCultureIgnoreCase) && !t.IsDeleted).FirstOrDefault();
                if (tech == null)
                    throw new DomainException("Technology doesn't exist", project.CreatedBy.Alias, 001);
                technology.Id = tech.Id;
            }

            var existingProject = _projectReadRepository.Get(project.Id);
            if (existingProject == null)
                throw new DomainException(string.Format("Project with Id {0} doesn't exist", project.Id), new Exception());
            var projEntity = new Model.Project()
            {
                Id = project.Id,
                Abstract = project.Abstract,
                AdditionalDetail = project.AdditionalDetails,
                Description = project.Description,
                IsDeleted = false,
                LastModifiedBy = project.LastModifiedBy.Alias,
                LastModifiedOn = DateTime.UtcNow,
                Name = project.Name,
                CreatedBy = existingProject.CreatedBy,
                CreatedOn = existingProject.CreatedOn
            };
            _projectRepository.Update(projEntity, project.LastModifiedBy.Alias);
            #endregion
            
            #region Update Technology
            //New Mapping
            var currentEntities = project.Technologies.Select(tech =>
            {
                return new Model.ProjectTechnologies()
                {
                    ProjectId = project.Id,
                    TechnologyId = tech.Id,
                    CreatedBy = project.CreatedBy.Alias,
                    CreatedOn = DateTime.UtcNow,
                    LastModifiedBy = project.CreatedBy.Alias,
                    LastModifiedOn = DateTime.UtcNow
                };
            });

            var existingEntities = _projTechReadRepository.Get(ptmap => ptmap.ProjectId == project.Id && !ptmap.IsDeleted);

            //Delete
            var deletedTechnologies = existingEntities.Where(existing => !currentEntities.Any(current => current.TechnologyId == existing.TechnologyId));
            deletedTechnologies.ToList().ForEach(technology => _projTechRepository.Delete(technology, true));

            //Add
            var addedTechnologies = currentEntities.Where(current => !existingEntities.Any(existing => existing.TechnologyId == current.TechnologyId));
            addedTechnologies.ToList().ForEach(technology => _projTechRepository.Create(technology));

            #endregion

            #region Update Links
            var existingLinks = _linkReadRepository.Get(link => link.ProjectId == project.Id
                                                            && !link.IsDeleted);
            UpdateLink(project, existingLinks, "Code");
            UpdateLink(project, existingLinks, "Preview");

            var additionalExistingLinks = existingLinks.Where(link => link.Type != "Code" && link.Type != "Preview");
            //Update Additional Links
            foreach (var additionalLink in project.AdditionalLinks.Where(link => link.Href != null))
            {
                var existingLink = additionalExistingLinks.FirstOrDefault(link => link.Id == additionalLink.Id);

                //Update Link
                if (existingLink != null && (existingLink.Href != additionalLink.Href.ToString() || existingLink.Description != additionalLink.Desciption))
                {
                    var link = new Model.Link()
                    {
                        Id = existingLink.Id,
                        Description = additionalLink.Desciption,
                        Type = additionalLink.LinkType,
                        Href = additionalLink.Href.ToString(),
                        ProjectId = project.Id,
                        CreatedBy = existingLink.CreatedBy,
                        CreatedOn = existingLink.CreatedOn,
                        LastModifiedBy = project.LastModifiedBy.Alias,
                        LastModifiedOn = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    _linkRepository.Update(link, link.LastModifiedBy);
                }
                else
                {
                    var link = new Model.Link()
                    {
                        Description = additionalLink.Desciption,
                        Type = additionalLink.LinkType,
                        Href = additionalLink.Href.ToString(),
                        ProjectId = project.Id,
                        CreatedBy = project.LastModifiedBy.Alias,
                        CreatedOn = DateTime.UtcNow,
                        LastModifiedBy = project.LastModifiedBy.Alias,
                        LastModifiedOn = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    _linkRepository.Create(link, link.CreatedBy);
                }
            }
            var toBeDeletedLinks = additionalExistingLinks.Where(exitingLink => !project.AdditionalLinks.Exists(link => link.Id == exitingLink.Id));
            toBeDeletedLinks.ToList().ForEach(link =>
            {
                link.IsDeleted = true;
                link.LastModifiedBy = project.LastModifiedBy.Alias;
                link.LastModifiedOn = DateTime.UtcNow;
                _linkRepository.Update(link, link.LastModifiedBy);
            });

            #endregion

            #region Update Contacts
            var currentContacts = project.Contacts.Select(contact =>
            {
                return new Model.ProjectContact()
                {
                    ProjectId = project.Id,
                    Alias = contact.Alias,
                    CreatedBy = project.CreatedBy.Alias,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                    LastModifiedBy = project.CreatedBy.Alias,
                    LastModifiedOn = DateTime.UtcNow
                };
            });

            var existingContacts = _contactReadRepository.Get(contact => contact.ProjectId == project.Id && !contact.IsDeleted);

            //Delete
            var deletedContacts = existingContacts.Where(existing => !currentContacts.Any(current => current.Alias.Equals(existing.Alias)));
            deletedContacts.ToList().ForEach(contact => _contactRepository.Delete(contact, true));

            //Add
            var newContacts = currentContacts.Where(current => !existingContacts.Any(existing => existing.Alias.Equals(current.Alias)));
            newContacts.ToList().ForEach(contact => _contactRepository.Create(contact, project.CreatedBy.Alias));
            #endregion

            #region Update Summary
            var existingSummary = _projectSummaryReadRepository.Get(summary => summary.ProjectId == project.Id)
                    .FirstOrDefault();
            var projSummary = new Model.ProjectSummary()
            {
                Id = existingSummary.Id,
                ProjectId = existingSummary.ProjectId,
                Abstract = project.Abstract,
                AdditionalDetail = project.AdditionalDetails,
                CodeLink = project.CodeLink != null && project.CodeLink.Href != null 
                            ? project.CodeLink.Href.ToString() : null,
                Contacts = (project.Contacts == null || !project.Contacts.Any())
                            ? "[]" : string.Format("[{0}]", string.Join(",", project.Contacts.Select(contact => string.Format("\"{0}\"", contact.Alias)))),
                Description = project.Description,
                IsDeleted = false,
                LastModifiedBy = project.LastModifiedBy.Alias,
                LastModifiedOn = DateTime.UtcNow,
                Name = project.Name,
                PreviewLink = project.PreviewLink != null && project.PreviewLink.Href != null 
                                ? project.PreviewLink.Href.ToString() : null,
                Technologies = (project.Technologies == null || !project.Technologies.Any())
                                ? "[]" : string.Format("[{0}]", string.Join(",", project.Technologies.Select(tech => string.Format("\"{0}\"", tech.Name)))),
                CreatedBy = existingSummary.CreatedBy,
                CreatedOn = existingProject.CreatedOn
            };

            _projectSummaryRepository.Update(projSummary);
            #endregion

            //Saving the context
            _projectRepository.Save();
        }

        private void UpdateLink(Project project, IEnumerable<Model.Link> existingLinks, string linkType)
        {
            var existingLink = existingLinks.FirstOrDefault(link => link.Type.Equals(linkType));
            var projectLink = linkType.Equals("Code") ? project.CodeLink as Link : project.PreviewLink as Link;
            if (projectLink != null && projectLink.Href != null)
            {
                //Update the code link
                if (existingLink != null && existingLink.Href != projectLink.Href.ToString())
                {
                    var link = new Model.Link()
                    {
                        Id = existingLink.Id,
                        Type = linkType,
                        Description = projectLink.Desciption,
                        Href = project.CodeLink.Href.ToString(),
                        ProjectId = project.Id,
                        CreatedBy = existingLink.CreatedBy,
                        CreatedOn = existingLink.CreatedOn,
                        LastModifiedBy = project.LastModifiedBy.Alias,
                        LastModifiedOn = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    _linkRepository.Update(link, project.LastModifiedBy.Alias);
                }
                //Add new Link
                else if (existingLink == null)
                {
                    var link = new Model.Link()
                    {
                        Type = linkType,
                        Description = projectLink.Desciption,
                        Href = projectLink.Href.ToString(),
                        ProjectId = project.Id,
                        CreatedBy = project.LastModifiedBy.Alias,
                        CreatedOn = DateTime.UtcNow,
                        LastModifiedBy = project.LastModifiedBy.Alias,
                        LastModifiedOn = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    _linkRepository.Create(link, link.CreatedBy);
                }
            }
            else
            {
                //Delete
                if (existingLink != null)
                {
                    var link = existingLink;
                    link.LastModifiedBy = project.LastModifiedBy.Alias;
                    link.LastModifiedOn = DateTime.UtcNow;
                    link.IsDeleted = true;
                    _linkRepository.Update(link, link.LastModifiedBy);
                }
            }
        }
    }
}