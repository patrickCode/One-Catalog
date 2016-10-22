using System.Linq;
using System.Collections.Generic;
using Domain.TechnologyContext.Aggregates;
using Model = Microsoft.Catalog.Database.Models;
using Microsoft.Catalog.Common.Interfaces.Repository;
using Microsoft.Catalog.Domain.TechnologyContext.Interfaces;

namespace Microsoft.Catalog.Domain.TechnologyContext.ApplicationServices
{
    public class TechnologyReadService : ITechnologyReadService
    {
        private readonly IReadOnlyRepository<Model.Technology> _techRepository;
        public TechnologyReadService(IReadOnlyRepository<Model.Technology> techRepository)
        {
            _techRepository = techRepository;
        }
        public IEnumerable<Technology> Get()
        {
            var entities = _techRepository.GetAll();
            return entities.Select(entity => ToDomain(entity));
        }

        public Technology Get(string name)
        {
            var entity = _techRepository.Get(tech => tech.Name.Equals(name)).FirstOrDefault();
            if (entity != null)
                return ToDomain(entity);
            return null;
        }

        public IEnumerable<Technology> Get(string[] names)
        {
            var entities = _techRepository.Get(tech => names.Any(n => n.Equals(tech.Name)));
            return entities.Select(entity => ToDomain(entity));
        }

        public IEnumerable<Technology> Suggest(string name)
        {
            var entities = _techRepository.Get(tech => tech.Name.StartsWith(name));
            return entities.Select(entity => ToDomain(entity));
        }

        private Technology ToDomain(Model.Technology entity)
        {
            return new Technology()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }
    }
}