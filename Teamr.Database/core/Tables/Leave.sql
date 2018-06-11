CREATE TABLE [dbo].Leave
(
	[Id] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY CLUSTERED,
	CreatedOn DATETIME NOT NULL,
	[CreatedByUserId] INT NOT NULL,
    [Notes] NVARCHAR(MAX) NULL, 
    [LeaveTypeId] INT NOT NULL,
    [ScheduledOn] DATETIME NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_dbo.Leave_dbo.User_LeaveTypeId] FOREIGN KEY ([LeaveTypeId]) REFERENCES [dbo].[LeaveType] ([Id]),
	CONSTRAINT [FK_dbo.Leave_dbo.AspNetUsers_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
)
