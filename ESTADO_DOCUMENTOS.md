# Estado de documentos

Última revisión: 23 de octubre de 2026.

| Documento | Estado | Nota |
|---|---|---|
| 01 Inspección inicial | COMPLETADO Y APROBADO | El Integrante 1, Yefry Yahir García García, confirmó este documento como completado y aprobado. |
| 02 CRUD simple | COMPLETADO | El Integrante 1, Yefry Yahir García García, confirmó este documento como completado. |
| 03 CRUD relacional | COMPLETADO PARCIAL | El Integrante 1, Yefry Yahir García García, confirmó los módulos Inventarios y Empleados. No se registran otros módulos como probados. |
| 04 Maestro detalle | COMPLETADO | El Integrante 1, Yefry Yahir García García, confirmó Traslados: Consultar con detalle, Agregar, Buscar, Editar y Eliminar. |
| 05 Git colaborativo | PARCIAL | El Integrante 1, Yefry Yahir García García, confirmó su parte: repositorio privado, rama `main` publicada, clon en `C:\2026_Progra2_Integracion` y rama `integrante1` publicada. Faltan ramas de los otros integrantes, Pull Requests y la confirmación de los tres; no está finalizado. |
| 06 Login y roles | BLOQUEADO | Debe quedar para la etapa final. |
| 07 Panel de navegación | BLOQUEADO | Requiere autorización posterior. |
| 08 Dashboard | COMPLETADO | El Integrante 1, Yefry Yahir García García, confirmó contra SQL Server: Ventas de hoy, Productos activos, Traslados pendientes, Clientes registrados, Ventas de los últimos 7 días, Estado de existencias y Actividad reciente. El diseño se conservó sin modificar `DashboardForms.Designer.cs`. |
| 09 Revisión del proyecto | BLOQUEADO | Requiere autorización posterior. |
| 10 Publicación | BLOQUEADO | Requiere autorización posterior. |
| 11 Publicación complementaria | PENDIENTE | Sin inicio autorizado. |

## Base documental comprobada

- Están los archivos raíz requeridos, las carpetas planas `skills/`, `database/` y `docs/`, y los once documentos de trabajo.
- Existe `src/SistemaFerreterias/` con la solución y los proyectos Entidad, Datos, Negocio y Presentación.
- El catálogo de `db_Ferreteria` se verificó en solo lectura el 16 de octubre: 17 tablas y 19 claves foráneas. El detalle está en `database/CONEXION_DATABASE.md`.
- El respaldo `Docker/Backups/db_Ferreteria_20261016.bak` superó `RESTORE VERIFYONLY` y su hash coincide con el archivo del contenedor.

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

No se han iniciado pruebas nuevas ni código adicional con esta actualización documental. Los documentos bloqueados permanecen bloqueados.

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
