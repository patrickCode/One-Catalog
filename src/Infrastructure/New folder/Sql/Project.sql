CREATE TABLE [dbo].[Project]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(255) NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [Abstract] NVARCHAR(300) NULL, 
    [AdditionalDetail] NVARCHAR(MAX) NULL, 
    [Technologies] NVARCHAR(MAX) NULL, 
    [Contacts] NVARCHAR(MAX) NULL, 
    [CodeLink] NVARCHAR(300) NULL, 
    [PreviewLink] NVARCHAR(300) NULL, 
    [AdditionalLinks] NVARCHAR(MAX) NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [CreatedBy] NVARCHAR(300) NOT NULL, 
    [LastModifiedOn] DATETIME NOT NULL, 
    [LastModifiedBy] NVARCHAR(300) NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0
)
