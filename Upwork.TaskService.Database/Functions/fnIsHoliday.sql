CREATE FUNCTION [dbo].[fnIsHoliday]
(
	@Date DATE
)
RETURNS BIT
AS
BEGIN
	IF DATENAME(dw, @Date) = 'Sunday' OR DATENAME(dw, @Date) = 'Saturday'
	BEGIN
		RETURN 1
	END

	IF EXISTS (SELECT * FROM [dbo].[tblHoliday] WHERE [Date] = @Date)
	BEGIN
		RETURN 1
	END

	RETURN 0
END
