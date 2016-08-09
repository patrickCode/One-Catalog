using Database.Models;
using Microsoft.Catalog.Common.Interfaces.Repository;
using Microsoft.Catalog.Database.Repositories.Read;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Catalog.Database.Test.Functional_Tests
{
    [TestClass]
    [TestCategory("Functional")]
    public class TechnologyReadTests
    {
        IReadOnlyRepository<Technology> _technologyRepository;
        public TechnologyReadTests()
        {
            //Arrange
            #warning Use only Dev database connection string
            const string ConnectionString = "Server=tcp:sql-msonecatalogdev.database.windows.net,1433;Database=db-msonecatalogdev;Trusted_Connection=False;User ID=catalogdevadmin;Password=CltgServerdev#312";
            var dbOptionsBuilder = new DbContextOptionsBuilder<db_msonecatalogdevContext>().UseSqlServer(ConnectionString);
            var dbOptions = dbOptionsBuilder.Options;
            var dbContext = new db_msonecatalogdevContext(dbOptions);
            _technologyRepository = new TechnologyReadOnlyRepository(dbContext);
        }
        [TestMethod]
        public void Database_Technology_GetAll()
        {
            //Act
            var technologies = _technologyRepository.GetAll();

            //Assert
            Assert.IsNotNull(technologies);
            Assert.IsTrue(technologies.Any());
        }
    }
}