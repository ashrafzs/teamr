CREATE TABLE [dbo].[Issue]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Key] NVARCHAR(50) NOT NULL, 
    [ProjectId] INT NOT NULL, 
    [Type] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [InProgressOn] DATETIME NULL, 
    [CodeReviewOn] DATETIME NULL, 
    [ReadyForDeployOn] DATETIME NULL, 
    [ResolvedOn] DATETIME NULL, 
    [CreatedByUserId] INT NOT NULL, 
    [AssignedToUserId] INT NULL, 
    [ReviewedByUserId] INT NULL, 
    [Status] INT NOT NULL, 
    [StoryPoints] DECIMAL(18, 2) NULL, 
    CONSTRAINT [FK_Issue_ProjectId_Project_Id] FOREIGN KEY ([ProjectId]) REFERENCES [jira].[Project]([Id]), 
	CONSTRAINT [FK_Issue_CreatedByUserId_User_Id] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
	CONSTRAINT [FK_Issue_AssignedToUserId_User_Id] FOREIGN KEY ([AssignedToUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
	CONSTRAINT [FK_Issue_ReviewedByUserId_User_Id] FOREIGN KEY ([ReviewedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
)
