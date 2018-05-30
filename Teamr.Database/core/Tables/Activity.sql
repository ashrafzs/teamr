CREATE TABLE [dbo].[Activity]
(
	[Id] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY CLUSTERED,
	CreatedOn DATETIME NOT NULL,
	[CreatedByUserId] INT NOT NULL,
	[PerformedOn] DATETIME NULL, 
    [Notes] NVARCHAR(MAX) NULL, 
    [Quantity] DECIMAL(18, 2) NOT NULL, 
    [ActivityTypeId] INT NOT NULL,
    CONSTRAINT [FK_dbo.Activity_dbo.User_ActivityTypeId] FOREIGN KEY ([ActivityTypeId]) REFERENCES [dbo].[ActivityType] ([Id]),
	CONSTRAINT [FK_dbo.Activity_dbo.AspNetUsers_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
)
