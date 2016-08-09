CREATE TABLE [dbo].[Link]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Type] NVARCHAR(300) NOT NULL, 
    [Href] NVARCHAR(300) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [ProjectId] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [CreatedBy] NVARCHAR(300) NOT NULL, 
    [LastModifiedOn] DATETIME NOT NULL, 
    [LastModifiedBy] NVARCHAR(300) NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0
)
