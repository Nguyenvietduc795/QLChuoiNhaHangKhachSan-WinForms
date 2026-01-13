uSE [QuanLyChuoiNhaHangKhachSan]
IF OBJECT_ID('dbo.usp_WarehouseVoucher_NextCode','P') IS NOT NULL
    DROP PROCEDURE dbo.usp_WarehouseVoucher_NextCode;
GO
CREATE PROCEDURE dbo.usp_WarehouseVoucher_NextCode
    @Prefix NVARCHAR(2),
    @NextCode NVARCHAR(30) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

    DECLARE @NextNumber INT;
    ;WITH cte AS (
        SELECT MAX(CAST(SUBSTRING(VoucherCode, 3, 10) AS INT)) AS MaxNum
        FROM dbo.WarehouseVoucher WITH (UPDLOCK, HOLDLOCK)
        WHERE VoucherCode LIKE @Prefix + '%'
    )
    SELECT @NextNumber = ISNULL(MaxNum, 0) + 1 FROM cte;

    SET @NextCode = @Prefix + RIGHT('000' + CAST(@NextNumber AS VARCHAR(3)), 3);

    COMMIT TRANSACTION;
END
GO
