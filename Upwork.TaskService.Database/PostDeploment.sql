INSERT INTO [dbo].[fxPriority]([Id], [Name])
SELECT 1, 'High'
UNION ALL
SELECT 2, 'Medium'
UNION ALL
SELECT 3, 'Low'
GO

INSERT INTO [dbo].[fxStatus]([Id], [Name])
SELECT 1, 'New'
UNION ALL
SELECT 2, 'In Progress'
UNION ALL
SELECT 3, 'Finished'
GO