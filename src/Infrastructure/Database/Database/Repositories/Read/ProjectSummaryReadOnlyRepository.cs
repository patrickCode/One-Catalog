using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Read
{
    public class ProjectSummaryReadOnlyRepository : BaseReadOnlyRepository<ProjectSummary>
    {
        public ProjectSummaryReadOnlyRepository(OneCatalogDbContext dbContext) : base(dbContext) { }
    }
}