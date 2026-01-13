IF OBJECT_ID('dbo.usp_WarehouseVoucher_Create','P') IS NOT NULL
    DROP PROCEDURE dbo.usp_WarehouseVoucher_Create;
GO
CREATE PROCEDURE dbo.usp_WarehouseVoucher_Create
    @VoucherType    VARCHAR(10),   -- 'IMPORT' | 'EXPORT'
    @WarehouseType  VARCHAR(20),   -- 'INGREDIENT' | 'EQUIPMENT'
    @BranchCode     VARCHAR(50),   -- RestaurantsCode ho?c HotelCode
    @Status         NVARCHAR(50),
    @Note           NVARCHAR(500) = NULL,
    @NewVoucherID   INT OUTPUT,
    @NewVoucherCode VARCHAR(50) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Prefix VARCHAR(2) = CASE UPPER(@VoucherType)
                                    WHEN 'IMPORT' THEN 'PN'
                                    WHEN 'EXPORT' THEN 'PX'
                                    ELSE 'XX'
                                 END;

    BEGIN TRANSACTION;
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

    DECLARE @NextNumber INT;

    ;WITH cte AS (
        SELECT MAX(CAST(SUBSTRING(VoucherCode, 3, 10) AS INT)) AS MaxNum
        FROM dbo.WarehouseVoucher WITH (UPDLOCK, HOLDLOCK)
        WHERE VoucherCode LIKE @Prefix + '%'
    )
    SELECT @NextNumber = ISNULL(MaxNum, 0) + 1 FROM cte;

    SET @NewVoucherCode = @Prefix + RIGHT('000' + CAST(@NextNumber AS VARCHAR(3)), 3);

    -- L?u theo lo?i kho: INGREDIENT -> RestaurantsCode, EQUIPMENT -> HotelCode
    INSERT INTO dbo.WarehouseVoucher (VoucherCode, VoucherType, WarehouseType, RestaurantsCode, HotelCode, Status, Note, CreatedDate)
    VALUES (@NewVoucherCode, @VoucherType, @WarehouseType,
            CASE WHEN UPPER(@WarehouseType) = 'INGREDIENT' THEN @BranchCode ELSE NULL END,
            CASE WHEN UPPER(@WarehouseType) = 'EQUIPMENT'  THEN @BranchCode ELSE NULL END,
            @Status, @Note, GETDATE());

    SET @NewVoucherID = SCOPE_IDENTITY();

    COMMIT TRANSACTION;
END
GO

IF COL_LENGTH('dbo.WarehouseVoucher','RestaurantsCode') IS NULL
    ALTER TABLE dbo.WarehouseVoucher ADD RestaurantsCode VARCHAR(50) NULL;

IF COL_LENGTH('dbo.WarehouseVoucher','HotelCode') IS NULL
    ALTER TABLE dbo.WarehouseVoucher ADD HotelCode VARCHAR(50) NULL;

IF COL_LENGTH('dbo.WarehouseVoucher','CreatedDate') IS NULL
    ALTER TABLE dbo.WarehouseVoucher ADD CreatedDate DATETIME2(0) NOT NULL
        CONSTRAINT DF_WarehouseVoucher_CreatedDate DEFAULT SYSDATETIME();



