using Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Write
{
    public class TechnologyRepository: BaseRepository<Technology>
    {
        public TechnologyRepository(db_msonecatalogdevContext dbContext): base(dbContext) { }
    }
}