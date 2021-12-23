CREATE PROCEDURE UpdateUser
	@Id INT,
	@Email VARCHAR(30),
	@Name VARCHAR(20),
	@Surname VARCHAR(30),
	@Age INT
AS
BEGIN
	UPDATE [dbo].[User]
	SET Email = @Email,
		Name = @Name,
		Surname = @Surname,
		Age = @Age, 
		Active = 0
	WHERE Id = @Id
END