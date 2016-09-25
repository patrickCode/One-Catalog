using System.Linq;
using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Read
{
    public class TechnologyReadOnlyRepository: BaseReadOnlyRepository<Technology>
    {
        public TechnologyReadOnlyRepository(OneCatalogDbContext dbContext): base(dbContext) { }
        public override Technology Get(object id)
        {
            var technologyId = (int)id;
            return _dbContext.Set<Technology>().FirstOrDefault(technology => technology.Id == technologyId);
        }
    }
}