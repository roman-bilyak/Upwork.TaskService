CREATE PROCEDURE [dbo].[spInsertTask]
	@Id NVARCHAR(50),
	@Name NVARCHAR(100),
	@Description NVARCHAR(500),
	@DueDate DATETIME,
	@StartDate DATETIME,
	@EndDate DATETIME,
	@Priority SMALLINT,
	@Status SMALLINT
AS
    INSERT INTO [dbo].[tblTask]
	(
		[Id],
		[Name],
		[Description],
		[DueDate],
		[StartDate],
		[EndDate],
		[Priority],
		[Status]
	)
    VALUES
	(
		@Id,
		@Name,
		@Description,
		@DueDate,
		@StartDate,
		@EndDate,
		@Priority,
		@Status
	)

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