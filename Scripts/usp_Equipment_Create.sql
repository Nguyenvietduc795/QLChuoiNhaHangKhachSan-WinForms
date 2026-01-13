CREATE OR ALTER PROCEDURE dbo.usp_Equipment_Create
    @EquipmentName   NVARCHAR(200),
    @Unit            NVARCHAR(50),
    @DefaultPrice    DECIMAL(18, 2),
    @StockQuantity   DECIMAL(18, 2),
    @MinStock        DECIMAL(18, 2),
    @NewId           INT OUTPUT,
    @NewCode         NVARCHAR(20) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Prefix NVARCHAR(2) = N'TB';
    DECLARE @NextNumber INT;

    BEGIN TRY
        SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
        BEGIN TRAN;

        SELECT @NextNumber = MAX(TRY_CONVERT(INT, SUBSTRING(EquipmentCode, LEN(@Prefix) + 1, 10)))
        FROM dbo.Equipment WITH (UPDLOCK, HOLDLOCK)
        WHERE EquipmentCode LIKE @Prefix + '%';

        IF @NextNumber IS NULL SET @NextNumber = 0;
        SET @NextNumber += 1;

        SET @NewCode = @Prefix + RIGHT('0000' + CONVERT(VARCHAR(10), @NextNumber), 4);

        INSERT INTO dbo.Equipment
        (
            EquipmentCode,
            EquipmentName,
            Unit,
            DefaultPrice,
            StockQuantity,
            MinStock,
            LastUpdated,
            IsActive
        )
        VALUES
        (
            @NewCode,
            @EquipmentName,
            @Unit,
            @DefaultPrice,
            @StockQuantity,
            @MinStock,
            SYSDATETIME(),
            1
        );

        SET @NewId = SCOPE_IDENTITY();

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END
