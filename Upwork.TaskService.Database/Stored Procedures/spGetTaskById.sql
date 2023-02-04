CREATE PROCEDURE [dbo].[spGetTaskById]
    @Id NVARCHAR(50)
AS
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