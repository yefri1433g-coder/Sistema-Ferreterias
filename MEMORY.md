# Memoria del proyecto

Actualizado: 28 de octubre de 2026.

## Comprobado

- El 28 de octubre de 2026 se implementaron Login y roles configurables: `Tbl_Roles`, `Tbl_RolesAccesos`, CRUD de roles y permisos por módulo. La migración `database/scripts/20261028_login_roles.sql` se aplicó a `db_Ferreteria`; quedaron Supervisor, Cajero y Bodeguero. Las cuentas Administrador se reasignaron a Supervisor y Vendedor a Cajero para conservar las cuentas y empleados relacionados. La consulta posterior confirmó los tres roles y sus accesos. Se preparó `database/scripts/20261028_login_roles_rollback.sql`, pero no se ejecutó. El estudiante confirmó el funcionamiento del Login, la navegación y los accesos por rol, y el CRUD de Roles.
- El 28 de octubre de 2026 se pasó el usuario conectado desde el menú a los CRUD de Usuarios y Roles; ambos muestran usuario y rol en su encabezado. El estudiante confirmó que los encabezados funcionan.
- El commit `ff1d79f` agregó comentarios de lectura a 71 archivos C# y se integró por fast-forward a `main` y GitHub. MSBuild 18.8.2 compiló los cuatro proyectos desde `main` con salida aislada en `bin/CodexMainValidation`; la aplicación se inició y el proceso respondió. GitHub confirmó que `main` e `integrante2` apuntaban al mismo commit.
- El 28 de octubre de 2026, a petición del estudiante, se limpiaron de `db_Ferreteria` los datos transaccionales agregados desde la aplicación. Ventas, detalles de ventas, pagos, Kardex, traslados y detalles de traslado quedaron con cinco filas de semilla cada uno; `Tbl_Stock` volvió a las cantidades del script. La verificación confirmó cinco filas en las tablas sembradas, seis usuarios (cinco cuentas de ejemplo más `admin`), tres roles y 28 accesos configurables. Las identidades transaccionales se reiniciaron para que la próxima inserción use el ID 6.
- Curso: Programación II 2026. Sistema de ferreterías multisucursal.
- Existe la solución `src/SistemaFerreterias/SistemaFerreterias.slnx`, con los proyectos Entidad, Datos, Negocio y Presentación.
- El 23 de octubre de 2026, el clon `C:\2026_Progra2_Integracion` estaba en `main`, sincronizado con `origin/main`. El historial local muestra integrados los Pull Requests #1 (Integrante 2), #2 (Integrante 1) y #4 (Integrante 3). Hay ramas remotas publicadas `origin/integrante1`, `origin/integrante2` y `origin/Integrante-3`.
- La solución compiló correctamente el 16 de octubre de 2026 mediante MSBuild/dotnet con salida aislada en `bin/CodexBuild`, sin advertencias ni errores. La salida normal a `bin/Debug` tuvo DLL ocupadas por otro proceso.
- La arquitectura respeta `Presentación -> Negocio -> Datos -> SQL Server`; Entidad transporta los datos entre capas.
- `database/SCRIPT NUEVO2.sql` define `db_Ferreteria` y las tablas con prefijo `Tbl_`. El bloque de semillas solo se ejecuta cuando todas las tablas están vacías, para evitar duplicados.
- `Datos.ConexionDatos` usa `System.Data.SqlClient`, toma `ConexionSQLServer` desde `App.config` y prioriza `FERRETERIA_CONNECTION_STRING` si está definida. La conexión SQL Server se verificó con autenticación SQL en `localhost,1434`, base `db_Ferreteria`.
- El 16 de octubre se inspeccionó el catálogo de `db_Ferreteria` en modo de solo lectura: 17 tablas, sus columnas, tipos, nulabilidad, PK y 19 FK. El detalle está en `database/CONEXION_DATABASE.md`; no se consultaron ni modificaron filas de negocio durante esa inspección.
- Se ejecutó el respaldo `db_Ferreteria_20261016.bak` en `/var/opt/mssql/data` del contenedor `sql_prog2b`; `RESTORE VERIFYONLY` fue correcto y la copia en `Docker/Backups/` coincide por SHA-256. No se modificaron tablas ni datos.
- A petición del estudiante, se depuró la base a cinco filas de ejemplo por cada una de las 16 tablas sembradas. `Tbl_Usuarios` conserva cinco cuentas de ejemplo más `admin` (seis filas).
- La lista de Ventas muestra todos los productos activos y su stock agregado por sucursal, incluso cuando es cero; la aplicación impide vender cantidades superiores a la existencia. La consulta devolvió cinco productos activos para cada una de las cinco sucursales y el estudiante confirmó la pantalla.
- El 22 de octubre de 2026 se compiló `src/SistemaFerreterias/SistemaFerreterias.slnx` para cerrar documentalmente el CRUD simple: 0 errores y una advertencia `NETSDK1194` por usar una ruta de salida a nivel de solución. No se ejecutaron pruebas manuales nuevas ni se modificó SQL Server.
- El 23 de octubre de 2026, el Integrante 1, Yefry Yahir García García, realizó y confirmó contra SQL Server los siete indicadores del Dashboard: Ventas de hoy, Productos activos, Traslados pendientes, Clientes registrados, Ventas de los últimos 7 días, Estado de existencias y Actividad reciente. El diseño se conservó sin modificar `DashboardForms.Designer.cs`.
- El 23 de octubre de 2026, el Integrante 1, Yefry Yahir García García, confirmó como finalizado el Documento 08 — Dashboard.
- El 23 de octubre de 2026, el Integrante 1, Yefry Yahir García García, probó y confirmó en Traslados: Consultar con detalle, Agregar encabezado y detalle, Buscar, Editar y Eliminar. La paginación muestra diez registros por página y los totales no cambian al navegar entre páginas.

## CRUD validados por el estudiante

- Usuarios (`dbo.Tbl_Usuarios`, `UsuariosForms`): Agregar, Buscar, Editar y Eliminar funcionan. Las operaciones usan parámetros SQL, validan PK y campos, y la paginación muestra diez registros por página sin alterar el total general.
- Clientes (`dbo.Tbl_Clientes`, `ClientesForms`): Agregar, Consultar, Buscar y Editar fueron confirmados. Eliminar y Limpiar quedan pendientes de prueba manual. Limpiar conserva IDs 1 a 5 y elimina clientes con `id_cliente > 5` después de confirmación.
- Sucursales (`dbo.Tbl_Sucursales`, pestaña en `CatalogosGestionForms`): Agregar, Buscar, Editar, Eliminar y paginación fueron confirmados. Limpiar elimina `id_sucursal > 5`, conserva los cinco ejemplos y queda pendiente de prueba manual.
- Usuarios tiene un botón Limpiar equivalente: conserva IDs 1 a 6 (cinco ejemplos y `admin`) y queda pendiente de prueba manual.
- Inventarios (`dbo.Tbl_Inventarios`, `InventariosForms`): el Integrante 1, Yefry Yahir García García, realizó y confirmó Consultar, ComboBox de sucursales, paginación de diez registros, totales dinámicos, Agregar, Buscar, Editar y Eliminar.
- Proveedores (`dbo.Tbl_Proveedores`, `ProveedoresForms`): el estudiante confirmó que el módulo funciona, incluida la búsqueda por identificador, nombre, contacto, teléfono y correo; las tarjetas resumen se actualizan con el resultado filtrado sin depender de la página actual.
- Empleados (`dbo.Tbl_Empleados`, `EmpleadosForms`): el Integrante 1, Yefry Yahir García García, realizó y confirmó Consultar, ComboBox de usuarios y sucursales, paginación de diez registros, totales dinámicos, Agregar, Buscar, Editar y Eliminar.

## Decisiones y pendientes

- Se mantienen SQL Server, SSMS y la arquitectura Entidad, Datos, Negocio y Presentación.
- Se respetan las tablas `Tbl_` y las claves `id_` del esquema actual.
- El Integrante 1, Yefry Yahir García García, confirmó como completados y aprobados los Documentos 01 y 02; confirmó parcialmente el Documento 03 para Inventarios y Empleados; y confirmó como completados Traslados del Documento 04 y el Dashboard del Documento 08. No se registran como probados otros módulos.
- Documento 05 — Git colaborativo: el historial local confirma el clon, `main` sincronizada con `origin/main`, ramas remotas publicadas para los tres integrantes y Pull Requests #1, #2 y #4 integrados. La visibilidad privada del repositorio, la conservación de las carpetas originales y las pruebas finales de CRUD de los tres integrantes requieren confirmación del grupo. No declarar el documento finalizado hasta verificar esos puntos.
- No se almacenan credenciales reales en archivos del proyecto.
