using Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Write
{
    public class LinkRepository: BaseRepository<Link>
    {
        public LinkRepository(db_msonecatalogdevContext dbContext): base(dbContext) { }
    }
}