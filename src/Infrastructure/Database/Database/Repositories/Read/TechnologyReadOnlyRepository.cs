using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Read
{
    public class TechnologyReadOnlyRepository: BaseReadOnlyRepository<Technology>
    {
        public TechnologyReadOnlyRepository(OneCatalogDbContext dbContext): base(dbContext) { }
    }
}