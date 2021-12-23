CREATE PROCEDURE CreateNewUser
	@Email VARCHAR(30),
	@Name VARCHAR(20),
	@Surname VARCHAR(30),
	@Age INT,
	@NewUserId INT OUTPUT
AS
BEGIN
	INSERT INTO [dbo].[User] (Email, Name, Surname, Age, Active)
	VALUES (@Email,	@Name, @Surname, @Age, 0)

	SELECT @NewUserId = @@Identity
END