CREATE PROCEDURE [dbo].[spGetAllTasks]
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
RETURN 0