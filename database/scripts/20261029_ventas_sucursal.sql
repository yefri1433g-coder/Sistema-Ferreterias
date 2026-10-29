/* Actualiza bases antiguas al modelo de ventas por sucursal.
   Conserva NULL en ventas antiguas: su sucursal original no se puede inferir. */
IF COL_LENGTH('dbo.Tbl_Ventas', 'id_sucursal') IS NULL
BEGIN
    ALTER TABLE dbo.Tbl_Ventas ADD id_sucursal INT NULL;
END;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_key_columns
    WHERE parent_object_id = OBJECT_ID('dbo.Tbl_Ventas')
      AND parent_column_id = COLUMNPROPERTY(OBJECT_ID('dbo.Tbl_Ventas'), 'id_sucursal', 'ColumnId')
      AND referenced_object_id = OBJECT_ID('dbo.Tbl_Sucursales')
)
BEGIN
    ALTER TABLE dbo.Tbl_Ventas ADD CONSTRAINT FK_Tbl_Ventas_Tbl_Sucursales
        FOREIGN KEY (id_sucursal) REFERENCES dbo.Tbl_Sucursales(id_sucursal);
END;
GO
