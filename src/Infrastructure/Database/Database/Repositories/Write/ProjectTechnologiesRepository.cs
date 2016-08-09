using Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Write
{
    public class ProjectTechnologiesRepository: BaseRepository<ProjectTechnologies>
    {
        public ProjectTechnologiesRepository(db_msonecatalogdevContext dbContext): base(dbContext) { }
    }
}