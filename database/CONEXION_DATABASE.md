# Conexión y esquema verificado de la base de datos

**Base:** `db_Ferreteria`  
**Servidor:** `localhost,1434`  
**Esquema consultado:** `dbo`  
**Verificación:** conexión con `sqlcmd` Go 1.6.0 y consultas de solo lectura a los catálogos de SQL Server, 13 de octubre de 2026.

No se incluyen contraseñas en este documento. La conexión se realizó con autenticación de SQL Server. La versión mostrada por SSMS es SQL Server `16.0.4265.3`.

## Resumen

- 17 tablas de usuario en el esquema `dbo`.
- 94 columnas inspeccionadas.
- 17 claves primarias, todas de una columna.
- 20 restricciones de clave foránea, registradas como relaciones origen → destino.
- La consulta anterior de `TOP (5)` solo mostraba cinco nombres; la inspección actual consultó todas las tablas.

## Tablas y columnas

`NOT NULL` indica que la columna no acepta nulos; `NULL` indica que sí los acepta. `PK` identifica la clave primaria según el catálogo de SQL Server.

### `dbo.Tbl_Categorias`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_categoria` | `int` | NOT NULL | PK |
| `nombre` | `varchar(60)` | NOT NULL | |
| `descripcion` | `varchar(150)` | NULL | |

### `dbo.Tbl_Clientes`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_cliente` | `int` | NOT NULL | PK |
| `nombre` | `varchar(100)` | NOT NULL | |
| `nit` | `varchar(15)` | NULL | |
| `telefono` | `varchar(15)` | NULL | |
| `direccion` | `varchar(150)` | NULL | |

### `dbo.Tbl_Compras`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_compra` | `int` | NOT NULL | PK |
| `id_proveedor` | `int` | NOT NULL | FK |
| `fecha` | `date` | NOT NULL | |
| `total` | `decimal(12,2)` | NOT NULL | |
| `id_sucursal` | `int` | NULL | FK |

### `dbo.Tbl_DetalleDeCompras`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_detalle_compra` | `int` | NOT NULL | PK |
| `id_compra` | `int` | NOT NULL | FK |
| `id_producto` | `int` | NOT NULL | FK |
| `cantidad` | `int` | NOT NULL | |
| `precio_unitario` | `decimal(10,2)` | NOT NULL | |

### `dbo.Tbl_DetallesVentas`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_detalle_venta` | `int` | NOT NULL | PK |
| `id_venta` | `int` | NOT NULL | FK |
| `id_producto` | `int` | NOT NULL | FK |
| `cantidad` | `int` | NOT NULL | |
| `precio_unitario` | `decimal(10,2)` | NOT NULL | |

### `dbo.Tbl_DetalleTraslado`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_detalle_traslado` | `int` | NOT NULL | PK |
| `id_traslado` | `int` | NOT NULL | FK |
| `id_producto` | `int` | NOT NULL | FK |
| `cantidad` | `int` | NOT NULL | |

### `dbo.Tbl_Empleados`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_empleado` | `int` | NOT NULL | PK |
| `id_usuario` | `int` | NOT NULL | FK |
| `id_sucursal` | `int` | NOT NULL | FK |
| `nombre` | `varchar(60)` | NOT NULL | |
| `apellido` | `varchar(60)` | NOT NULL | |
| `cargo` | `varchar(50)` | NOT NULL | |
| `telefono` | `varchar(15)` | NULL | |
| `estado` | `bit` | NOT NULL | |

### `dbo.Tbl_Inventarios`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_inventario` | `int` | NOT NULL | PK |
| `id_sucursal` | `int` | NOT NULL | FK |
| `fecha` | `datetime` | NOT NULL | |
| `descripcion` | `varchar(150)` | NULL | |

### `dbo.Tbl_Kardex`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_kardex` | `int` | NOT NULL | PK |
| `id_producto` | `int` | NOT NULL | FK |
| `fecha` | `datetime` | NOT NULL | |
| `tipo_movimiento` | `varchar(20)` | NOT NULL | |
| `cantidad` | `int` | NOT NULL | |
| `saldo` | `int` | NOT NULL | |
| `descripcion` | `varchar(150)` | NULL | |

### `dbo.Tbl_Pagos`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_pago` | `int` | NOT NULL | PK |
| `id_venta` | `int` | NOT NULL | FK |
| `fecha` | `datetime` | NOT NULL | |
| `monto` | `decimal(12,2)` | NOT NULL | |
| `metodo_pago` | `varchar(30)` | NOT NULL | |

### `dbo.Tbl_Productos`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_producto` | `int` | NOT NULL | PK |
| `id_categoria` | `int` | NOT NULL | FK |
| `codigo` | `varchar(30)` | NOT NULL | |
| `nombre` | `varchar(100)` | NOT NULL | |
| `descripcion` | `varchar(200)` | NULL | |
| `precio_costo` | `decimal(10,2)` | NOT NULL | |
| `precio_venta` | `decimal(10,2)` | NOT NULL | |
| `estado` | `bit` | NOT NULL | |
| `unidad_medida` | `varchar(50)` | NULL | |

### `dbo.Tbl_Proveedores`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_proveedor` | `int` | NOT NULL | PK |
| `nombre` | `varchar(100)` | NOT NULL | |
| `contacto` | `varchar(80)` | NULL | |
| `telefono` | `varchar(15)` | NULL | |
| `correo` | `varchar(100)` | NULL | |
| `direccion` | `varchar(150)` | NULL | |

### `dbo.Tbl_Stock`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_stock` | `int` | NOT NULL | PK |
| `id_sucursal` | `int` | NOT NULL | FK |
| `id_producto` | `int` | NOT NULL | FK |
| `cantidad` | `int` | NOT NULL | |
| `stock_minimo` | `int` | NOT NULL | |

### `dbo.Tbl_Sucursales`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_sucursal` | `int` | NOT NULL | PK |
| `nombre` | `varchar(80)` | NOT NULL | |
| `direccion` | `varchar(150)` | NOT NULL | |
| `telefono` | `varchar(15)` | NULL | |
| `estado` | `bit` | NOT NULL | |
| `municipio` | `varchar(100)` | NULL | |
| `departamento` | `varchar(100)` | NULL | |
| `horario_atencion` | `varchar(100)` | NULL | |

### `dbo.Tbl_Traslados`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_traslado` | `int` | NOT NULL | PK |
| `id_sucursal_origen` | `int` | NOT NULL | FK |
| `id_sucursal_destino` | `int` | NOT NULL | FK |
| `fecha` | `datetime` | NOT NULL | |
| `estado` | `bit` | NOT NULL | |
| `id_empleado` | `int` | NOT NULL | FK |

### `dbo.Tbl_Usuarios`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_usuario` | `int` | NOT NULL | PK |
| `nombre_usuario` | `varchar(50)` | NOT NULL | |
| `contrasena` | `varchar(100)` | NOT NULL | |
| `rol` | `varchar(30)` | NOT NULL | |
| `estado` | `bit` | NOT NULL | |

### `dbo.Tbl_Ventas`

| Columna | Tipo | Nulabilidad | Clave |
|---|---|---|---|
| `id_venta` | `int` | NOT NULL | PK |
| `id_cliente` | `int` | NOT NULL | FK |
| `fecha` | `datetime` | NOT NULL | |
| `total` | `decimal(12,2)` | NOT NULL | |

## Claves primarias

Todas son PK de una columna; las columnas marcadas `PK` en cada tabla son las que devuelve `sys.key_constraints`.

## Relaciones de clave foránea comprobadas

| Origen | Destino |
|---|---|
| `Tbl_Compras.id_proveedor` | `Tbl_Proveedores.id_proveedor` |
| `Tbl_Compras.id_sucursal` | `Tbl_Sucursales.id_sucursal` |
| `Tbl_DetalleDeCompras.id_compra` | `Tbl_Compras.id_compra` |
| `Tbl_DetalleDeCompras.id_producto` | `Tbl_Productos.id_producto` |
| `Tbl_DetallesVentas.id_producto` | `Tbl_Productos.id_producto` |
| `Tbl_DetallesVentas.id_venta` | `Tbl_Ventas.id_venta` |
| `Tbl_DetalleTraslado.id_producto` | `Tbl_Productos.id_producto` |
| `Tbl_DetalleTraslado.id_traslado` | `Tbl_Traslados.id_traslado` |
| `Tbl_Empleados.id_sucursal` | `Tbl_Sucursales.id_sucursal` |
| `Tbl_Empleados.id_usuario` | `Tbl_Usuarios.id_usuario` |
| `Tbl_Inventarios.id_sucursal` | `Tbl_Sucursales.id_sucursal` |
| `Tbl_Kardex.id_producto` | `Tbl_Productos.id_producto` |
| `Tbl_Pagos.id_venta` | `Tbl_Ventas.id_venta` |
| `Tbl_Productos.id_categoria` | `Tbl_Categorias.id_categoria` |
| `Tbl_Stock.id_producto` | `Tbl_Productos.id_producto` |
| `Tbl_Stock.id_sucursal` | `Tbl_Sucursales.id_sucursal` |
| `Tbl_Traslados.id_empleado` | `Tbl_Empleados.id_empleado` |
| `Tbl_Traslados.id_sucursal_origen` | `Tbl_Sucursales.id_sucursal` |
| `Tbl_Traslados.id_sucursal_destino` | `Tbl_Sucursales.id_sucursal` |
| `Tbl_Ventas.id_cliente` | `Tbl_Clientes.id_cliente` |

Las tablas y columnas de origen y destino se obtuvieron de `sys.foreign_key_columns`; no se dedujeron relaciones a partir de nombres parecidos. No se documentaron valores ni datos de filas.
