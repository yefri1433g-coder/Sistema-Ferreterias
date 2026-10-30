# Sistema Ferreterías

Aplicación de escritorio en C# y Windows Forms para gestionar usuarios,
clientes, ventas, pagos, productos, categorías, sucursales, compras,
inventarios, stock, kardex y traslados. Usa SQL Server.

## Requisitos

- Windows y Visual Studio con .NET Framework 4.8.
- SQL Server local o un contenedor SQL Server.
- Restaurar los paquetes NuGet al abrir la solución.

## Preparación de la base de datos

1. Crea la base ejecutando [`database/scripts/SCRIPT NUEVO2.sql`](database/scripts/SCRIPT%20NUEVO2.sql)
   en SQL Server Management Studio.
2. Reemplaza las contraseñas `CAMBIAR_CLAVE` de las cuentas de ejemplo por
   contraseñas locales antes de usar el sistema.
3. Si usas el script, la base se llama `db_Ferreteria`; SQL Server suele ignorar
   mayúsculas y minúsculas en el nombre.

Para una base existente, ejecuta también
[`database/scripts/20261023_compras_sucursal.sql`](database/scripts/20261023_compras_sucursal.sql).
La migración agrega la sucursal opcional a las compras antiguas sin asignarles una.
Esas compras quedan bloqueadas para edición hasta que se registre su sucursal correcta.

Para bases antiguas sin sucursal en Ventas, ejecuta también
[`database/scripts/20261029_ventas_sucursal.sql`](database/scripts/20261029_ventas_sucursal.sql).
Las ventas antiguas conservan la sucursal sin asignar.

Para distinguir Pendiente, En tránsito y Completado en Traslados, la migración
[`database/scripts/20261029_traslados_estado.sql`](database/scripts/20261029_traslados_estado.sql)
amplía `Tbl_Traslados.estado` de `BIT` a `TINYINT`. Conserva 0 = Completado
y 1 = Pendiente, y permite 2 = En tránsito. Este ajuste del modelo fue aprobado
el 29 de octubre de 2026. En bases existentes se debe aplicar esta migración;
sin ella, En tránsito muestra un aviso y no se guarda como Pendiente.

Para habilitar el Login con roles y accesos configurables, ejecuta también
[`database/scripts/20261028_login_roles.sql`](database/scripts/20261028_login_roles.sql).
La migración conserva las cuentas antiguas, reasigna `Administrador` a `Supervisor`
y `Vendedor` a `Cajero`, y crea los tres roles iniciales y sus módulos permitidos.
La reversión controlada está en `database/scripts/20261028_login_roles_rollback.sql`;
solo procede mientras no se haya personalizado la configuración de roles.

El código actual usa el esquema de ese script. La migración en
`database/scripts/legacy/` pertenece a una versión anterior y no se debe ejecutar para
la versión actual.

## Conexión

La aplicación lee primero la variable de usuario `FERRETERIA_CONNECTION_STRING`,
después la variable del proceso y, si ninguna está definida, usa la conexión
de `App.config`.
Para SQL Server con autenticación SQL, define la cadena localmente antes de
abrir Visual Studio, sustituyendo los valores por los de tu instalación:

```powershell
$env:FERRETERIA_CONNECTION_STRING = 'Server=localhost,1434;Database=db_ferreteria;User Id=sa;Password=<tu-clave>;Encrypt=False;TrustServerCertificate=True'
```

No guardes claves reales en `App.config`, scripts compartidos ni en Git.

## Ejecutar

Abre `src/SistemaFerreterias/SistemaFerreterias.slnx` en Visual Studio, restaura NuGet y ejecuta el
proyecto `Presentacion`. También se puede compilar con MSBuild:

```powershell
& 'C:\Program Files\Microsoft Visual Studio\18\Insiders\MSBuild\Current\Bin\MSBuild.exe' `
  '.\src\SistemaFerreterias\SistemaFerreterias.slnx' /t:Build /p:Configuration=Debug /v:minimal
```

## Instalación y entrega por carpeta

La preparación del 29 de octubre de 2026 está en
[`docs/ENTREGA_FINAL.md`](docs/ENTREGA_FINAL.md). Se compiló Release con
Visual Studio 2022 y se preparó `entrega/ProyectoFinal_20261029/aplicacion/`.
Esta carpeta y el respaldo son artefactos locales excluidos de Git.

En el equipo de destino:

1. Disponer de Windows con .NET Framework 4.8. Visual Studio no es necesario
   para ejecutar la aplicación.
2. Copiar la carpeta `aplicacion` completa: Presentacion.exe, las DLL y
   Presentacion.exe.config. No copiar únicamente el ejecutable.
3. Configurar localmente la conexión a SQL Server según
   [`database/CONEXION_DATABASE.md`](database/CONEXION_DATABASE.md).
   Si el servidor está en otro equipo, usar su IP/nombre y puerto; localhost
   representa siempre el equipo donde se ejecuta la aplicación.
4. Ejecutar `Presentacion.exe` y comprobar Login, Panel, Dashboard y CRUD.
   Registrar pruebas y responsables en el documento de entrega.

La solución usa .NET Framework 4.8 y Presentacion es el proyecto de inicio.
La salida Release es `src/SistemaFerreterias/Presentacion/bin/Release/`.
La copia preparada contiene las dependencias producidas por la compilación,
incluidos FontAwesome.Sharp y los ensamblados de recursos requeridos.

Existe un respaldo real verificado, disponible localmente fuera de Docker;
su ubicación y SHA-256 figuran en el documento de conexión. El script
[`database/01_Backup_Base_Datos.sql`](database/01_Backup_Base_Datos.sql)
sirve para preparar respaldos posteriores después de elegir una ruta nueva.

El ejecutable copiado abrió Login en este mismo equipo. **Siguen pendientes
la prueba en otro Windows, sus pruebas CRUD y la autorización de los tres
integrantes.** Esta preparación no declara finalizado el proyecto.

## Estructura

- `src/`: proyectos `Datos`, `Entidad`, `Negocio` y `Presentacion`.
- `database/scripts/`: script de creación y scripts antiguos en `legacy/`.
- `database/data/` y `database/backups/`: espacio para datos exportados y copias locales (ignorados por Git).
- `docker/`: configuración opcional para SQL Server local.
- `docs/`: guías y diagramas; aportes en `docs/guides/aportes.md`.
- `skills/11_Publicacion.md`: procedimiento de publicación y entrega final.
- `MEMORY.md`: resumen técnico y estado del proyecto.

## Equipo

Los integrantes y sus módulos están registrados en
[`docs/guides/aportes.md`](docs/guides/aportes.md).
