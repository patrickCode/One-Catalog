using System.Linq;
using Database.Models;

namespace Microsoft.Catalog.Database.Repositories
{
    public class ProjectReadOnlyRepository : BaseReadOnlyRepository<Project>
    {
        public ProjectReadOnlyRepository(db_msonecatalogdevContext dbContext): base(dbContext) { }

        public override Project Get(object id)
        {
            var projectId = (int)id;
            return _dbContext.Set<Project>().FirstOrDefault(project => project.Id == projectId);
        }
    }
}