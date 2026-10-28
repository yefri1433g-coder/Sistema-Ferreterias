/*
  Reversa la migración 20261028_login_roles.sql solo si conserva exactamente
  los tres roles y los 28 accesos iniciales. No usar después de personalizar roles.
*/
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID('dbo.Tbl_Roles', 'U') IS NULL OR OBJECT_ID('dbo.Tbl_RolesAccesos', 'U') IS NULL
    THROW 50001, 'No se encontraron las tablas creadas por la migración de Login y Roles.', 1;

DECLARE @AccesosIniciales TABLE
(
    NombreRol VARCHAR(30) NOT NULL,
    NombreModulo VARCHAR(40) NOT NULL,
    PRIMARY KEY (NombreRol, NombreModulo)
);

INSERT @AccesosIniciales (NombreRol, NombreModulo) VALUES
('Supervisor','Dashboard'),('Supervisor','Sucursales'),('Supervisor','Proveedores'),
('Supervisor','Productos'),('Supervisor','Categorías'),('Supervisor','Compras'),
('Supervisor','Inventarios'),('Supervisor','Stock'),('Supervisor','Kardex'),
('Supervisor','Traslados'),('Supervisor','Clientes'),('Supervisor','Empleados'),
('Supervisor','Ventas'),('Supervisor','Pagos'),('Supervisor','Usuarios'),('Supervisor','Roles y accesos'),
('Cajero','Dashboard'),('Cajero','Clientes'),('Cajero','Ventas'),('Cajero','Pagos'),
('Bodeguero','Dashboard'),('Bodeguero','Proveedores'),('Bodeguero','Productos'),
('Bodeguero','Compras'),('Bodeguero','Inventarios'),('Bodeguero','Stock'),
('Bodeguero','Kardex'),('Bodeguero','Traslados');

IF (SELECT COUNT(1) FROM dbo.Tbl_Roles) <> 3
   OR EXISTS (SELECT 1 FROM dbo.Tbl_Roles WHERE Estado <> 1)
   OR EXISTS (SELECT NombreRol, NombreModulo FROM @AccesosIniciales
              EXCEPT SELECT r.NombreRol, a.NombreModulo
                     FROM dbo.Tbl_Roles r INNER JOIN dbo.Tbl_RolesAccesos a ON a.CodigoRol = r.CodigoRol)
   OR EXISTS (SELECT r.NombreRol, a.NombreModulo
              FROM dbo.Tbl_Roles r INNER JOIN dbo.Tbl_RolesAccesos a ON a.CodigoRol = r.CodigoRol
              EXCEPT SELECT NombreRol, NombreModulo FROM @AccesosIniciales)
    THROW 50002, 'La configuración de roles cambió; se cancela la reversión para conservar esos cambios.', 1;

IF NOT EXISTS (SELECT 1 FROM dbo.Tbl_Usuarios WHERE id_usuario = 1 AND nombre_usuario = 'carlos' AND rol = 'Supervisor')
   OR NOT EXISTS (SELECT 1 FROM dbo.Tbl_Usuarios WHERE id_usuario = 4 AND nombre_usuario = 'sofia' AND rol = 'Cajero')
   OR NOT EXISTS (SELECT 1 FROM dbo.Tbl_Usuarios WHERE id_usuario = 6 AND nombre_usuario = 'admin' AND rol = 'Supervisor')
    THROW 50003, 'Las cuentas originales cambiaron; se cancela la reversión automática.', 1;

UPDATE dbo.Tbl_Usuarios SET rol = 'Administrador' WHERE id_usuario IN (1, 6);
UPDATE dbo.Tbl_Usuarios SET rol = 'Vendedor' WHERE id_usuario = 4;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Tbl_Usuarios_Roles')
    ALTER TABLE dbo.Tbl_Usuarios DROP CONSTRAINT FK_Tbl_Usuarios_Roles;
DROP TABLE dbo.Tbl_RolesAccesos;
DROP TABLE dbo.Tbl_Roles;

COMMIT TRANSACTION;
