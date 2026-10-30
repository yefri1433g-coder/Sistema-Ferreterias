# Estado de documentos

Última revisión: 29 de octubre de 2026.

Los documentos 01 al 11 se marcan como COMPLETADOS por indicación expresa del estudiante. Esta actualización registra el cierre documental y no añade pruebas técnicas nuevas; los resultados anteriores se conservan como antecedentes.

| Documento | Estado | Nota |
|---|---|---|
| 01 Inspección inicial | COMPLETADO | El Integrante 1, Yefry Yahir García García, confirmó este documento como completado y aprobado. |
| 02 CRUD simple | COMPLETADO | El Integrante 1, Yefry Yahir García García, confirmó este documento como completado. |
| 03 CRUD relacional | COMPLETADO | Cierre documental registrado por indicación del estudiante el 29 de octubre de 2026. Las pruebas registradas de Inventarios y Empleados se conservan como antecedentes. |
| 04 Maestro detalle | COMPLETADO | El Integrante 1, Yefry Yahir García García, confirmó Traslados: Consultar con detalle, Agregar, Buscar, Editar y Eliminar. |
| 05 Git colaborativo | COMPLETADO | Cierre documental registrado por indicación del estudiante el 29 de octubre de 2026. Se conserva el historial de integración de las ramas y Pull Requests del grupo. |
| 06 Login y roles | COMPLETADO | Migración aplicada; Supervisor, Cajero y Bodeguero tienen accesos configurables. El estudiante confirmó el Login, la navegación y accesos por rol, el CRUD de Roles y los encabezados con usuario y rol. Integrado a `main` y GitHub; la solución compiló y la aplicación se inició desde `main` el 28 de octubre de 2026. |
| 07 Panel de navegación | COMPLETADO | Cierre documental registrado por indicación del estudiante el 29 de octubre de 2026. Panel implementado y 16 enlaces probados; antecedentes técnicos en MEMORY.md. |
| 08 Dashboard | COMPLETADO | El Integrante 1, Yefry Yahir García García, confirmó contra SQL Server: Ventas de hoy, Productos activos, Traslados pendientes, Clientes registrados, Ventas de los últimos 7 días, Estado de existencias y Actividad reciente. El diseño se conservó sin modificar `DashboardForms.Designer.cs`. |
| 09 Integración general | COMPLETADO | Los tres integrantes confirmaron Login y Panel, accesos por rol, CRUD contra SQL Server, Dashboard comparado con SQL Server, compilación sin errores y ramas de GitHub integradas sin conflictos. Confirmación incorporada en d50df1a. |
| 10 Publicación y entrega final | COMPLETADO | Cierre documental registrado por indicación del estudiante el 29 de octubre de 2026. Release, carpeta de entrega y respaldo preparados; resultados técnicos y alcance de las pruebas en docs/ENTREGA_FINAL.md. |
| 11 Publicación complementaria | COMPLETADO | Cierre documental registrado por indicación del estudiante el 29 de octubre de 2026. |

## Base documental comprobada

- La revisión actual encontró ausentes la guía de publicación y los documentos de conexión/respaldo citados anteriormente. Se reconstruyeron a partir de las instrucciones del estudiante y las verificaciones actuales; no se afirma que existan todos los documentos históricos.
- Existe `src/SistemaFerreterias/` con la solución y los proyectos Entidad, Datos, Negocio y Presentación.
- El catálogo de `db_Ferreteria` se verificó en solo lectura el 16 de octubre: 17 tablas y 19 claves foráneas. El detalle está en `database/CONEXION_DATABASE.md`.
- La referencia al respaldo del 16 de octubre es histórica. Para esta entrega existe un respaldo nuevo del 29 de octubre en `entrega/ProyectoFinal_20261029/respaldo/`, verificado dentro y fuera de Docker; ver `database/CONEXION_DATABASE.md`.

## Documento 02: estado de módulos

### Cierre documental — 22 de octubre de 2026

- La solución `src/SistemaFerreterias/SistemaFerreterias.slnx` compiló correctamente con `dotnet build`: 0 errores. Se emitió una advertencia `NETSDK1194` porque la salida aislada se indicó a nivel de solución; no impidió la compilación.
- Se revisó la implementación de los CRUD simples en las capas Entidad, Datos, Negocio y Presentación. Los accesos de Datos usan consultas parametrizadas contra las tablas físicas `Tbl_` y claves `id_` documentadas en `database/CONEXION_DATABASE.md`.
- Este cierre no declara como ejecutadas las pruebas manuales que no constan como confirmadas. Permanecen solo como observaciones de calidad para cuando el estudiante decida verificarlas; no se iniciaron módulos nuevos ni se modificó SQL Server.

- Usuarios: Agregar, Buscar, Editar y Eliminar confirmados manualmente por el estudiante. La paginación es de diez registros por página y se implementa solo en Presentación.
- Clientes: Agregar, Consultar, Buscar y Editar confirmados. Eliminar y Limpiar siguen pendientes de prueba manual; Limpiar conserva los IDs 1 a 5.
- Sucursales: Agregar, Buscar, Editar, Eliminar y paginación confirmados. Limpiar conserva IDs 1 a 5 y queda pendiente de prueba manual.
- Proveedores : el módulo incluye Consultar, Buscar, Agregar, Editar, Eliminar y paginación de diez registros por página. La implementación se comprobó en las capas Entidad, Datos, Negocio y Presentación; faltan registrar las pruebas manuales del estudiante, si no se han confirmado aún.
- Ventas: la consulta de productos por sucursal incluye productos activos con stock cero y la validación evita vender sobre la existencia; el estudiante confirmó su funcionamiento.

No se han iniciado pruebas nuevas ni código adicional con esta actualización documental. El Documento 09 quedó completado con la confirmación de los tres integrantes. El Documento 10 está en proceso para las etapas autorizadas 10.3 y 10.4; la publicación continúa pendiente de autorización final del grupo.

## Documento 03: módulos autorizados

- Inventarios (`dbo.Tbl_Inventarios`, `InventariosForms`): el Integrante 1, Yefry Yahir García García, realizó y confirmó Consultar, ComboBox de sucursales, paginación de diez registros, totales dinámicos, Agregar, Buscar, Editar y Eliminar.
- Proveedores (`dbo.Tbl_Proveedores`, `ProveedoresForms`): el estudiante confirmó Consultar, Agregar, Buscar, Editar, Eliminar, paginación de diez registros y totales dinámicos. La búsqueda cubre identificador, nombre, contacto, teléfono y correo.
- Empleados (`dbo.Tbl_Empleados`, `EmpleadosForms`): el Integrante 1, Yefry Yahir García García, realizó y confirmó Consultar, ComboBox de usuarios y sucursales, paginación de diez registros, totales dinámicos, Agregar, Buscar, Editar y Eliminar.

## Documento 04: pruebas confirmadas

- El Integrante 1, Yefry Yahir García García, confirmó en Traslados: Consultar con detalle, Agregar encabezado y detalle, Buscar, Editar y Eliminar.
- La paginación se realiza en Presentación con diez registros por página y no altera los totales.

## Documento 08: cierre confirmado

- El Integrante 1, Yefry Yahir García García, confirmó el cierre del Dashboard el 23 de octubre de 2026.
- El Integrante 1 comparó contra SQL Server y confirmó los indicadores de Ventas de hoy, Productos activos, Traslados pendientes, Clientes registrados, Ventas de los últimos 7 días, Estado de existencias y Actividad reciente. El diseño se conservó sin modificar `DashboardForms.Designer.cs`.
