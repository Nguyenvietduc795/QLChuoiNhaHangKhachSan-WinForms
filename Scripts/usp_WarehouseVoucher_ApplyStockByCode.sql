CREATE OR ALTER PROCEDURE dbo.usp_WarehouseVoucher_ApplyStockByCode
    @VoucherCode VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @VoucherID INT;
    DECLARE @VoucherType VARCHAR(10);
    DECLARE @WarehouseType VARCHAR(20);

    SELECT @VoucherID = VoucherID,
           @VoucherType = UPPER(RTRIM(LTRIM(ISNULL(VoucherType,'')))),
           @WarehouseType = UPPER(RTRIM(LTRIM(ISNULL(WarehouseType,''))))
    FROM dbo.WarehouseVoucher
    WHERE VoucherCode = @VoucherCode;

    IF @VoucherID IS NULL
    BEGIN
        THROW 50031, 'Voucher not found.', 1;
    END

    IF @VoucherType = 'IMPORT'
    BEGIN
        IF @WarehouseType = 'INGREDIENT'
        BEGIN
            UPDATE i
            SET i.StockQuantity = ISNULL(i.StockQuantity, 0) + ISNULL(d.Quantity, 0),
                i.LastUpdated = SYSDATETIME()
            FROM dbo.Ingredient i
            INNER JOIN dbo.WarehouseVoucherDetail d ON d.IngredientID = i.IngredientID
            WHERE d.VoucherID = @VoucherID;
        END
        ELSE IF @WarehouseType = 'EQUIPMENT'
        BEGIN
            UPDATE e
            SET e.StockQuantity = ISNULL(e.StockQuantity, 0) + ISNULL(d.Quantity, 0),
                e.LastUpdated = SYSDATETIME()
            FROM dbo.Equipment e
            INNER JOIN dbo.WarehouseVoucherDetail d ON d.EquipmentID = e.EquipmentID
            WHERE d.VoucherID = @VoucherID;
        END
    END
    ELSE IF @VoucherType = 'EXPORT'
    BEGIN
        IF @WarehouseType = 'INGREDIENT'
        BEGIN
            UPDATE i
            SET i.StockQuantity = ISNULL(i.StockQuantity, 0) - ISNULL(d.Quantity, 0),
                i.LastUpdated = SYSDATETIME()
            FROM dbo.Ingredient i
            INNER JOIN dbo.WarehouseVoucherDetail d ON d.IngredientID = i.IngredientID
            WHERE d.VoucherID = @VoucherID;
        END
        ELSE IF @WarehouseType = 'EQUIPMENT'
        BEGIN
            UPDATE e
            SET e.StockQuantity = ISNULL(e.StockQuantity, 0) - ISNULL(d.Quantity, 0),
                e.LastUpdated = SYSDATETIME()
            FROM dbo.Equipment e
            INNER JOIN dbo.WarehouseVoucherDetail d ON d.EquipmentID = e.EquipmentID
            WHERE d.VoucherID = @VoucherID;
        END
    END
END
GO
