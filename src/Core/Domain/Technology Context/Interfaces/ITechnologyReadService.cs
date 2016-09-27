using Domain.TechnologyContext.Aggregates;
using System.Collections.Generic;

namespace Microsoft.Catalog.Domain.TechnologyContext.Interfaces
{
    public interface ITechnologyReadService
    {
        IEnumerable<Technology> Get();
    }
}