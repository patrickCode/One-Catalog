using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Write
{
    public class TechnologyRepository: BaseRepository<Technology>
    {
        public TechnologyRepository(OneCatalogDbContext dbContext): base(dbContext) { }
    }
}