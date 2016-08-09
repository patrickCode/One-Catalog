using Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Read
{
    public class ProjectTechnologiesReadOnlyRepository: BaseReadOnlyRepository<ProjectTechnologies>
    {
        public ProjectTechnologiesReadOnlyRepository(db_msonecatalogdevContext dbContext): base(dbContext) { }
    }
}