using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Catalog.Database.Repositories.Read;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Catalog.Database.Repositories.Write;
using Microsoft.Catalog.Common.Interfaces.Repository;
using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Test.Functional_Tests
{
    [TestClass]
    [TestCategory("Functional")]
    public class TechnologyWriteTests
    {
        IReadOnlyRepository<Technology> _technologyReadOnlyRepository;
        IRepository<Technology> _technologyRepository;
        public TechnologyWriteTests()
        {
            //Arrange
            #warning Use only Dev database connection string
            const string ConnectionString = "Server=tcp:sql-msonecatalogdev.database.windows.net,1433;Database=db-msonecatalogdev;Trusted_Connection=False;User ID=catalogdevadmin;Password=CltgServerdev#312";
            var dbOptionsBuilder = new DbContextOptionsBuilder<OneCatalogDbContext>().UseSqlServer(ConnectionString);
            var dbOptions = dbOptionsBuilder.Options;
            var dbContext = new OneCatalogDbContext(dbOptions);
            _technologyRepository = new TechnologyRepository(dbContext);
            _technologyReadOnlyRepository = new TechnologyReadOnlyRepository(dbContext);
        }

        [TestMethod]
        public void Database_Technology_AddNew()
        {
            //Arrange
            var technology = new Technology()
            {
                Name = "INTEGRATION_TEST_" + Guid.NewGuid().ToString(),
                Description = "Interation test description"
            };

            //Act
            var technologyId = _technologyRepository.CreateAndSave(technology, "SYS.Test");

            //Assert
            Assert.IsNotNull(technologyId);
            Assert.IsTrue(technologyId > 0);
            var addedTechnology = _technologyReadOnlyRepository.Get(technologyId);
            Assert.IsNotNull(addedTechnology);
            Assert.AreEqual(technology.Name, addedTechnology.Name);
            Assert.AreEqual(technology.Description, addedTechnology.Description);
        }
    }
}