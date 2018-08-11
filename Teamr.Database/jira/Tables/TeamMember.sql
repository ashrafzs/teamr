CREATE TABLE [jira].[TeamMember]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [UserId] INT NOT NULL, 
    CONSTRAINT [FK_Team_UserId_ToUser_Id] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id])
)
