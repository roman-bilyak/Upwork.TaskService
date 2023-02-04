CREATE PROCEDURE [dbo].[spDeleteTask]
    @Id NVARCHAR(50)
AS
    DELETE FROM [dbo].[tblTask]
    WHERE Id = @Id
RETURN 0