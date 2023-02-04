CREATE PROCEDURE [dbo].[spUpdateTask]
    @Id NVARCHAR(50),
    @Name NVARCHAR(100),
    @Description NVARCHAR(500),
    @DueDate DATETIME,
    @StartDate DATETIME,
    @EndDate DATETIME,
    @Priority SMALLINT,
    @Status SMALLINT
AS
    UPDATE [dbo].[tblTask]
    SET
        [Name] = @Name, 
        [Description] = @Description, 
        [DueDate] = @DueDate, 
        [StartDate] = @StartDate,
        [EndDate] = @EndDate,
        [Priority] = @Priority,
        [Status] = @Status
    WHERE Id = @Id

    SELECT
        [Id],
        [Name],
        [Description],
        [DueDate],
        [StartDate],
        [EndDate],
        [Priority],
        [Status] 
    FROM [dbo].[tblTask]
    WHERE Id = @Id
RETURN 0