/*
   Habilita editar compras con inventario por sucursal.
   Las compras existentes quedan con id_sucursal NULL hasta asignarles
   manualmente la sucursal correcta. No se debe inferir esa asignación.
*/
IF COL_LENGTH('dbo.Tbl_Compras', 'id_sucursal') IS NULL
BEGIN
    ALTER TABLE dbo.Tbl_Compras ADD id_sucursal INT NULL;
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Tbl_Compras_Tbl_Sucursales'
      AND parent_object_id = OBJECT_ID('dbo.Tbl_Compras')
)
BEGIN
    ALTER TABLE dbo.Tbl_Compras
        ADD CONSTRAINT FK_Tbl_Compras_Tbl_Sucursales
        FOREIGN KEY (id_sucursal) REFERENCES dbo.Tbl_Sucursales(id_sucursal);
END;
GO
