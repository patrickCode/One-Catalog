using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Read
{
    public class LinkReadOnlyRepository: BaseReadOnlyRepository<Link>
    {
        public LinkReadOnlyRepository(OneCatalogDbContext dbContext): base(dbContext) { }
    }
}