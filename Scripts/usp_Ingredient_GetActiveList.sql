CREATE OR ALTER PROCEDURE dbo.usp_Ingredient_GetActiveList
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
           LastUpdated
    FROM dbo.Ingredient
    WHERE ISNULL(IsActive, 1) = 1
    ORDER BY IngredientCode;
END
GO
