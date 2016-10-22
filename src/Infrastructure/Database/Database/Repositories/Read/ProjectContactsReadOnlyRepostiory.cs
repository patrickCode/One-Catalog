using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Read
{
    public class ProjectContactsReadOnlyRepostiory: BaseReadOnlyRepository<ProjectContact>
    {
        public ProjectContactsReadOnlyRepostiory(OneCatalogDbContext dbContext): base(dbContext) { }
    }
}