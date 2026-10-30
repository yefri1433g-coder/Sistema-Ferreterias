# Documento 10 — Preparación de entrega

Fecha: 29 de octubre de 2026. Carpeta de trabajo autorizada:
`C:\2026_ProgramacionII`. El estudiante autorizó continuar con el plan.
El cambio remoto `d50df1a` registra la confirmación de los tres integrantes
para el Documento 09. Se conserva esa confirmación; la aprobación final de
la entrega y las pruebas en otro Windows continúan pendientes.

## Artefactos preparados

- Solución: `src/SistemaFerreterias/SistemaFerreterias.slnx`.
- Inicio: `Presentacion`, WinExe, .NET Framework 4.8, AnyCPU.
- Compilación Release con MSBuild 17.14 de Visual Studio 2022: correcta.
- Salida: `src/SistemaFerreterias/Presentacion/bin/Release/`.
- Aplicación copiada: `entrega/ProyectoFinal_20261029/aplicacion/`, 15 archivos
  de ejecución, incluido Presentacion.exe.config; sin contraseñas en ese archivo.
- Se inició directamente el ejecutable copiado: abrió
  `Acceso | Ferretería Multisucursal` y el proceso respondió.
- Esta prueba fue en otra carpeta del mismo equipo, no en otro Windows.
- Respaldo local verificado: ver `database/CONEXION_DATABASE.md`.
- Paquete: `entrega/ProyectoFinal_20261029.zip`. Se verificaron los hashes de
  los 15 archivos de ejecución y del respaldo dentro del ZIP contra SHA256.txt.

La prueba automatizada inicial no confirmó la ventana al iniciarla oculta.
El comprobador posterior fue bloqueado por la protección de Windows y no se
utilizó como evidencia de éxito. La comprobación válida fue el inicio directo
del ejecutable de entrega y la ventana de Login observada por el proceso.
No se desactivaron protecciones del equipo.

## Lista final

- [ ] Estado documental consistente: el 09 consta completado, pero las filas
  del 03, 05 y 07 todavía muestran pendientes que deben reconciliarse con ese cierre.
- [x] Solución actual compila en Release.
- [x] Ejecutable copiado abre Login fuera de la carpeta original.
- [ ] Ejecutable abre en otro equipo Windows.
- [x] Conexión SQL Server comprobada desde este equipo.
- [ ] Conexión SQL Server comprobada desde el equipo de destino.
- [ ] Login con credenciales, Panel, Dashboard y CRUD probados en la entrega por el grupo.
- [x] Respaldo .bak disponible fuera de Docker y verificado.
- [x] Requisitos e instrucciones de instalación documentados.
- [ ] Evidencias del equipo de destino y pruebas del grupo.
- [x] Propuesta de documentación revisada y commit/push autorizados por el estudiante.
- [ ] Integrantes 1, 2 y 3 confirman la entrega.

## Registro del grupo pendiente

| Responsable | Revisión de la entrega |
|---|---|
| Yefry Yahir Garcia Garcia | Pendiente: Inventarios, Stock, Kardex, Proveedores, Traslados y Dashboard. |
| Gustavo Alfredo Alecio Gonzales | Pendiente: Categorías, Productos, Sucursales y Compras. |
| Brandon Josue Barias Monroy | Pendiente: Login, Usuarios, Clientes, Ventas, Pagos, menú e integración. |

Registrar equipo, fecha, operación, resultado y evidencia después de cada
prueba. Las verificaciones de desarrollo anteriores no sustituyen esta prueba
del ejecutable entregado. El proyecto no se declara finalizado.

## Propuesta de commit, sin ejecutar

Incluir README.md, MEMORY.md, ESTADO_DOCUMENTOS.md, .gitignore,
skills/11_Publicacion.md, database/CONEXION_DATABASE.md,
database/01_Backup_Base_Datos.sql y este documento. El cambio local del
diseñador en Presentacion.csproj debe revisarse antes de incluirlo.

Mensaje propuesto: `Preparar entrega Release y documentar respaldo y validaciones pendientes`.
Excluir entrega/, .bak, .zip, bin/, obj/, .vs/ y credenciales. El estudiante
autorizó esta subida; no equivale a la autorización final de entrega del grupo.
Se integró también la referencia remota a otra carpeta de entrega en
`C:\2026_Progra2_Entrega\Presentacion_Release`; los 15 archivos y el respaldo
verificados aquí corresponden a `entrega/ProyectoFinal_20261029/`.
