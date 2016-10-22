using System.Collections.Generic;
using Domain.TechnologyContext.Aggregates;

namespace Microsoft.Catalog.Domain.TechnologyContext.Interfaces
{
    public interface ITechnologyReadService
    {
        IEnumerable<Technology> Get();
        Technology Get(string name);
        IEnumerable<Technology> Get(string[] names);
        IEnumerable<Technology> Suggest(string name);

    }
}