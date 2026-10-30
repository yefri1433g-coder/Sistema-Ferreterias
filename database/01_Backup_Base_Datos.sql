-- Elegir una ruta nueva y accesible para el servicio SQL Server.
-- La ruta pertenece al servidor; en Docker no es una ruta de Windows.
USE [db_Ferreteria];
SET NOCOUNT ON;
DECLARE @Ruta nvarchar(4000) = N'CAMBIAR_RUTA_DEL_RESPALDO.bak';
IF @Ruta LIKE N'CAMBIAR%'
    THROW 50001, 'Configura primero una ruta nueva para el respaldo.', 1;

BACKUP DATABASE [db_Ferreteria] TO DISK = @Ruta WITH COPY_ONLY, CHECKSUM;
RESTORE VERIFYONLY FROM DISK = @Ruta WITH CHECKSUM;

-- Después copiar el archivo fuera de Docker y comparar su SHA-256.
-- No versionar el .bak. Este script no restaura ni elimina la base.
