CREATE FUNCTION [dbo].[fnShiftHolidayToWorkday](@Date date)
RETURNS DATE
AS
BEGIN
    IF DATENAME( dw, @Date ) = 'Saturday'
        SET @Date = DATEADD(day, - 1, @Date)

    ELSE IF DATENAME( dw, @Date ) = 'Sunday'
        SET @Date = DATEADD(day, 1, @Date)

    RETURN @Date
END
