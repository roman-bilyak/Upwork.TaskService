CREATE FUNCTION [dbo].[fnGetHolidays]
(
    @Year INT
)
RETURNS TABLE
AS
RETURN (  
    SELECT [Date], [dbo].[fnGetHoliday]([Date]) AS [Holiday]
    FROM (
        SELECT DATEADD(DAY, [number], convert(VARCHAR, @Year) + '-01-01') [Date]
        FROM [master]..[spt_values]
        WHERE [type] = 'p'
    ) AS D
    WHERE YEAR([Date]) = @year AND [dbo].[fnGetHoliday]([Date]) IS NOT NULL
)