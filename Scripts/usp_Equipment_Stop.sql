CREATE OR ALTER PROCEDURE dbo.usp_Equipment_Stop
    @EquipmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Equipment
    SET IsActive = 0,
        LastUpdated = SYSDATETIME()
    WHERE EquipmentID = @EquipmentID
      AND IsActive = 1;

    IF @@ROWCOUNT = 0
        THROW 50011, 'Equipment not found or already inactive.', 1;
END
GO
