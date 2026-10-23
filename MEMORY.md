# MEMORY.md — Estado del Proyecto

## Completado
- Estructura inicial del repositorio creada.
- Git inicializado.
- Repositorio GitHub creado.
- SqlServer preparado con Docker.
- Acceso al repositorio compartido con el equipo.

## En desarrollo
- Preparación del entorno de IA.

## Avance verificado: Stock y Productos
- La solución compiló en configuración Debug.
- Se verificaron contra SQL Server las operaciones de agregar, consultar, editar y eliminar productos.
- Se verificaron contra SQL Server las operaciones de agregar, consultar, buscar, editar y eliminar stock.
- Los registros temporales de prueba se limpiaron; la base quedó con 5 productos y 5 registros de stock.
- El estudiante probó en la interfaz y confirmó que los botones Editar de Stock y Productos funcionan.
- `ESTADO_DOCUMENTOS.md` no estaba presente en el workspace al momento de la revisión.

## Pendiente
- Crear solución por capas.
- Configurar SqlServer.
- Crear CRUD patrón Sucursales.
- Desarrollar módulos individuales.
- Implementar maestro-detalle.
- Implementar Login.
- Implementar Panel de Navegación.
- Implementar Dashboard.
- Publicar la base de datos.
- Integrar y revisar el proyecto final.

## Decisiones
- Se utilizará arquitectura Entidad, Datos, Negocio y Presentación.
- Sucursales será el módulo patrón del proyecto.
- Los Skills se crearán por patrón de desarrollo y no por tabla.
