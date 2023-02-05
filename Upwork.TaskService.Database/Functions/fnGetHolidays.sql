CREATE FUNCTION [dbo].[fnGetHolidays]
(
    @Year INT
)
RETURNS TABLE
AS
RETURN (  
    SELECT [Date], [dbo].[fnGetHoliday]([Date]) AS [Holiday]
    FROM (
        SELECT TOP 366 DATEADD(DAY, ROW_NUMBER () OVER (ORDER BY [column_id]), convert(VARCHAR, @Year) + '-01-01') [Date]
        FROM [master].[sys].[columns]
    ) AS D
    WHERE YEAR([Date]) = @year AND [dbo].[fnGetHoliday]([Date]) IS NOT NULL
)