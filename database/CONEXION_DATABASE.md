# Conexión para la entrega

Verificación: 29 de octubre de 2026. Este documento registra la configuración
actual; no reproduce el antiguo inventario de tablas citado en MEMORY.md.

## Aplicación

`Datos.ConexionDatos` usa System.Data.SqlClient y consulta, en este orden:

1. Variable de usuario `FERRETERIA_CONNECTION_STRING`.
2. Variable del proceso con el mismo nombre.
3. `ConexionSQLServer` en `Presentacion.exe.config` (App.config en el proyecto).

La conexión efectiva en este equipo se comprobó con `localhost,1434`,
base `db_Ferreteria`, autenticación SQL y una variable del proceso.
El contenedor correspondiente es `sql_prog2b_backup_20261016`.
No se publican el usuario ni la contraseña efectivos.

La configuración incluida con la entrega usa autenticación integrada y no
contiene contraseña. Cada equipo debe establecer su propia conexión antes
de ejecutar la aplicación. Una variable de usuario existente tiene prioridad
sobre la configuración del archivo y la variable del proceso.

## Otro equipo Windows

- Si SQL Server se ejecuta en el mismo equipo: usar su servidor y puerto locales.
- Si se ejecuta en otra computadora: sustituir localhost por la IP o nombre de
  esa computadora y el puerto publicado, por ejemplo `IP_DEL_SERVIDOR,1434`.
- Comprobar conectividad, credenciales y permisos desde el Windows de destino.
- No reutilizar automáticamente el puerto de otro contenedor.

Ejemplo sin credenciales reales:

```text
Server=IP_DEL_SERVIDOR,1434;Database=db_Ferreteria;User Id=USUARIO_LOCAL;Password=CLAVE_LOCAL;Encrypt=False;TrustServerCertificate=True
```

La prueba desde otro equipo sigue pendiente. Las credenciales se distribuyen
por separado y no se incluyen en GitHub ni en las evidencias.

## Respaldo de entrega

- Archivo: `db_Ferreteria_entrega_20261029_769b7557.bak`.
- Copia local: `entrega/ProyectoFinal_20261029/respaldo/`.
- Original: `/var/opt/mssql/data/` en el contenedor indicado.
- BACKUP COPY_ONLY WITH CHECKSUM y RESTORE VERIFYONLY WITH CHECKSUM: correctos.
- SHA-256, coincidente dentro y fuera del contenedor:
  `646f92a890b3cb465b3b75c3a1f946e04641c0ba0d07ea550774a4444bf7a4f8`.

El respaldo está excluido de Git. Su restauración en el equipo de destino no
se ha probado todavía y no debe sobrescribir una base existente sin revisión.
