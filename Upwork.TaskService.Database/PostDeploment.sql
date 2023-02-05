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

DECLARE @Year INT
SET @Year = YEAR(GETDATE())

WHILE @Year < YEAR(GETDATE()) + 5
BEGIN
    INSERT INTO [dto].[tblHoliday]([Date], [Holiday])
    SELECT [Date], [Holiday]
    FROM [dbo].[fnGetHolidays](@Year)

    SET @Year = @Year + 1
END