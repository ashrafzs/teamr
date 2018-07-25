create TABLE [dbo].[LeaveType]
(
	[Id] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY CLUSTERED,
	CreatedOn DATETIME NOT NULL,
	[UserId] INT NOT NULL,
	[Name] NVARCHAR(100) NOT NULL, 
    [Remarks] NVARCHAR(MAX) NULL, 
    [Quantity] DECIMAL(18, 2) NOT NULL,
	[Tag] VARCHAR(5) NOT NULL DEFAULT 'T', 
    CONSTRAINT [FK_dbo.LeaveType_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
)
