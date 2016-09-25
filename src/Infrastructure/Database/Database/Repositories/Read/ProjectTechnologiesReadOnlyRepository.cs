using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Read
{
    public class ProjectTechnologiesReadOnlyRepository: BaseReadOnlyRepository<ProjectTechnologies>
    {
        public ProjectTechnologiesReadOnlyRepository(OneCatalogDbContext dbContext): base(dbContext) { }
    }
}