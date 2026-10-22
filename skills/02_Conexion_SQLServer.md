# 02 — Conexión a SQL Server

**Estado: COMPLETADO — conexión y consulta inicial verificadas en SSMS.**

Esta etapa está reservada para documentar y configurar la conexión de la aplicación C# .NET Framework a SQL Server, siguiendo las reglas de `AGENTS.md`.

## Configuración comprobada

- Servidor: `localhost,1434`.
- Base de datos activa: `db_Ferreteria`.
- Autenticación: SQL Server; la sesión de SSMS aparece conectada como `sa`.
- La captura de SSMS indica que la conexión usa cifrado opcional y confianza del certificado del servidor.
- No se guarda la contraseña en archivos del proyecto.

## Resultado de inspección inicial

La consulta de `DB_NAME()` devolvió `db_Ferreteria`. La consulta de `INFORMATION_SCHEMA.TABLES`, limitada a las primeras cinco tablas base y ordenada por nombre, devolvió:

1. `dbo.Tbl_Categorias`
2. `dbo.Tbl_Clientes`
3. `dbo.Tbl_Compras`
4. `dbo.Tbl_DetalleDeCompras`
5. `dbo.Tbl_DetallesVentas`

La captura muestra el mensaje `Consulta ejecutada correctamente`. Este resultado no enumera todas las tablas de la base.

## Uso de `sqlcmd` desde CMD

El `sqlcmd` ODBC anterior (v15.0.1300.359, ODBC Driver 17) falló con `Encryption not supported on the client`, incluso usando `-N -C`. Se instaló `sqlcmd` Go v1.6.0 y se verificó que ejecuta la consulta correctamente en `localhost,1434` y `db_Ferreteria`.

En una nueva ventana de CMD, ejecutar `refreshenv` para refrescar el PATH y comprobar `where sqlcmd`; `C:\Program Files\SqlCmd\sqlcmd.exe` debe aparecer primero. Si la ventana no reconoce el PATH nuevo, usar esa ruta completa. El acceso por contraseña debe dejar que sqlcmd la solicite; no escribirla en los comandos ni guardarla en el proyecto.
