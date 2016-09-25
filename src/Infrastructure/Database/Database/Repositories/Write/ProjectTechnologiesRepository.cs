using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Write
{
    public class ProjectTechnologiesRepository: BaseRepository<ProjectTechnologies>
    {
        public ProjectTechnologiesRepository(OneCatalogDbContext dbContext): base(dbContext) { }
    }
}