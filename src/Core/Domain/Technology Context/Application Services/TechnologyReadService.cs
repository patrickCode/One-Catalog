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