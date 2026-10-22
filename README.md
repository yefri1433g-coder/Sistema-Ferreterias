# Proyecto Final de Programación II

Sistema de ferreterías multisucursal para el curso Programación II 2026.

## Tecnologías y arquitectura

- C# con .NET Framework y Windows Forms.
- SQL Server, `System.Data.SqlClient` y SSMS para administración de la base.
- Docker para el entorno SQL Server configurado para el curso.

La aplicación se organiza en capas:

`Presentación -> Negocio -> Datos -> SQL Server`

Entidad contiene las clases que transportan información entre las capas.

## Estructura actual

- `skills/`: once guías planas de trabajo.
- `database/`: documentación de conexión, respaldo y el script `SCRIPT NUEVO2.sql`.
- `docs/COMANDOS_RAPIDOS.md`: comandos de compilación para CMD.
- `src/SistemaFerreterias/`: solución `SistemaFerreterias.slnx` con Entidad, Datos, Negocio y Presentación.
- `Docker/Backups/`: respaldo verificado `db_Ferreteria_20261016.bak`.

## Conexión y estado comprobado

La configuración de desarrollo está en `src/SistemaFerreterias/Presentacion/App.config`. `ConexionDatos` puede tomar `FERRETERIA_CONNECTION_STRING`, que prevalece sobre `ConexionSQLServer` del archivo de configuración. No se guardan credenciales reales en el repositorio.

La conexión a `localhost,1434` y la base `db_Ferreteria` fue verificada. El catálogo cuenta con 17 tablas y 19 claves foráneas; consulta [database/CONEXION_DATABASE.md](database/CONEXION_DATABASE.md) para el detalle. La solución compiló correctamente el 16 de octubre de 2026, sin advertencias ni errores, usando salida aislada.

## Estado de trabajo

Los Documentos 01 y 02 están cerrados. El Documento 02 conserva observaciones de pruebas manuales no confirmadas en [ESTADO_DOCUMENTOS.md](ESTADO_DOCUMENTOS.md). No iniciar otros módulos sin autorización expresa.

## Reglas de trabajo

- Mantener las capas, consultas SQL parametrizadas y manejo de errores.
- Conservar los diseños existentes y no publicar credenciales.
- Registrar en `MEMORY.md` únicamente hechos comprobados.
- En Windows, preferir `cmd.exe`; revisar [AGENTS.md](AGENTS.md) y [docs/COMANDOS_RAPIDOS.md](docs/COMANDOS_RAPIDOS.md).
