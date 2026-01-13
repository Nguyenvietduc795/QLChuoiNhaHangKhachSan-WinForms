CREATE OR ALTER PROCEDURE dbo.usp_WarehouseVoucher_GetDetailsByCode
    @VoucherCode VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT d.VoucherDetailID,
           d.VoucherID,
           d.IngredientID,
           d.EquipmentID,
           d.Quantity,
           d.UnitPrice,
           d.LineTotal,
           COALESCE(i.IngredientCode, e.EquipmentCode) AS ItemCode,
           COALESCE(i.IngredientName, e.EquipmentName) AS ItemName,
           COALESCE(i.Unit, e.Unit) AS Unit
    FROM dbo.WarehouseVoucherDetail d
    INNER JOIN dbo.WarehouseVoucher v ON v.VoucherID = d.VoucherID
    LEFT JOIN dbo.Ingredient i ON d.IngredientID = i.IngredientID
    LEFT JOIN dbo.Equipment e ON d.EquipmentID = e.EquipmentID
    WHERE v.VoucherCode = @VoucherCode
    ORDER BY d.VoucherDetailID;
END
GO
