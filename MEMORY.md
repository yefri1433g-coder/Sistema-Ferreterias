# Memoria del proyecto

Actualizado: 22 de octubre de 2026.

## Comprobado

- Curso: Programación II 2026. Sistema de ferreterías multisucursal.
- Existe la solución `src/SistemaFerreterias/SistemaFerreterias.slnx`, con los proyectos Entidad, Datos, Negocio y Presentación.
- La solución compiló correctamente el 16 de octubre de 2026 mediante MSBuild/dotnet con salida aislada en `bin/CodexBuild`, sin advertencias ni errores. La salida normal a `bin/Debug` tuvo DLL ocupadas por otro proceso.
- La arquitectura respeta `Presentación -> Negocio -> Datos -> SQL Server`; Entidad transporta los datos entre capas.
- `database/SCRIPT NUEVO2.sql` define `db_Ferreteria` y las tablas con prefijo `Tbl_`. El bloque de semillas solo se ejecuta cuando todas las tablas están vacías, para evitar duplicados.
- `Datos.ConexionDatos` usa `System.Data.SqlClient`, toma `ConexionSQLServer` desde `App.config` y prioriza `FERRETERIA_CONNECTION_STRING` si está definida. La conexión SQL Server se verificó con autenticación SQL en `localhost,1434`, base `db_Ferreteria`.
- El 16 de octubre se inspeccionó el catálogo de `db_Ferreteria` en modo de solo lectura: 17 tablas, sus columnas, tipos, nulabilidad, PK y 19 FK. El detalle está en `database/CONEXION_DATABASE.md`; no se consultaron ni modificaron filas de negocio durante esa inspección.
- Se ejecutó el respaldo `db_Ferreteria_20261016.bak` en `/var/opt/mssql/data` del contenedor `sql_prog2b`; `RESTORE VERIFYONLY` fue correcto y la copia en `Docker/Backups/` coincide por SHA-256. No se modificaron tablas ni datos.
- A petición del estudiante, se depuró la base a cinco filas de ejemplo por cada una de las 16 tablas sembradas. `Tbl_Usuarios` conserva cinco cuentas de ejemplo más `admin` (seis filas).
- La lista de Ventas muestra todos los productos activos y su stock agregado por sucursal, incluso cuando es cero; la aplicación impide vender cantidades superiores a la existencia. La consulta devolvió cinco productos activos para cada una de las cinco sucursales y el estudiante confirmó la pantalla.
- No hay remoto de GitHub configurado para este proyecto.
- El 22 de octubre de 2026 se compiló `src/SistemaFerreterias/SistemaFerreterias.slnx` para cerrar documentalmente el CRUD simple: 0 errores y una advertencia `NETSDK1194` por usar una ruta de salida a nivel de solución. No se ejecutaron pruebas manuales nuevas ni se modificó SQL Server.

## CRUD validados por el estudiante

- Usuarios (`dbo.Tbl_Usuarios`, `UsuariosForms`): Agregar, Buscar, Editar y Eliminar funcionan. Las operaciones usan parámetros SQL, validan PK y campos, y la paginación muestra diez registros por página sin alterar el total general.
- Clientes (`dbo.Tbl_Clientes`, `ClientesForms`): Agregar, Consultar, Buscar y Editar fueron confirmados. Eliminar y Limpiar quedan pendientes de prueba manual. Limpiar conserva IDs 1 a 5 y elimina clientes con `id_cliente > 5` después de confirmación.
- Sucursales (`dbo.Tbl_Sucursales`, pestaña en `CatalogosGestionForms`): Agregar, Buscar, Editar, Eliminar y paginación fueron confirmados. Limpiar elimina `id_sucursal > 5`, conserva los cinco ejemplos y queda pendiente de prueba manual.
- Usuarios tiene un botón Limpiar equivalente: conserva IDs 1 a 6 (cinco ejemplos y `admin`) y queda pendiente de prueba manual.
- Inventarios (`dbo.Tbl_Inventarios`, `InventariosForms`): el estudiante confirmó que Consultar, carga de sucursales, Agregar, Buscar, Editar, Eliminar, paginación del listado CRUD y totales funcionan correctamente. La paginación del CRUD se mantiene en Presentación con diez registros por página y no altera el total.
- Proveedores (`dbo.Tbl_Proveedores`, `ProveedoresForms`): el estudiante confirmó que el módulo funciona, incluida la búsqueda por identificador, nombre, contacto, teléfono y correo; las tarjetas resumen se actualizan con el resultado filtrado sin depender de la página actual.
- Empleados (`dbo.Tbl_Empleados`, `EmpleadosForms`): el estudiante confirmó Consultar, ComboBox de usuarios y sucursales, Agregar, Buscar, Editar, Eliminar, paginación principal de diez registros y tarjetas dinámicas basadas en el resultado filtrado.

## Decisiones y pendientes

- Se mantienen SQL Server, SSMS y la arquitectura Entidad, Datos, Negocio y Presentación.
- Se respetan las tablas `Tbl_` y las claves `id_` del esquema actual.
- Los Documentos 01 y 02 están cerrados. El cierre del Documento 02 fue solicitado por el estudiante el 22 de octubre de 2026; sus pruebas manuales no confirmadas se mantienen como observaciones en `ESTADO_DOCUMENTOS.md`. No se debe iniciar otro módulo sin una nueva confirmación del estudiante.
- No se almacenan credenciales reales en archivos del proyecto.
