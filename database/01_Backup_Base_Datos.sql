-- Respaldo completo verificado de db_Ferreteria.
-- El motor SQL Server del contenedor tiene acceso a esta ruta.
BACKUP DATABASE [db_Ferreteria]
TO DISK = N'/var/opt/mssql/data/db_Ferreteria_full_20261013_01.bak'
WITH CHECKSUM, COMPRESSION, STATS = 10;
GO

-- Verifica la integridad del conjunto de respaldo.
RESTORE VERIFYONLY
FROM DISK = N'/var/opt/mssql/data/db_Ferreteria_full_20261013_01.bak'
WITH CHECKSUM;
GO
