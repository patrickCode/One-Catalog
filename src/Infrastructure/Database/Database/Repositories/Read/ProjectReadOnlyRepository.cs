using System.Linq;
using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Read
{
    public class ProjectReadOnlyRepository : BaseReadOnlyRepository<Project>
    {
        public ProjectReadOnlyRepository(OneCatalogDbContext dbContext): base(dbContext) { }

        public override Project Get(object id)
        {
            var projectId = (int)id;
            return _dbContext.Set<Project>().FirstOrDefault(project => project.Id == projectId);
        }
    }
}