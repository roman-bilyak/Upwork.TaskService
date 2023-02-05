CREATE TABLE [dbo].[tblTask]
(
    [Id] NVARCHAR(50) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NOT NULL,
    [DueDate] DATE NOT NULL,
    [StartDate] DATE NOT NULL,
    [EndDate] DATE NOT NULL,
    [Priority] SMALLINT NOT NULL,
    [Status] SMALLINT NOT NULL, 
    CONSTRAINT [FK_tblTask_fxPriority] FOREIGN KEY ([Priority]) REFERENCES [dbo].[fxPriority]([Id]), 
    CONSTRAINT [FK_tblTask_fxStatus] FOREIGN KEY ([Status]) REFERENCES [dbo].[fxStatus]([Id]),
)