using Microsoft.EntityFrameworkCore;
using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Write
{
    public class ProjectSummaryRepository : BaseRepository<ProjectSummary>
    {
        public ProjectSummaryRepository(OneCatalogDbContext dbContext) : base(dbContext) { }
    }
}