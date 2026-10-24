USE [db_Ferreteria];
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID('dbo.Tbl_Ventas', 'U') IS NULL
        THROW 50001, 'No existe dbo.Tbl_Ventas en db_Ferreteria.', 1;

    IF COL_LENGTH('dbo.Tbl_Ventas', 'id_sucursal') IS NULL
        ALTER TABLE dbo.Tbl_Ventas ADD id_sucursal INT NULL;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.foreign_key_columns fkc
        WHERE fkc.parent_object_id = OBJECT_ID('dbo.Tbl_Ventas')
          AND COL_NAME(fkc.parent_object_id, fkc.parent_column_id) = 'id_sucursal'
          AND fkc.referenced_object_id = OBJECT_ID('dbo.Tbl_Sucursales')
          AND COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) = 'id_sucursal'
    )
    BEGIN
        ALTER TABLE dbo.Tbl_Ventas WITH CHECK
        ADD CONSTRAINT FK_Tbl_Ventas_Tbl_Sucursales
            FOREIGN KEY (id_sucursal) REFERENCES dbo.Tbl_Sucursales(id_sucursal);
    END;

    COMMIT TRANSACTION;
    PRINT 'La relación opcional Venta-Sucursal quedó configurada.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
