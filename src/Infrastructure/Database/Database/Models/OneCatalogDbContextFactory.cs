using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.Catalog.Database.Models
{
    public class OneCatalogDbContextFactory : IDbContextFactory<OneCatalogDbContext>
    {
        public OneCatalogDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OneCatalogDbContext>();
            optionsBuilder.UseSqlServer(@"Server=tcp:sql-msonecatalogdev.database.windows.net,1433;Database=dbmsonecatalogdev;Trusted_Connection=False;User ID=catalogdevadmin;Password=CltgServerdev#312");

            return new OneCatalogDbContext(optionsBuilder.Options);
        }
    }
}