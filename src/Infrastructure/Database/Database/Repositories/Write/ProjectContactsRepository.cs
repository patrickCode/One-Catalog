using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Write
{
    public class ProjectContactsRepository: BaseRepository<ProjectContact>
    {
        public ProjectContactsRepository(OneCatalogDbContext dbContext): base (dbContext) { }
    }
}