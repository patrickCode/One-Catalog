CREATE TABLE [dbo].[Technology]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(300) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [CreatedOn] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(300) NOT NULL, 
    [LastModifiedOn] DATETIME NOT NULL, 
    [LastModifiedBy] NVARCHAR(300) NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0
)
