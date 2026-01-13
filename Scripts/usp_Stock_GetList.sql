CREATE OR ALTER PROCEDURE dbo.usp_Stock_GetList
    @WarehouseType NVARCHAR(20) = NULL,
    @Keyword NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    WITH Stock AS
    (
        SELECT
            'EQUIPMENT' AS ItemType,
            e.EquipmentID AS ItemId,
            e.EquipmentCode AS ItemCode,
            e.EquipmentName AS ItemName,
            e.Unit,
            ISNULL(e.DefaultPrice, 0) AS DefaultPrice,
            ISNULL(e.StockQuantity, 0) AS StockQuantity,
            ISNULL(e.MinStock, 0) AS MinStock,
            e.LastUpdated,
            ISNULL(e.IsActive, 1) AS IsActive
        FROM dbo.Equipment e

        UNION ALL

        SELECT
            'INGREDIENT' AS ItemType,
            i.IngredientID,
            i.IngredientCode,
            i.IngredientName,
            i.Unit,
            ISNULL(i.DefaultPrice, 0),
            ISNULL(i.StockQuantity, 0),
            ISNULL(i.MinStock, 0),
            i.LastUpdated,
            ISNULL(i.IsActive, 1)
        FROM dbo.Ingredient i
    )
    SELECT
        s.ItemCode,
        s.ItemName,
        s.Unit,
        s.StockQuantity,
        s.MinStock,
        CAST(s.MinStock AS NVARCHAR(50)) AS MiniStock,
        CAST(s.MinStock AS NVARCHAR(50)) AS MiniStockName,
        s.DefaultPrice,
        s.LastUpdated,
        s.ItemType,
        CASE
            WHEN s.IsActive = 0 THEN N'Ng?ng s? d?ng'
            WHEN s.StockQuantity <= 0 THEN N'H?t hàng'
            WHEN s.StockQuantity < s.MinStock THEN N'Thi?u'
            ELSE N'?n ??nh'
        END AS StatusText
    FROM Stock s
    WHERE (@WarehouseType IS NULL OR s.ItemType = @WarehouseType)
      AND (
            @Keyword IS NULL
            OR s.ItemCode LIKE '%' + @Keyword + '%'
            OR s.ItemName LIKE '%' + @Keyword + '%'
          )
    ORDER BY s.ItemCode;
END
GO
