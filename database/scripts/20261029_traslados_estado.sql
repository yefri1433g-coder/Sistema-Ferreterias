-- Conserva los valores existentes: 0 = Completado, 1 = Pendiente.
-- Amplía el mismo campo para permitir 2 = En tránsito, sin agregar tablas.
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.Tbl_Traslados') AND name = N'estado'
)
    THROW 50001, 'No existe dbo.Tbl_Traslados.estado en esta base.', 1;

IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.Tbl_Traslados') AND name = N'estado'
      AND TYPE_NAME(system_type_id) NOT IN (N'bit', N'tinyint')
)
    THROW 50002, 'El tipo de estado no coincide con el modelo esperado.', 1;

IF EXISTS (SELECT 1 FROM dbo.Tbl_Traslados WHERE estado NOT IN (0, 1, 2))
    THROW 50003, 'Existen estados desconocidos; no se modificó la columna.', 1;

IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.Tbl_Traslados') AND name = N'estado'
      AND TYPE_NAME(system_type_id) = N'bit'
)
    ALTER TABLE dbo.Tbl_Traslados ALTER COLUMN estado TINYINT NOT NULL;

COMMIT TRANSACTION;
