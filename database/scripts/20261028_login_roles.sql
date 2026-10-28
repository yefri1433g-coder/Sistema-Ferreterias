/*
  Documento 06 - Login y roles
  Idempotente para la base db_Ferreteria.
  Conserva las cuentas antiguas: Administrador pasa a Supervisor y
  Vendedor pasa a Cajero porque existen empleados relacionados.
*/
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID('dbo.Tbl_Roles', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tbl_Roles
    (
        CodigoRol INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Tbl_Roles PRIMARY KEY,
        NombreRol VARCHAR(30) NOT NULL,
        Estado BIT NOT NULL CONSTRAINT DF_Tbl_Roles_Estado DEFAULT (1),
        CONSTRAINT UQ_Tbl_Roles_NombreRol UNIQUE (NombreRol)
    );
END;

IF OBJECT_ID('dbo.Tbl_RolesAccesos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tbl_RolesAccesos
    (
        CodigoRolAcceso INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Tbl_RolesAccesos PRIMARY KEY,
        CodigoRol INT NOT NULL,
        NombreModulo VARCHAR(40) NOT NULL,
        CONSTRAINT FK_Tbl_RolesAccesos_Roles FOREIGN KEY (CodigoRol)
            REFERENCES dbo.Tbl_Roles(CodigoRol),
        CONSTRAINT UQ_Tbl_RolesAccesos_RolModulo UNIQUE (CodigoRol, NombreModulo)
    );
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Tbl_Roles WHERE NombreRol = 'Supervisor')
    INSERT dbo.Tbl_Roles (NombreRol, Estado) VALUES ('Supervisor', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.Tbl_Roles WHERE NombreRol = 'Cajero')
    INSERT dbo.Tbl_Roles (NombreRol, Estado) VALUES ('Cajero', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.Tbl_Roles WHERE NombreRol = 'Bodeguero')
    INSERT dbo.Tbl_Roles (NombreRol, Estado) VALUES ('Bodeguero', 1);

UPDATE dbo.Tbl_Usuarios SET rol = 'Supervisor' WHERE rol = 'Administrador';
UPDATE dbo.Tbl_Usuarios SET rol = 'Cajero' WHERE rol = 'Vendedor';

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Tbl_Usuarios_Roles')
    ALTER TABLE dbo.Tbl_Usuarios WITH CHECK ADD CONSTRAINT FK_Tbl_Usuarios_Roles
        FOREIGN KEY (rol) REFERENCES dbo.Tbl_Roles(NombreRol) ON UPDATE CASCADE;

DECLARE @Supervisor INT = (SELECT CodigoRol FROM dbo.Tbl_Roles WHERE NombreRol = 'Supervisor');
DECLARE @Cajero INT = (SELECT CodigoRol FROM dbo.Tbl_Roles WHERE NombreRol = 'Cajero');
DECLARE @Bodeguero INT = (SELECT CodigoRol FROM dbo.Tbl_Roles WHERE NombreRol = 'Bodeguero');

INSERT dbo.Tbl_RolesAccesos (CodigoRol, NombreModulo)
SELECT r.CodigoRol, m.NombreModulo
FROM (VALUES
    (@Supervisor, 'Dashboard'), (@Supervisor, 'Sucursales'), (@Supervisor, 'Proveedores'),
    (@Supervisor, 'Productos'), (@Supervisor, 'Categorías'), (@Supervisor, 'Compras'),
    (@Supervisor, 'Inventarios'), (@Supervisor, 'Stock'), (@Supervisor, 'Kardex'),
    (@Supervisor, 'Traslados'), (@Supervisor, 'Clientes'), (@Supervisor, 'Empleados'),
    (@Supervisor, 'Ventas'), (@Supervisor, 'Pagos'), (@Supervisor, 'Usuarios'),
    (@Supervisor, 'Roles y accesos'),
    (@Cajero, 'Dashboard'), (@Cajero, 'Clientes'), (@Cajero, 'Ventas'), (@Cajero, 'Pagos'),
    (@Bodeguero, 'Dashboard'), (@Bodeguero, 'Proveedores'), (@Bodeguero, 'Productos'),
    (@Bodeguero, 'Compras'), (@Bodeguero, 'Inventarios'), (@Bodeguero, 'Stock'),
    (@Bodeguero, 'Kardex'), (@Bodeguero, 'Traslados')
) AS m(CodigoRol, NombreModulo)
INNER JOIN dbo.Tbl_Roles r ON r.CodigoRol = m.CodigoRol
WHERE NOT EXISTS
(
    SELECT 1 FROM dbo.Tbl_RolesAccesos a
    WHERE a.CodigoRol = m.CodigoRol AND a.NombreModulo = m.NombreModulo
);

COMMIT TRANSACTION;
