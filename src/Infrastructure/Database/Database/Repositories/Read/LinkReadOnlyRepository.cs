using System.Linq;
using Database.Models;

namespace Microsoft.Catalog.Database.Repositories.Read
{
    public class LinkReadOnlyRepository: BaseReadOnlyRepository<Link>
    {
        public LinkReadOnlyRepository(db_msonecatalogdevContext dbContext): base(dbContext) { }

        public override Link Get(object id)
        {
            var linkId = (int)id;
            return _dbContext.Set<Link>().FirstOrDefault(link => link.Id == linkId);
        }
    }
}