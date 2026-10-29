using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entidad;

namespace Datos
{

    // CLASE CatalogosGestionDatos: Realiza consultas y operaciones de persistencia en SQL Server.
    public class CatalogosGestionDatos
    {
        private readonly ConexionDatos conexion = new ConexionDatos();

        // CONSULTAR: Leer
        private DataTable Leer(string sql)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
            {
                DataTable tabla = new DataTable();
                da.Fill(tabla);
                return tabla;
            }
        }

        public DataTable Categorias()
        {
            return Leer("SELECT id_categoria AS Codigo, nombre AS Nombre, descripcion AS Descripcion FROM Tbl_Categorias ORDER BY nombre;");
        }

        public DataTable Productos()
        {
            return Leer(@"SELECT p.id_producto AS Codigo, p.codigo AS SKU, p.nombre AS Nombre,
                                  p.descripcion AS Descripcion,
                                  p.precio_costo AS Costo, p.precio_venta AS Precio,
                                  p.id_categoria AS IdCategoria, c.nombre AS Categoria,
                                  p.estado AS Activo,
                                  ISNULL((SELECT SUM(st.cantidad) FROM Tbl_Stock st WHERE st.id_producto = p.id_producto), 0) AS Existencia,
                                  CASE WHEN EXISTS (SELECT 1 FROM Tbl_Stock st WHERE st.id_producto = p.id_producto AND st.cantidad <= st.stock_minimo) THEN 1 ELSE 0 END AS StockBajo
                           FROM Tbl_Productos p LEFT JOIN Tbl_Categorias c ON c.id_categoria = p.id_categoria
                           ORDER BY p.nombre;");
        }
        public DataTable Sucursales()
        {
            return Leer(@"SELECT s.id_sucursal AS Codigo, s.nombre AS Nombre, s.direccion AS Direccion,
                                  s.telefono AS Telefono, s.estado AS Activo,
                                  (SELECT COUNT(*) FROM Tbl_Empleados e WHERE e.id_sucursal = s.id_sucursal AND e.estado = 1) AS Personal
                           FROM Tbl_Sucursales s ORDER BY s.nombre;");
        }

        // BUSCAR: BuscarSucursales
        public DataTable BuscarSucursales(string criterio)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT s.id_sucursal AS Codigo, s.nombre AS Nombre, s.direccion AS Direccion,
                         s.telefono AS Telefono, s.estado AS Activo,
                         (SELECT COUNT(*) FROM dbo.Tbl_Empleados e WHERE e.id_sucursal = s.id_sucursal AND e.estado = 1) AS Personal
                  FROM dbo.Tbl_Sucursales s
                  WHERE s.nombre LIKE @criterio
                  ORDER BY s.nombre;", cn))
            {
                cmd.Parameters.Add("@criterio", SqlDbType.VarChar, 82).Value = "%" + (criterio ?? "").Trim() + "%";
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable tabla = new DataTable();
                    da.Fill(tabla);
                    return tabla;
                }
            }
        }

        public DataTable Proveedores()
        {
            return Leer("SELECT id_proveedor AS Codigo, nombre AS Nombre FROM Tbl_Proveedores ORDER BY nombre;");
        }

        public DataTable Compras()
        {
            return Leer(@"SELECT c.id_compra AS Codigo, c.id_proveedor AS IdProveedor, c.id_sucursal AS IdSucursal,
                                  ISNULL(s.nombre, 'Sucursal pendiente') AS Sucursal,
                                  p.nombre AS Proveedor, c.fecha AS Fecha, c.total AS Total,
                                  (SELECT COUNT(1) FROM Tbl_DetalleDeCompras d WHERE d.id_compra = c.id_compra) AS Lineas
                           FROM Tbl_Compras c
                           INNER JOIN Tbl_Proveedores p ON p.id_proveedor = c.id_proveedor
                           LEFT JOIN Tbl_Sucursales s ON s.id_sucursal = c.id_sucursal
                           ORDER BY c.fecha DESC, c.id_compra DESC;");
        }

        public DataTable DetalleCompra(int idCompra)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(@"SELECT d.id_producto AS IdProducto, p.codigo AS CodigoProducto, p.nombre AS Producto,
                                                           d.cantidad AS Cantidad, d.precio_unitario AS PrecioUnitario,
                                                           d.cantidad * d.precio_unitario AS Subtotal
                                                    FROM Tbl_DetalleDeCompras d
                                                    INNER JOIN Tbl_Productos p ON p.id_producto = d.id_producto
                                                    WHERE d.id_compra = @idCompra
                                                    ORDER BY p.nombre;", cn))
            {
                cmd.Parameters.Add("@idCompra", SqlDbType.Int).Value = idCompra;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable tabla = new DataTable();
                    da.Fill(tabla);
                    return tabla;
                }
            }
        }

        public DataTable ProductosActivos()
        {
            return Leer("SELECT id_producto AS Codigo, nombre AS Nombre FROM Tbl_Productos WHERE estado = 1 ORDER BY nombre;");
        }

        public DataTable SucursalesActivas()
        {
            return Leer("SELECT id_sucursal AS Codigo, nombre AS Nombre FROM Tbl_Sucursales WHERE estado = 1 ORDER BY nombre;");
        }

        // GUARDAR: AsignarSucursalCompra
        public void AsignarSucursalCompra(int idCompra, int idSucursal)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand("UPDATE Tbl_Compras SET id_sucursal=@sucursal WHERE id_compra=@compra AND id_sucursal IS NULL", cn))
            {
                cmd.Parameters.Add("@sucursal", SqlDbType.Int).Value = idSucursal;
                cmd.Parameters.Add("@compra", SqlDbType.Int).Value = idCompra;
                cn.Open();
                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("No se pudo asignar la sucursal. Actualiza la lista y vuelve a intentar.");
            }
        }

        // GUARDAR: GuardarCategoria
        public void GuardarCategoria(int? id, string nombre, string descripcion)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(id.HasValue
                ? "UPDATE Tbl_Categorias SET nombre=@n, descripcion=@d WHERE id_categoria=@id"
                : "INSERT INTO Tbl_Categorias(nombre, descripcion) VALUES(@n,@d)", cn))
            {
                cmd.Parameters.Add("@n", SqlDbType.VarChar, 60).Value = nombre.Trim();
                cmd.Parameters.Add("@d", SqlDbType.VarChar, 150).Value = (object)(descripcion ?? "").Trim();
                if (id.HasValue) cmd.Parameters.Add("@id", SqlDbType.Int).Value = id.Value;
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        // ELIMINAR: EliminarCategoria
        public void EliminarCategoria(int id)
        {
            Ejecutar("DELETE FROM Tbl_Categorias WHERE id_categoria=@id", id);
        }

        // GUARDAR: GuardarProducto
        public void GuardarProducto(int? id, string codigo, string nombre, string descripcion, decimal costo, decimal precio, int categoria, bool activo)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(id.HasValue
                ? @"UPDATE Tbl_Productos SET codigo=@c,nombre=@n,descripcion=@d,precio_costo=@costo,precio_venta=@p,id_categoria=@cat,estado=@e WHERE id_producto=@id"
                : @"INSERT INTO Tbl_Productos(codigo,nombre,descripcion,precio_costo,precio_venta,id_categoria,estado) VALUES(@c,@n,@d,@costo,@p,@cat,@e)", cn))
            {
                cmd.Parameters.Add("@c", SqlDbType.VarChar, 30).Value = codigo.Trim();
                cmd.Parameters.Add("@n", SqlDbType.VarChar, 100).Value = nombre.Trim();
                cmd.Parameters.Add("@d", SqlDbType.VarChar, 200).Value = (object)(descripcion ?? "").Trim();
                cmd.Parameters.Add("@costo", SqlDbType.Decimal).Value = costo;
                cmd.Parameters["@costo"].Precision = 10; cmd.Parameters["@costo"].Scale = 2;
                cmd.Parameters.Add("@p", SqlDbType.Decimal).Value = precio;
                cmd.Parameters["@p"].Precision = 10; cmd.Parameters["@p"].Scale = 2;
                cmd.Parameters.Add("@cat", SqlDbType.Int).Value = categoria;
                cmd.Parameters.Add("@e", SqlDbType.Bit).Value = activo;
                if (id.HasValue) cmd.Parameters.Add("@id", SqlDbType.Int).Value = id.Value;
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        // ELIMINAR: EliminarProducto
        public void EliminarProducto(int id) { Ejecutar("DELETE FROM Tbl_Productos WHERE id_producto=@id", id); }

        // GUARDAR: GuardarSucursal
        public void GuardarSucursal(int? id, string nombre, string direccion, string telefono, bool activo)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(id.HasValue
                ? @"UPDATE Tbl_Sucursales SET nombre=@n,direccion=@d,telefono=@t,estado=@e WHERE id_sucursal=@id"
                : @"INSERT INTO Tbl_Sucursales(nombre,direccion,telefono,estado) VALUES(@n,@d,@t,@e)", cn))
            {
                cmd.Parameters.Add("@n", SqlDbType.VarChar, 80).Value = nombre.Trim();
                cmd.Parameters.Add("@d", SqlDbType.VarChar, 150).Value = direccion.Trim();
                cmd.Parameters.Add("@t", SqlDbType.VarChar, 15).Value = (object)(telefono ?? "").Trim();
                cmd.Parameters.Add("@e", SqlDbType.Bit).Value = activo;
                if (id.HasValue) cmd.Parameters.Add("@id", SqlDbType.Int).Value = id.Value;
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        // AGREGAR: AgregarSucursal
        public void AgregarSucursal(Sucursal sucursal)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(
                "INSERT INTO dbo.Tbl_Sucursales (nombre, direccion, telefono, estado) " +
                "VALUES (@nombre, @direccion, @telefono, @estado)", cn))
            {
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 80).Value = sucursal.Nombre.Trim();
                cmd.Parameters.Add("@direccion", SqlDbType.VarChar, 150).Value = sucursal.Direccion.Trim();
                cmd.Parameters.Add("@telefono", SqlDbType.VarChar, 15).Value =
                    String.IsNullOrWhiteSpace(sucursal.Telefono) ? (object)DBNull.Value : sucursal.Telefono.Trim();
                cmd.Parameters.Add("@estado", SqlDbType.Bit).Value = sucursal.Estado;

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // EDITAR: EditarSucursal
        public void EditarSucursal(Sucursal sucursal)
        {
            try
            {
                using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE dbo.Tbl_Sucursales
                      SET nombre = @nombre, direccion = @direccion,
                          telefono = @telefono, estado = @estado
                      WHERE id_sucursal = @idSucursal;", cn))
                {
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 80).Value = sucursal.Nombre.Trim();
                    cmd.Parameters.Add("@direccion", SqlDbType.VarChar, 150).Value = sucursal.Direccion.Trim();
                    cmd.Parameters.Add("@telefono", SqlDbType.VarChar, 15).Value =
                        String.IsNullOrWhiteSpace(sucursal.Telefono) ? (object)DBNull.Value : sucursal.Telefono.Trim();
                    cmd.Parameters.Add("@estado", SqlDbType.Bit).Value = sucursal.Estado;
                    cmd.Parameters.Add("@idSucursal", SqlDbType.Int).Value = sucursal.IdSucursal;

                    cn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No se encontró la sucursal seleccionada para editar.");
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "editar la sucursal");
            }
        }

        // ELIMINAR: EliminarSucursal
        public void EliminarSucursal(int id)
        {
            try
            {
                using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM dbo.Tbl_Sucursales WHERE id_sucursal = @idSucursal", cn))
                {
                    cmd.Parameters.Add("@idSucursal", SqlDbType.Int).Value = id;
                    cn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No se encontró la sucursal seleccionada para eliminar.");
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "eliminar la sucursal");
            }
        }

        // ELIMINAR: EliminarSucursalesDePrueba
        public int EliminarSucursalesDePrueba(int ultimoIdOriginal)
        {
            try
            {
                using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM dbo.Tbl_Sucursales WHERE id_sucursal > @ultimoIdOriginal", cn))
                {
                    cmd.Parameters.Add("@ultimoIdOriginal", SqlDbType.Int).Value = ultimoIdOriginal;
                    cn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "limpiar las sucursales de prueba");
            }
        }

        private void Ejecutar(string sql, int id)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        // GUARDAR: GuardarCompra
        public int GuardarCompra(int proveedor, int sucursal, DateTime fecha, IList<CompraLineaCatalogo> detalles)
        {
            if (detalles == null || detalles.Count == 0) throw new InvalidOperationException("Agrega al menos un producto a la compra.");
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                // Agrupa los cambios relacionados para confirmarlos juntos o revertirlos si ocurre un error.
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        decimal total = 0;
                        foreach (CompraLineaCatalogo d in detalles) total += d.Subtotal;
                        int idCompra;
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO Tbl_Compras(id_proveedor,id_sucursal,fecha,total) VALUES(@prov,@sucursal,@fecha,@total); SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                        {
                            cmd.Parameters.Add("@prov", SqlDbType.Int).Value = proveedor;
                            cmd.Parameters.Add("@sucursal", SqlDbType.Int).Value = sucursal;
                            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                            cmd.Parameters.Add("@total", SqlDbType.Decimal).Value = total;
                            cmd.Parameters["@total"].Precision = 12; cmd.Parameters["@total"].Scale = 2;
                            idCompra = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        foreach (CompraLineaCatalogo d in detalles)
                        {
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO Tbl_DetalleDeCompras(id_compra,id_producto,cantidad,precio_unitario) VALUES(@compra,@prod,@cant,@costo)", cn, tx))
                            {
                                cmd.Parameters.Add("@compra", SqlDbType.Int).Value = idCompra;
                                cmd.Parameters.Add("@prod", SqlDbType.Int).Value = d.IdProducto;
                                cmd.Parameters.Add("@cant", SqlDbType.Int).Value = Decimal.ToInt32(d.Cantidad);
                                cmd.Parameters.Add("@costo", SqlDbType.Decimal).Value = d.CostoUnitario;

                                cmd.ExecuteNonQuery();
                            }
                            using (SqlCommand cmd = new SqlCommand("UPDATE Tbl_Stock WITH (UPDLOCK) SET cantidad=cantidad+@cant WHERE id_sucursal=@suc AND id_producto=@prod; IF @@ROWCOUNT=0 INSERT INTO Tbl_Stock(id_sucursal,id_producto,cantidad,stock_minimo) VALUES(@suc,@prod,@cant,0);", cn, tx))
                            {
                                cmd.Parameters.Add("@cant", SqlDbType.Int).Value = Decimal.ToInt32(d.Cantidad);
                                cmd.Parameters.Add("@suc", SqlDbType.Int).Value = sucursal;
                                cmd.Parameters.Add("@prod", SqlDbType.Int).Value = d.IdProducto;
                                cmd.ExecuteNonQuery();
                            }
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO Tbl_Kardex(id_producto,fecha,tipo_movimiento,cantidad,saldo,descripcion) VALUES(@prod,@fecha,'Entrada',@cant,(SELECT ISNULL(SUM(cantidad),0) FROM Tbl_Stock WHERE id_producto=@prod),'Compra No. '+CAST(@compra AS VARCHAR(20)))", cn, tx))
                            {
                                cmd.Parameters.Add("@prod", SqlDbType.Int).Value = d.IdProducto;
                                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                                cmd.Parameters.Add("@cant", SqlDbType.Int).Value = Decimal.ToInt32(d.Cantidad);
                                cmd.Parameters.Add("@compra", SqlDbType.Int).Value = idCompra;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tx.Commit(); return idCompra;
                    }
                    catch { tx.Rollback(); throw; }
                }
            }
        }

        // EDITAR: EditarCompra
        public void EditarCompra(int idCompra, int proveedor, int sucursal, DateTime fecha, IList<CompraLineaCatalogo> detalles)
        {
            if (detalles == null || detalles.Count == 0) throw new InvalidOperationException("Agrega al menos un producto a la compra.");
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                // Agrupa los cambios relacionados para confirmarlos juntos o revertirlos si ocurre un error.
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        int sucursalAnterior;
                        using (SqlCommand cmd = new SqlCommand("SELECT id_sucursal FROM Tbl_Compras WITH (UPDLOCK) WHERE id_compra=@id", cn, tx))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCompra;
                            object valor = cmd.ExecuteScalar();
                            if (valor == null) throw new InvalidOperationException("La compra ya no existe.");
                            if (valor == DBNull.Value) throw new InvalidOperationException("Esta compra histórica no tiene sucursal asignada. Asígnala antes de editarla.");
                            sucursalAnterior = Convert.ToInt32(valor);
                        }

                        DataTable anteriores;
                        using (SqlCommand cmd = new SqlCommand("SELECT id_producto, cantidad FROM Tbl_DetalleDeCompras WITH (UPDLOCK) WHERE id_compra=@id", cn, tx))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCompra;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { anteriores = new DataTable(); da.Fill(anteriores); }
                        }

                        foreach (DataRow anterior in anteriores.Rows)
                        {
                            int producto = Convert.ToInt32(anterior["id_producto"]);
                            int cantidad = Convert.ToInt32(anterior["cantidad"]);
                            using (SqlCommand cmd = new SqlCommand("UPDATE Tbl_Stock WITH (UPDLOCK) SET cantidad=cantidad-@cant WHERE id_sucursal=@suc AND id_producto=@prod AND cantidad>=@cant", cn, tx))
                            {
                                cmd.Parameters.Add("@cant", SqlDbType.Int).Value = cantidad;
                                cmd.Parameters.Add("@suc", SqlDbType.Int).Value = sucursalAnterior;
                                cmd.Parameters.Add("@prod", SqlDbType.Int).Value = producto;
                                if (cmd.ExecuteNonQuery() != 1) throw new InvalidOperationException("No se puede revertir la compra: el stock cambió o parte de esos productos ya fue consumida.");
                            }
                            RegistrarKardexCompra(cn, tx, producto, DateTime.Now, "Salida", cantidad, "Reversión por edición de compra No. " + idCompra);
                        }

                        decimal total = 0;
                        foreach (CompraLineaCatalogo detalle in detalles) total += detalle.Subtotal;
                        using (SqlCommand cmd = new SqlCommand("UPDATE Tbl_Compras SET id_proveedor=@prov,id_sucursal=@suc,fecha=@fecha,total=@total WHERE id_compra=@id", cn, tx))
                        {
                            cmd.Parameters.Add("@prov", SqlDbType.Int).Value = proveedor;
                            cmd.Parameters.Add("@suc", SqlDbType.Int).Value = sucursal;
                            cmd.Parameters.Add("@fecha", SqlDbType.Date).Value = fecha.Date;
                            cmd.Parameters.Add("@total", SqlDbType.Decimal).Value = total;
                            cmd.Parameters["@total"].Precision = 12; cmd.Parameters["@total"].Scale = 2;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCompra;
                            if (cmd.ExecuteNonQuery() != 1) throw new InvalidOperationException("No se pudo actualizar la compra.");
                        }
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Tbl_DetalleDeCompras WHERE id_compra=@id", cn, tx))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCompra;
                            cmd.ExecuteNonQuery();
                        }
                        foreach (CompraLineaCatalogo detalle in detalles)
                        {
                            int cantidad = Decimal.ToInt32(detalle.Cantidad);
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO Tbl_DetalleDeCompras(id_compra,id_producto,cantidad,precio_unitario) VALUES(@id,@prod,@cant,@costo)", cn, tx))
                            {
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCompra;
                                cmd.Parameters.Add("@prod", SqlDbType.Int).Value = detalle.IdProducto;
                                cmd.Parameters.Add("@cant", SqlDbType.Int).Value = cantidad;
                                cmd.Parameters.Add("@costo", SqlDbType.Decimal).Value = detalle.CostoUnitario;
                                cmd.Parameters["@costo"].Precision = 10; cmd.Parameters["@costo"].Scale = 2;
                                cmd.ExecuteNonQuery();
                            }
                            using (SqlCommand cmd = new SqlCommand("UPDATE Tbl_Stock WITH (UPDLOCK) SET cantidad=cantidad+@cant WHERE id_sucursal=@suc AND id_producto=@prod; IF @@ROWCOUNT=0 INSERT INTO Tbl_Stock(id_sucursal,id_producto,cantidad,stock_minimo) VALUES(@suc,@prod,@cant,0);", cn, tx))
                            {
                                cmd.Parameters.Add("@cant", SqlDbType.Int).Value = cantidad;
                                cmd.Parameters.Add("@suc", SqlDbType.Int).Value = sucursal;
                                cmd.Parameters.Add("@prod", SqlDbType.Int).Value = detalle.IdProducto;
                                cmd.ExecuteNonQuery();
                            }
                            RegistrarKardexCompra(cn, tx, detalle.IdProducto, fecha, "Entrada", cantidad, "Compra No. " + idCompra + " actualizada");
                        }
                        tx.Commit();
                    }
                    catch { tx.Rollback(); throw; }
                }
            }
        }

        // ELIMINAR: EliminarCompra
        public void EliminarCompra(int idCompra)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                // Agrupa los cambios relacionados para confirmarlos juntos o revertirlos si ocurre un error.
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        int sucursal;
                        using (SqlCommand cmd = new SqlCommand("SELECT id_sucursal FROM Tbl_Compras WITH (UPDLOCK) WHERE id_compra=@id", cn, tx))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCompra;
                            object valor = cmd.ExecuteScalar();
                            if (valor == null) throw new InvalidOperationException("La compra ya no existe.");
                            if (valor == DBNull.Value) throw new InvalidOperationException("Esta compra histórica no tiene sucursal asignada. Asigna la sucursal correcta antes de eliminarla.");
                            sucursal = Convert.ToInt32(valor);
                        }

                        DataTable detalles;
                        using (SqlCommand cmd = new SqlCommand("SELECT id_producto, cantidad FROM Tbl_DetalleDeCompras WITH (UPDLOCK) WHERE id_compra=@id", cn, tx))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCompra;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { detalles = new DataTable(); da.Fill(detalles); }
                        }
                        foreach (DataRow detalle in detalles.Rows)
                        {
                            int producto = Convert.ToInt32(detalle["id_producto"]);
                            int cantidad = Convert.ToInt32(detalle["cantidad"]);
                            using (SqlCommand cmd = new SqlCommand("UPDATE Tbl_Stock WITH (UPDLOCK) SET cantidad=cantidad-@cant WHERE id_sucursal=@suc AND id_producto=@prod AND cantidad>=@cant", cn, tx))
                            {
                                cmd.Parameters.Add("@cant", SqlDbType.Int).Value = cantidad;
                                cmd.Parameters.Add("@suc", SqlDbType.Int).Value = sucursal;
                                cmd.Parameters.Add("@prod", SqlDbType.Int).Value = producto;
                                if (cmd.ExecuteNonQuery() != 1) throw new InvalidOperationException("No se puede eliminar la compra: el stock cambió o parte de esos productos ya fue consumida.");
                            }
                            RegistrarKardexCompra(cn, tx, producto, DateTime.Now, "Salida", cantidad, "Reversión por eliminación de compra No. " + idCompra);
                        }

                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Tbl_DetalleDeCompras WHERE id_compra=@id", cn, tx))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCompra;
                            cmd.ExecuteNonQuery();
                        }
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Tbl_Compras WHERE id_compra=@id", cn, tx))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCompra;
                            if (cmd.ExecuteNonQuery() != 1) throw new InvalidOperationException("No se pudo eliminar la compra.");
                        }
                        tx.Commit();
                    }
                    catch { tx.Rollback(); throw; }
                }
            }
        }

        // GUARDAR: RegistrarKardexCompra
        private static void RegistrarKardexCompra(SqlConnection cn, SqlTransaction tx, int producto, DateTime fecha, string tipo, int cantidad, string descripcion)
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Tbl_Kardex(id_producto,fecha,tipo_movimiento,cantidad,saldo,descripcion) VALUES(@prod,@fecha,@tipo,@cant,(SELECT ISNULL(SUM(cantidad),0) FROM Tbl_Stock WHERE id_producto=@prod),@descripcion)", cn, tx))
            {
                cmd.Parameters.Add("@prod", SqlDbType.Int).Value = producto;
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                cmd.Parameters.Add("@tipo", SqlDbType.VarChar, 20).Value = tipo;
                cmd.Parameters.Add("@cant", SqlDbType.Int).Value = cantidad;
                cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 150).Value = descripcion;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
