using Database.Models;

namespace Microsoft.Catalog.Database.Repositories
{
    public class ProjectRepository: BaseRepository<Project>
    {
        public ProjectRepository(db_msonecatalogdevContext dbContext): base(dbContext) { }
    }
}