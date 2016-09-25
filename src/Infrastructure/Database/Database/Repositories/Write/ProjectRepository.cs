using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories
{
    public class ProjectRepository: BaseRepository<Project>
    {
        public ProjectRepository(OneCatalogDbContext dbContext): base(dbContext) { }
    }
}