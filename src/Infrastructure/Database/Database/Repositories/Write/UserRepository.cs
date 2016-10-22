using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Write
{
    public class UserRepository: BaseRepository<User>
    {
        public UserRepository(OneCatalogDbContext dbContext): base (dbContext) { }
    }
}