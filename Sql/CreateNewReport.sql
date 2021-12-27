CREATE PROCEDURE CreateNewReport
	@UserId INT,
	@Date DATETIME2(7),
	@Result INT
AS
BEGIN
	INSERT INTO [dbo].[Report] (UserId, Date, Result)
	VALUES (@UserId, @Date, @Result)
END