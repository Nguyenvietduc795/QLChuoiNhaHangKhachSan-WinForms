CREATE OR ALTER PROCEDURE dbo.usp_KPI_StableStockCount
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(1) AS StableStockCount
    FROM (
        SELECT ISNULL(e.StockQuantity, 0) AS StockQuantity,
               ISNULL(e.MinStock, 0) AS MinStock
        FROM dbo.Equipment e
        WHERE ISNULL(e.IsActive, 1) = 1

        UNION ALL

        SELECT ISNULL(i.StockQuantity, 0),
               ISNULL(i.MinStock, 0)
        FROM dbo.Ingredient i
        WHERE ISNULL(i.IsActive, 1) = 1
    ) AS s
    WHERE s.StockQuantity >= s.MinStock;
END
GO
