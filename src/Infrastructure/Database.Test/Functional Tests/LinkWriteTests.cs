using System;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Catalog.Database.Repositories.Read;
using Microsoft.Catalog.Database.Repositories.Write;
using Microsoft.Catalog.Common.Interfaces.Repository;

namespace Microsoft.Catalog.Database.Test.Functional_Tests
{
    [TestClass]
    [TestCategory("Functional")]
    public class LinkWriteTests
    {
        IReadOnlyRepository<Link> _linkReadOnlyRepository;
        IRepository<Link> _linkRepository;
        public LinkWriteTests()
        {
            //Arrange
            #warning Use only Dev database connection string
            const string ConnectionString = "Server=tcp:sql-msonecatalogdev.database.windows.net,1433;Database=db-msonecatalogdev;Trusted_Connection=False;User ID=catalogdevadmin;Password=CltgServerdev#312";
            var dbOptionsBuilder = new DbContextOptionsBuilder<db_msonecatalogdevContext>().UseSqlServer(ConnectionString);
            var dbOptions = dbOptionsBuilder.Options;
            var dbContext = new db_msonecatalogdevContext(dbOptions);
            _linkRepository = new LinkRepository(dbContext);
            _linkReadOnlyRepository = new LinkReadOnlyRepository(dbContext);
        }

        [TestMethod]
        public void Database_Link_AddNew()
        {
            //Arrange
            var link = new Link()
            {
                Href = "http://integrationtest.com",
                Description = "Integration test description " + Guid.NewGuid().ToString(),
                ProjectId = 1,
                Type = "Test"
            };

            //Act
            var linkId = _linkRepository.CreateAndSave(link, "SYS.Test");

            //Assert
            Assert.IsNotNull(linkId);
            Assert.IsTrue(linkId > 0);
            var addedLink = _linkReadOnlyRepository.Get(linkId);
            Assert.IsNotNull(addedLink);
            Assert.AreEqual(link.Href, addedLink.Href);
            Assert.AreEqual(link.Description, addedLink.Description);
            Assert.AreEqual(link.ProjectId, addedLink.ProjectId);
        }
    }

}
