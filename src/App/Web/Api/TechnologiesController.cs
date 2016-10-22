using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Domain.TechnologyContext.Aggregates;
using Microsoft.Catalog.Domain.TechnologyContext.Interfaces;

namespace Microsoft.Catalog.Web.Api
{
    [Route("api/technologies")]
    public class TechnologiesController: Controller
    {
        private readonly ITechnologyReadService _technologyReadService;
        public TechnologiesController(ITechnologyReadService technologyReadService)
        {
            _technologyReadService = technologyReadService;
        }

        [HttpGet]
        public IEnumerable<Technology> Get(string[] name)
        {
            if (name == null || !name.Any())
                return _technologyReadService.Get();
            return _technologyReadService.Get(name);
        }

        [HttpGet]
        [Route("suggest")]
        public IEnumerable<Technology> Suggest(string name)
        {
            return _technologyReadService.Suggest(name);
        }
    }
}