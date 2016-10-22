using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Read
{
    public class UserReadOnlyRepository: BaseReadOnlyRepository<User>
    {
        public UserReadOnlyRepository(OneCatalogDbContext dbContext): base(dbContext) { }
    }
}