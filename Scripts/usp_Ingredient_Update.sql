CREATE OR ALTER PROCEDURE dbo.usp_Ingredient_Update
    @IngredientID   INT,
    @IngredientName NVARCHAR(200),
    @Unit           NVARCHAR(50),
    @DefaultPrice   DECIMAL(18, 2),
    @StockQuantity  DECIMAL(18, 2),
    @MinStock       DECIMAL(18, 2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Ingredient
    SET IngredientName = @IngredientName,
        Unit = @Unit,
        DefaultPrice = @DefaultPrice,
        StockQuantity = @StockQuantity,
        MinStock = @MinStock,
        LastUpdated = SYSDATETIME()
    WHERE IngredientID = @IngredientID;
END
GO
