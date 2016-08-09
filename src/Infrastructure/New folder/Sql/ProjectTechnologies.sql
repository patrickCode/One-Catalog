CREATE TABLE [dbo].[ProjectTechnologies]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [ProjectId] INT NOT NULL, 
    [TechnologyId] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [CreatedBy] NVARCHAR(300) NOT NULL, 
    [LastModifiedOn] DATETIME NOT NULL, 
    [LastModifiedBy] NVARCHAR(300) NOT NULL
)
