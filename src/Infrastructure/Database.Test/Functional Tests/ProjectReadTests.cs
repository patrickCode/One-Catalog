using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Catalog.Database.Models;
using Microsoft.Catalog.Database.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Catalog.Common.Interfaces.Repository;

namespace Microsoft.Catalog.Database.Test.Functional_Tests
{
    [TestClass]
    [TestCategory("Functional")]
    public class ProjectReadTests
    {
        IReadOnlyRepository<Project> _projectRepository;
        public ProjectReadTests()
        {
            //Arrange
            #warning Use only Dev database connection string
            const string ConnectionString = "Server=tcp:sql-msonecatalogdev.database.windows.net,1433;Database=db-msonecatalogdev;Trusted_Connection=False;User ID=catalogdevadmin;Password=CltgServerdev#312";
            var dbOptionsBuilder = new DbContextOptionsBuilder<OneCatalogDbContext>().UseSqlServer(ConnectionString);
            var dbOptions = dbOptionsBuilder.Options;
            var dbContext = new OneCatalogDbContext(dbOptions);
            _projectRepository = new ProjectReadOnlyRepository(dbContext);
        } 

        [TestMethod]
        public void Database_Project_ReadAll()
        {
            //Act
            var projects = _projectRepository.GetAll();

            //Assert
            Assert.IsNotNull(projects);
            Assert.IsTrue(projects.Any());

            //Analyze
            AnalyzeProjects(projects);
        }

        [TestMethod]
        public void Database_Project_GetById()
        {
            //Arrange
            const int Id = 1;

            //Act
            var project = _projectRepository.Get(Id);

            //Assert
            Assert.IsNotNull(project);
            Assert.AreEqual(Id, project.Id);

            //Analyze
            AnalyzeProject(project);
        }

        [TestMethod]
        public void Database_Project_GetByFilter()
        {
            //Arrange
            const string filterText = "pratikb@microsoft.com";

            //Act
            var projects = _projectRepository.Get((p => p.Contacts.Equals(filterText)));

            //Assert
            Assert.IsNotNull(projects);
            Assert.IsTrue(projects.Any());
            foreach (var project in projects)
            {
                Assert.IsTrue(project.Contacts.Equals(filterText));
            }

            //Analyze
            AnalyzeProjects(projects);
        }

        [TestMethod]
        public void Database_Project_GetCount()
        {
            //Act
            var count = _projectRepository.GetCount();

            //Assert
            Assert.IsNotNull(count);
            Assert.IsTrue(count > 0);

            //Analyze
            Debug.WriteLine(count);
        }

        #region Helper
        private void AnalyzeProjects(IEnumerable<Project> projects)
        {
            foreach (var project in projects)
                AnalyzeProject(project);
        }

        private void AnalyzeProject(Project project)
        {
            Debug.WriteLine(string.Format("ID - {0}, Name - {1}, Description - {2}", project.Id, project.Name, project.Description));
        }
        #endregion
    }
}
