CREATE OR ALTER PROCEDURE dbo.usp_KPI_TotalStockQuantity
AS
BEGIN
    SET NOCOUNT ON;

    SELECT SUM(s.StockQuantity) AS TotalStockQuantity
    FROM (
        SELECT ISNULL(e.StockQuantity, 0) AS StockQuantity
        FROM dbo.Equipment e
        WHERE ISNULL(e.IsActive, 1) = 1

        UNION ALL

        SELECT ISNULL(i.StockQuantity, 0)
        FROM dbo.Ingredient i
        WHERE ISNULL(i.IsActive, 1) = 1
    ) AS s;
END
GO
