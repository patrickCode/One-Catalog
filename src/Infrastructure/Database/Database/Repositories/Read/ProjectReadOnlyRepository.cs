using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Read
{
    public class ProjectReadOnlyRepository : BaseReadOnlyRepository<Project>
    {
        public ProjectReadOnlyRepository(OneCatalogDbContext dbContext): base(dbContext) { }
    }
}