using Microsoft.Catalog.Common.Interfaces.Repository;
using Microsoft.Catalog.Database.Models;
using Microsoft.Catalog.Database.Repositories.Read;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Microsoft.Catalog.Database.Test.Functional_Tests
{
    [TestClass]
    [TestCategory("Functional")]
    public class LinkReadTests
    {
        IReadOnlyRepository<Link> _linkRepository;
        public LinkReadTests()
        {
            //Arrange
            #warning Use only Dev database connection string
            const string ConnectionString = "Server=tcp:sql-msonecatalogdev.database.windows.net,1433;Database=db-msonecatalogdev;Trusted_Connection=False;User ID=catalogdevadmin;Password=CltgServerdev#312";
            var dbOptionsBuilder = new DbContextOptionsBuilder<OneCatalogDbContext>().UseSqlServer(ConnectionString);
            var dbOptions = dbOptionsBuilder.Options;
            var dbContext = new OneCatalogDbContext(dbOptions);
            _linkRepository = new LinkReadOnlyRepository(dbContext);
        }

        [TestMethod]
        public void Database_Link_GetAll()
        {
            //Act
            var links = _linkRepository.GetAll();

            //Assert
            Assert.IsNotNull(links);
            Assert.IsTrue(links.Any());
        }
    }
}