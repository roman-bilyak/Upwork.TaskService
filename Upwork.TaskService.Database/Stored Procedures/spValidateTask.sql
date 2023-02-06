CREATE PROCEDURE [dbo].[spValidateTask]
    @Id NVARCHAR(50),
    @Name NVARCHAR(100),
    @Description NVARCHAR(500),
    @DueDate DATE,
    @StartDate DATE,
    @EndDate DATE,
    @Priority SMALLINT,
    @Status SMALLINT
AS
    DECLARE @Error nvarchar(2048)

    IF @DueDate < DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0)
    BEGIN
        SET @Error  = '{ "Member": "DueDate", Message: "''DueDate'' cannot be in the past"}'
        ;THROW 50100, @Error, 1
    END

    IF ([dbo].[fnIsHoliday](@DueDate) = 1)
    BEGIN
        SET @Error  = '{ "Member": "DueDate", Message: "''DueDate'' cannot be on a holiday or weekend"}'
        ;THROW 50200, @Error, 1
    END

    DECLARE @MaxDueDateCount INT = 100
    DECLARE @CurrrentDueDateCount INT

	SELECT @CurrrentDueDateCount = COUNT(*)
    FROM [dbo].[tblTask]
    WHERE [Id] <> @Id AND [DueDate] = @DueDate AND [Priority] = 1 AND [Status] <> 3

    IF @CurrrentDueDateCount >= @MaxDueDateCount
    BEGIN
        SET @Error = '{ "Member": "DueDate", Message: "The system doesn''t allow more than ' + CAST(@MaxDueDateCount AS VARCHAR(10)) + ' High Priority tasks which have the same due date and are not finished"}'
        ;THROW 50300, @Error, 1
    END
RETURN 0