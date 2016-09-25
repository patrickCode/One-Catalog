using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Catalog.Database.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Catalog.Common.Interfaces.Repository;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Catalog.Database.Models;

namespace Microsoft.Catalog.Database.Test.Functional_Tests
{
    [TestClass]
    public class ProjectWriteTests
    {
        private IRepository<Project> _projectRepository;
        private IReadOnlyRepository<Project> _projectReadOnlyRepository;
        public ProjectWriteTests()
        {
            //Arrange
            #warning Use only Dev database connection string
            const string ConnectionString = "Server=tcp:sql-msonecatalogdev.database.windows.net,1433;Database=db-msonecatalogdev;Trusted_Connection=False;User ID=catalogdevadmin;Password=CltgServerdev#312";
            var dbOptionsBuilder = new DbContextOptionsBuilder<OneCatalogDbContext>().UseSqlServer(ConnectionString);
            var dbOptions = dbOptionsBuilder.Options;
            var dbContext = new OneCatalogDbContext(dbOptions);
            _projectRepository = new ProjectRepository(dbContext);
            _projectReadOnlyRepository = new ProjectReadOnlyRepository(dbContext);
        }

        [TestMethod]
        public void Database_Project_AddNew()
        {
            //Arrange
            var project = new Project()
            {
                Name = "DB_INTERGRATION_TEST" + "_"+Guid.NewGuid().ToString(),
                Description = "Integration test description",
                Abstract = "Intgr test abstract",
                AdditionalDetail = "Integration test additional details",
                AdditionalLinks = "http://aka.ms",
                Contacts = "pratikb@microsoft.com",
                CodeLink = "http://github/patrickCode",
                Technologies = "ASP.NET Core 1.0, .NET Core Cli",
                PreviewLink = "http://msonecatalogdev.azurewebsites.net/"
            };

            //Act
            var projectId = _projectRepository.CreateAndSave(project, "SYS.Test");
            _projectRepository.Save();
            //Assert
            Assert.IsNotNull(projectId);
            Assert.IsTrue(projectId > 0);
            var createdProject = _projectReadOnlyRepository.Get(projectId);
            Assert.IsNotNull(projectId);

            //Analyze
            AnalyzeProject(project);
        }

        [TestMethod]
        public void Database_Project_Edit()
        {
            //Arrange
            const int ProjectId = 1;
            const string ModifiedBy = "SYS.Test_Edit";
            var projectName = "INTERGRATION_TEST_RENAMED_" + Guid.NewGuid().ToString();
            var projectDesc = "Intergration test description renamed " + Guid.NewGuid().ToString();
            var project = _projectReadOnlyRepository.Get(ProjectId);
            project.Name = projectName;
            project.Description = projectDesc;

            //Act
            _projectRepository.Update(project, ModifiedBy);
            _projectRepository.Save();

            //Assert
            var editedProject = _projectReadOnlyRepository.Get(ProjectId);
            Assert.IsNotNull(editedProject);
            Assert.AreEqual(ProjectId, editedProject.Id);
            Assert.AreEqual(projectName, editedProject.Name);
            Assert.AreEqual(projectDesc, editedProject.Description);

            //Analyze
            AnalyzeProject(editedProject);
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
