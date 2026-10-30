# Documento 10 — Publicación y entrega final

Guía reconstruida el 29 de octubre de 2026 a partir de las instrucciones
del grupo. El archivo no existía en este repositorio. No acredita el cierre
de documentos ni la aprobación individual de los integrantes.

## Alcance autorizado

- Entrega por carpeta de la aplicación Windows Forms existente, C# y .NET Framework 4.8.
- Compilar la solución en Release con Visual Studio 2022 y conservar el proyecto original.
- Copiar el ejecutable, todas sus dependencias y su configuración.
- Usar SQL Server existente; no introducir servicios, instaladores ni tecnologías web.
- Mantener contraseñas y respaldos fuera de Git.

## Procedimiento

1. Leer README.md, AGENTS.md, MEMORY.md, ESTADO_DOCUMENTOS.md y
   database/CONEXION_DATABASE.md.
2. Revisar el cierre real de los documentos 01 al 09 y registrar lo pendiente.
3. Revisar el plan de entrega y obtener la confirmación del grupo antes de publicar.
4. Compilar `src/SistemaFerreterias/SistemaFerreterias.slnx` en Release.
5. Preparar una carpeta limpia con los archivos de ejecución de Presentacion.
6. Configurar la conexión localmente y probar el equipo Windows de destino.
7. Comprobar Login, Panel, Dashboard, navegación y CRUD con los responsables.
8. Crear un respaldo COPY_ONLY con CHECKSUM, ejecutar RESTORE VERIFYONLY,
   copiarlo fuera de Docker y comparar SHA-256.
9. Registrar en docs/ENTREGA_FINAL.md los resultados y pendientes reales.
10. Proponer archivos y mensaje de commit. Esperar autorización específica
    antes del commit y push finales.

Una prueba en otra carpeta del mismo equipo no equivale a una prueba en otro
Windows. No declarar finalizado el proyecto sin las tres confirmaciones.
