using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Write
{
    public class LinkRepository: BaseRepository<Link>
    {
        public LinkRepository(OneCatalogDbContext dbContext): base(dbContext) { }
    }
}