CREATE FUNCTION [dbo].[fnGetHoliday](@Date DATE)
RETURNS VARCHAR(50)
AS
BEGIN
    DECLARE @s VARCHAR(50)

    SELECT @s = CASE
        WHEN [dbo].[fnShiftHolidayToWorkday](CONVERT(varchar, [Year]  ) + '-01-01') = @Date THEN 'New Year'
        WHEN [dbo].[fnShiftHolidayToWorkday](CONVERT(varchar, [Year]+1) + '-01-01') = @Date THEN 'New Year'
        WHEN [dbo].[fnShiftHolidayToWorkday](CONVERT(varchar, [Year]  ) + '-07-04') = @Date THEN 'Independence Day'
        WHEN [dbo].[fnShiftHolidayToWorkday](CONVERT(varchar, [Year]  ) + '-12-25') = @Date THEN 'Christmas Day'
        WHEN [dbo].[fnShiftHolidayToWorkday](CONVERT(varchar, [Year]) + '-12-31') = @Date THEN 'New Years Eve'
        WHEN [dbo].[fnShiftHolidayToWorkday](CONVERT(varchar, [Year]) + '-11-11') = @Date THEN 'Veteran''s Day'

        WHEN [Month] = 1 AND [DayOfMonth] BETWEEN 15 AND 21 AND [DayName] = 'Monday' THEN 'Martin Luther King Day'
        WHEN [Month] = 5 AND [DayOfMonth] >= 25 AND [DayName] = 'Monday' THEN 'Memorial Day'
        WHEN [Month] = 9  AND [DayOfMonth] <= 7 AND [DayName] = 'Monday' THEN 'Labor Day'
        WHEN [Month] = 11 AND [DayOfMonth] BETWEEN 22 AND 28 AND [DayName] = 'Thursday' THEN 'Thanksgiving Day'
        WHEN [Month] = 11 AND [DayOfMonth] BETWEEN 23 AND 29 AND [DayName] = 'Friday' THEN 'Day After Thanksgiving'
        ELSE NULL END
    FROM (
        SELECT
            [Year] = YEAR(@Date),
            [Month] = MONTH(@Date),
            [DayOfMonth] = DAY(@Date),
            [DayName]   = DATENAME(weekday, @Date)
    ) c

    RETURN @s
END