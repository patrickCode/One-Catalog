using Microsoft.Catalog.Domain.ProjectContext.Aggregates;

namespace Microsoft.Catalog.Domain.ProjectContext.Interfaces
{
    public interface IProjectService
    {
        int AddProject(Project project);
        void UpdateProject(Project project);
        void DeleteProject(int id);
    }
}