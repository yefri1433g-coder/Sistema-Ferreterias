# Arquitectura

La aplicación usa cuatro proyectos .NET Framework 4.8:

1. **Presentacion**: formularios Windows Forms y navegación.
2. **Negocio**: validación y reglas de uso.
3. **Datos**: consultas SQL Server y transacciones.
4. **Entidad**: modelos transferidos entre capas.

Flujo habitual: el formulario invoca una clase de Negocio; Negocio valida los
datos y llama a Datos; Datos ejecuta consultas sobre `db_Ferreteria`.

La conexión se obtiene de la variable de entorno `FERRETERIA_CONNECTION_STRING`
si está definida, y de lo contrario de `src/Presentacion/App.config`.

## Esquema

`database/scripts/SCRIPT NUEVO2.sql` define el esquema esperado por todas las pantallas.
La pantalla de Compras registra la cabecera y los detalles en sus tablas, y
actualiza inventario por sucursal en `Tbl_Stock` y el historial en `Tbl_Kardex`.
