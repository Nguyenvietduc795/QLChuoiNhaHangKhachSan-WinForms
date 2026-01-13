CREATE OR ALTER PROCEDURE dbo.usp_Ingredient_Stop
    @IngredientID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Ingredient
    SET IsActive = 0,
        LastUpdated = SYSDATETIME()
    WHERE IngredientID = @IngredientID
      AND IsActive = 1;

    IF @@ROWCOUNT = 0
        THROW 50012, 'Ingredient not found or already inactive.', 1;
END
GO
