CREATE OR ALTER PROCEDURE dbo.usp_Ingredient_GetById
    @IngredientID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IngredientID,
           IngredientCode,
           IngredientName,
           Unit,
           DefaultPrice,
           StockQuantity,
           MinStock,
           LastUpdated,
           IsActive
    FROM dbo.Ingredient
    WHERE IngredientID = @IngredientID;
END
GO
