# Reglas del proyecto Programación II

## Antes de trabajar

1. Leer `README.md` y `MEMORY.md`.
2. Revisar el módulo y el esquema SQL afectados.
3. Definir un plan breve antes de modificar código.
4. Seguir el skill indicado por la tarea, si corresponde.

## Entorno y comandos

- En Windows, preferir `cmd.exe` cuando esté disponible. No usar PowerShell si CMD permite realizar la tarea solicitada.
- No instalar herramientas ni ejecutar scripts de base de datos sin autorización.
- Mantener los documentos basados en hechos comprobados; distinguir claramente los pendientes y las instrucciones.

## Documentos y autorización

- El Documento 01 está registrado como aprobado en `ESTADO_DOCUMENTOS.md`.
- Mantener los documentos 02 al 10 bloqueados hasta recibir autorización expresa para el módulo o funcionalidad correspondiente.
- Antes de avanzar a una conexión o inspección de SQL Server en el flujo inicial, mostrar el árbol real del proyecto y esperar que el estudiante lo confirme.
- No iniciar CRUD hasta completar y autorizar las etapas previas.
- Realizar Git/GitHub después de los CRUD y antes de Login. Dejar Login y roles para el final.
- No crear otra solución, formularios nuevos, ni agregar Login, roles o Dashboard fuera del alcance y autorización vigentes.
- Conservar los diseños existentes.
- `skills/`, `database/` y `docs/` son carpetas planas, sin subcarpetas. Solo `src/` contiene el sistema existente.

## Tecnologías y arquitectura

- C# con .NET Framework, Windows Forms, SQL Server y `System.Data.SqlClient`.
- SQL Server Management Studio (SSMS) se usa para administrar la base de datos.
- Entidad: clases para representar y transportar datos.
- Datos: conexión y operaciones parametrizadas con SQL Server.
- Negocio: reglas y validaciones.
- Presentación: formularios Windows Forms.
- Presentación utiliza Negocio; Negocio utiliza Datos y Entidad; Datos utiliza Entidad. Presentación no accede directamente a SQL Server.
- Aplicar POO básica, consultas SQL parametrizadas y `try-catch` donde corresponda.
- Incluir comentarios didácticos apropiados para el curso.

## Base de datos

- Respetar las tablas `Tbl_`, el modelo existente y las columnas físicas de claves con prefijo `id_`.
- No renombrar claves a `Codigo...`, crear tablas o relaciones adicionales, ni cambiar el modelo sin autorización.
- Usar parámetros para valores en consultas SQL.
- No guardar contraseñas reales en archivos versionados.
- SSMS es el cliente de administración; la aplicación se conecta directamente al motor de SQL Server.

## Forma de trabajo

- Mantener el código claro y apropiado para el nivel del curso; evitar complejidad y cambios fuera del módulo solicitado.
- Explicar los cambios realizados y no afirmar resultados que no se verificaron.
- Actualizar `MEMORY.md` solo con resultados comprobados.
- Revisar `git status` antes de registrar cambios. Mantener el repositorio local sin remoto de GitHub.
