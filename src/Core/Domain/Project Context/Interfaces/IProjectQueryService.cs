using Microsoft.Catalog.Domain.ProjectContext.Aggregates;

namespace Microsoft.Catalog.Domain.ProjectContext.Interfaces
{
    public interface IProjectQueryService
    {
        Project Get(int id);
    }
}