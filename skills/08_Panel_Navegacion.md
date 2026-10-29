# Documento 07 — Panel de navegación

Responsable: Integrante 2.

Estado al 29 de octubre de 2026: implementado y probado técnicamente;
pendiente de revisión visual y aprobación final del estudiante.

Este archivo faltaba en el espacio de trabajo y se creó para registrar la
implementación autorizada y las pruebas verificadas. No representa una
confirmación de cierre del documento.

## Alcance y decisiones

- Trabajar en el repositorio real `C:\2026_ProgramacionII`; la carpeta
  `C:\2026_Progra2_ProyectoFinal` indicada en la guía no existe en este equipo.
- Solución: `src/SistemaFerreterias/SistemaFerreterias.slnx`.
- Formularios existentes; no se crearon pantallas ni se agregaron bibliotecas.
- `MenuPrincipalForms.Designer.cs` define la distribución, las secciones,
  los botones y sus eventos. Se reutilizan los nombres de los seis botones
  originales, `tlpMenu`, `panelEncabezado`, `panelInferior`, `PanelDivisor`,
  `pictureBox1`, `lblTitulo` y `lblSubtitulo`. La distribución anterior de
  tarjetas y el menú horizontal se sustituyeron por la navegación lateral
  autorizada. Se conserva la imagen de `MenuPrincipalForms.resx`.
- `MenuPrincipalForms.cs` conecta Click con `Navegar` y `AbrirModulo`.
  No construye controles visuales durante la ejecución.
- Los módulos usan `Show()` como formularios sin borde ni nivel superior,
  dentro de `contenido`. No se abren como ventanas modales de navegación.
- Un segundo clic en el módulo activo conserva su instancia. Al cambiar de
  módulo se cierra y libera el anterior. Dashboard permite volver al inicio.
- El contenedor ajusta el tamaño del módulo y permite desplazamiento cuando
  la ventana es menor que el tamaño mínimo del formulario. El menú lateral
  también tiene desplazamiento; sus últimas opciones siguen accesibles.
- `Salir` conserva la salida de la aplicación. No se presenta como cierre
  de sesión con regreso al Login.
- Se conserva el Login y los permisos aprobados del Documento 06, ya
  integrados antes de esta tarea. No se agregaron nuevas reglas de acceso
  ni se modificaron Login, Dashboard, CRUD, Datos o Negocio para este documento.

## Mapa de navegación verificado

| Sección | Control del diseñador | Formulario real |
|---|---|---|
| Inicio | btnMenuDashboard | DashboardForms |
| Operaciones | btnMenuVentas | VentasForms |
| Operaciones | btnMenuCompras | ComprasForms |
| Operaciones | btnMenuPagos | PagosForms |
| Operaciones | btnMenuTraslados | TrasladosForms |
| Catálogos | btnMenuProductos | ProductosForms |
| Catálogos | btnMenuCategorias | CategoriasForms |
| Catálogos | btnMenuClientes | ClientesForms |
| Catálogos | btnMenuProveedores | ProveedoresForms |
| Existencias | btnMenuInventarios | InventariosForms |
| Existencias | btnMenuStock | StockForms |
| Existencias | btnMenuKardex | KardexForms |
| Administración | btnMenuSucursales | SucursalesForms |
| Administración | btnMenuEmpleados | EmpleadosForms |
| Administración | btnMenuUsuarios | UsuariosForms |
| Administración | btnMenuRoles | RolesForms |

Todos los enlaces usan Click → Navegar → AbrirModulo. El botón `btnSalir`
usa Click → btnSalir_Click → Application.Exit().

## Resultados comprobados

El 29 de octubre de 2026, MSBuild de Visual Studio 2022 compiló los cuatro
proyectos sin errores. Un ejecutable local de verificación, con la configuración
de la aplicación y consultas de lectura, comprobó:

1. Los 16 botones abren el tipo de formulario indicado en el mapa.
2. Cada formulario queda alojado en el contenedor del Panel.
3. Cambiar de opción libera el formulario anterior.
4. Repetir cada opción conserva la misma instancia.
5. Cerrar Clientes y abrirlo de nuevo crea una instancia válida.
6. Maximizar y restaurar funcionan.
7. En tamaños solicitados de 1400 × 900 y 920 × 640, el desplazamiento
   permite alcanzar el extremo del módulo; los tamaños físicos pueden variar
   según la escala de Windows.
8. El desplazamiento del menú permite alcanzar Roles y accesos.
9. Cajero mantiene cuatro opciones visibles y Bodeguero ocho.

Las capturas locales `PanelNuevo.png` y `PanelPequeno.png`, inspeccionadas
visualmente, y el ejecutable de verificación están en
`src/SistemaFerreterias/Presentacion/bin/CodexPanelRevision/`, ignorado por Git.
No se ejecutaron operaciones CRUD de escritura ni se modificó SQL Server.

## Revisión pendiente del estudiante

Actualización posterior autorizada: el Login conserva fondo azul marino y letras
blancas; las demás pantallas conservan fondo blanco. El Panel incorpora un perfil
circular naranja con iniciales, menú de usuario/rol y opción Cerrar sesión, además
de un botón Cerrar sesión separado de Salir. La prueba de sesión confirmó ingreso
real, consulta del perfil, retorno al Login con campos vacíos, ausencia de Panel
residual y un segundo ingreso/cierre desde el menú del perfil. La solución compiló
sin errores. Evidencias locales: `PanelPerfilFinal.png` y `LoginAzulMarino.png`.

Preferencia visual vigente: fondo blanco en las pantallas y azul marino en el
menú y los acentos. El ajuste posterior del 29 de octubre separó Fondo de Marino,
aumentó la tipografía pequeña y el espacio de encabezados y paginación. Se
compiló sin errores y se abrieron los 14 listados con la escala local; se
inspeccionaron Pagos, Stock, Proveedores, Dashboard y Login. Las capturas locales
`DashboardBlanco.png` y `PagosBlanco.png` reflejan esta preferencia, que sustituye
el fondo marino solicitado anteriormente. Falta la aprobación visual del estudiante.

### Ajuste visual posterior autorizado — 29 de octubre de 2026

Se aplicó la paleta azul marino compartida y se sustituyeron las barras nativas
del menú y del contenedor principal por `PanelDesplazable`, con indicadores
delgados y redondeados. Las barras propias de otros controles, como las tablas,
se conservan. Se verificaron rueda de alta resolución, arrastre vertical,
teclado, límites y restablecimiento al ampliar. La prueba de ventana pequeña
confirmó acceso al extremo del módulo y a Roles y accesos. La solución compiló
sin errores. Las capturas actualizadas son `PanelMarino.png`,
`PanelMarinoExtremo.png`, `DashboardMarino.png` y `LoginMarino.png`, en la carpeta
local de evidencias indicada arriba. Sigue pendiente la aprobación visual final.

- Compilar en Visual Studio 2022 y revisar el Panel con la escala de su pantalla.
- Abrir sus módulos, volver a Dashboard y confirmar la agrupación del menú.
- Revisar el desplazamiento horizontal y vertical al restaurar la ventana.
- Confirmar el cierre del Documento 07 o solicitar mejoras.

No marcar como aprobado ni hacer commit o push sin autorización.
