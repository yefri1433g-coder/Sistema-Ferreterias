using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entidad;

namespace Datos
{
    // CLASE VentaDatos: Realiza consultas y operaciones de persistencia en SQL Server.
    public class VentaDatos
    {
        private readonly ConexionDatos _conexion = new ConexionDatos();

        // CONSULTAR: ListarVentas
        public System.Data.DataTable ListarVentas()
        {
            System.Data.DataTable tabla = new System.Data.DataTable();
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT v.id_venta, c.nombre AS cliente, ISNULL(s.nombre, 'Sucursal no registrada') AS sucursal, " +
                    "v.id_sucursal, v.fecha, v.total, ISNULL(SUM(p.monto), 0) AS pagado " +
                    "FROM Tbl_Ventas v INNER JOIN Tbl_Clientes c ON c.id_cliente = v.id_cliente " +
                    "LEFT JOIN Tbl_Sucursales s ON s.id_sucursal = v.id_sucursal " +
                    "LEFT JOIN Tbl_Pagos p ON p.id_venta = v.id_venta " +
                    "GROUP BY v.id_venta, c.nombre, s.nombre, v.id_sucursal, v.fecha, v.total ORDER BY v.id_venta DESC", cn))
                    da.Fill(tabla);
                return tabla;
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("No se pudo consultar el historial de ventas.", ex);
            }
        }

        // CONSULTAR: ListarSucursales
        public List<ItemCombo> ListarSucursales()
        {
            List<ItemCombo> lista = new List<ItemCombo>();

            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "SELECT id_sucursal, nombre FROM Tbl_Sucursales WHERE estado = 1 ORDER BY nombre", cn);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ItemCombo
                        {
                            Id = dr.GetInt32(0),
                            Texto = dr.GetString(1)
                        });
                    }
                }
            }
            return lista;
        }

        // Muestra el catálogo activo y el stock de la sucursal, incluso cuando sea cero.
        // CONSULTAR: ListarProductos
        public List<ProductoVenta> ListarProductos(int idSucursal)
        {
            List<ProductoVenta> lista = new List<ProductoVenta>();

            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "SELECT p.id_producto, p.codigo, p.nombre, p.precio_venta, ISNULL(s.Existencia, 0) " +
                    "FROM dbo.Tbl_Productos p " +
                    "LEFT JOIN (SELECT id_producto, SUM(cantidad) AS Existencia " +
                    "           FROM dbo.Tbl_Stock WHERE id_sucursal = @s GROUP BY id_producto) s " +
                    "       ON s.id_producto = p.id_producto " +
                    "WHERE p.estado = 1 " +
                    "ORDER BY p.nombre", cn);
                cmd.Parameters.Add("@s", System.Data.SqlDbType.Int).Value = idSucursal;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ProductoVenta
                        {
                            IdProducto = dr.GetInt32(0),
                            Codigo = dr.GetString(1),
                            Nombre = dr.GetString(2),
                            PrecioVenta = dr.GetDecimal(3),
                            Existencia = dr.GetInt32(4)
                        });
                    }
                }
            }
            return lista;
        }

        // Guarda la venta, sus detalles, el stock y el kardex en una transacción.
        // Si el método es Pendiente, omite el pago para que Pagos calcule el saldo.
        // Si algo falla, no se guarda nada.
        // GUARDAR: Guardar
        public int Guardar(Venta v)
        {
            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();

                // Agrupa los cambios relacionados para confirmarlos juntos o revertirlos si ocurre un error.
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        // 1) Venta
                        int idVenta;
                        using (var cmd = new SqlCommand(
                            "INSERT INTO Tbl_Ventas (id_cliente, id_sucursal, fecha, total) VALUES (@c, @s, @f, @t); " +
                            "SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@c", v.IdCliente);
                            cmd.Parameters.AddWithValue("@s", v.IdSucursal);
                            cmd.Parameters.AddWithValue("@f", v.Fecha);
                            cmd.Parameters.AddWithValue("@t", v.Total);
                            idVenta = (int)cmd.ExecuteScalar();
                        }

                        foreach (DetalleVenta d in v.Detalles)
                        {
                            // 2) Detalle de la venta
                            using (var cmd = new SqlCommand(
                                "INSERT INTO Tbl_DetallesVentas (id_venta, id_producto, cantidad, precio_unitario) " +
                                "VALUES (@v, @p, @c, @pu)", cn, tx))
                            {
                                cmd.Parameters.AddWithValue("@v", idVenta);
                                cmd.Parameters.AddWithValue("@p", d.IdProducto);
                                cmd.Parameters.AddWithValue("@c", d.Cantidad);
                                cmd.Parameters.AddWithValue("@pu", d.PrecioUnitario);
                                cmd.ExecuteNonQuery();
                            }

                            // 3) Descontar stock (solo si alcanza)
                            int filas;
                            using (var cmd = new SqlCommand(
                                "UPDATE Tbl_Stock SET cantidad = cantidad - @c " +
                                "WHERE id_sucursal = @s AND id_producto = @p AND cantidad >= @c", cn, tx))
                            {
                                cmd.Parameters.AddWithValue("@c", d.Cantidad);
                                cmd.Parameters.AddWithValue("@s", v.IdSucursal);
                                cmd.Parameters.AddWithValue("@p", d.IdProducto);
                                filas = cmd.ExecuteNonQuery();
                            }

                            if (filas == 0)
                                throw new InvalidOperationException(
                                    "Stock insuficiente de \"" + d.Producto + "\" en la sucursal seleccionada.");

                            // 4) Kardex: salida con el saldo total del producto
                            int saldo;
                            using (var cmd = new SqlCommand(
                                "SELECT ISNULL(SUM(cantidad), 0) FROM Tbl_Stock WHERE id_producto = @p", cn, tx))
                            {
                                cmd.Parameters.AddWithValue("@p", d.IdProducto);
                                saldo = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            using (var cmd = new SqlCommand(
                                "INSERT INTO Tbl_Kardex (id_producto, fecha, tipo_movimiento, cantidad, saldo, descripcion) " +
                                "VALUES (@p, @f, 'Salida', @c, @saldo, @desc)", cn, tx))
                            {
                                cmd.Parameters.AddWithValue("@p", d.IdProducto);
                                cmd.Parameters.AddWithValue("@f", v.Fecha);
                                cmd.Parameters.AddWithValue("@c", d.Cantidad);
                                cmd.Parameters.AddWithValue("@saldo", saldo);
                                cmd.Parameters.AddWithValue("@desc", "Venta No. " + idVenta);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Registra el pago completo solo cuando se eligió un método de pago real.
                        // La opción Pendiente deja la venta sin pagos en Tbl_Pagos.
                        if (!string.Equals(v.MetodoPago, "Pendiente", StringComparison.OrdinalIgnoreCase))
                        {
                            using (var cmd = new SqlCommand(
                                "INSERT INTO Tbl_Pagos (id_venta, fecha, monto, metodo_pago) " +
                                "VALUES (@v, @f, @m, @mp)", cn, tx))
                            {
                                cmd.Parameters.AddWithValue("@v", idVenta);
                                cmd.Parameters.AddWithValue("@f", v.Fecha);
                                cmd.Parameters.AddWithValue("@m", v.Total);
                                cmd.Parameters.AddWithValue("@mp", v.MetodoPago);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        return idVenta;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        // CONSULTAR: Obtener
        public Venta Obtener(int idVenta)
        {
            return Consultar(idVenta);
        }

        // CONSULTAR: Consultar
        public Venta Consultar(int idVenta)
        {
            Venta venta = null;
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT v.id_venta, v.id_cliente, c.nombre, v.id_sucursal, v.fecha, v.total " +
                        "FROM Tbl_Ventas v INNER JOIN Tbl_Clientes c ON c.id_cliente = v.id_cliente " +
                        "WHERE v.id_venta = @id", cn))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = idVenta;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                venta = new Venta
                                {
                                    IdVenta = dr.GetInt32(0),
                                    IdCliente = dr.GetInt32(1),
                                    NombreCliente = dr.GetString(2),
                                    IdSucursal = dr.IsDBNull(3) ? 0 : dr.GetInt32(3),
                                    Fecha = dr.GetDateTime(4),
                                    Total = dr.GetDecimal(5),
                                    MetodoPago = "Pendiente"
                                };
                            }
                        }
                    }

                    if (venta == null) return null;
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT d.id_producto, p.codigo, p.nombre, d.cantidad, d.precio_unitario " +
                        "FROM Tbl_DetallesVentas d INNER JOIN Tbl_Productos p ON p.id_producto = d.id_producto " +
                        "WHERE d.id_venta = @id ORDER BY d.id_detalle_venta", cn))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = idVenta;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                venta.Detalles.Add(new DetalleVenta
                                {
                                    IdProducto = dr.GetInt32(0),
                                    Codigo = dr.GetString(1),
                                    Producto = dr.GetString(2),
                                    Cantidad = dr.GetInt32(3),
                                    PrecioUnitario = dr.GetDecimal(4)
                                });
                            }
                        }
                    }
                }
                return venta;
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("No se pudo consultar la venta seleccionada.", ex);
            }
        }

        // EDITAR: ActualizarPendiente
        public bool ActualizarPendiente(Venta venta)
        {
            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                // Agrupa los cambios relacionados para confirmarlos juntos o revertirlos si ocurre un error.
                using (SqlTransaction tx = cn.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        VerificarPendiente(cn, tx, venta.IdVenta);
                        VerificarSucursalVenta(cn, tx, venta.IdVenta, venta.IdSucursal);
                        List<DetalleVenta> anteriores = ListarDetalles(cn, tx, venta.IdVenta);

                        using (SqlCommand cmd = new SqlCommand(
                            "UPDATE Tbl_Ventas SET id_cliente = @cliente, id_sucursal = @sucursal, total = @total WHERE id_venta = @id", cn, tx))
                        {
                            cmd.Parameters.Add("@cliente", SqlDbType.Int).Value = venta.IdCliente;
                            cmd.Parameters.Add("@sucursal", SqlDbType.Int).Value = venta.IdSucursal;
                            SqlParameter total = cmd.Parameters.Add("@total", SqlDbType.Decimal);
                            total.Precision = 12; total.Scale = 2; total.Value = venta.Total;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = venta.IdVenta;
                            if (cmd.ExecuteNonQuery() == 0) throw new InvalidOperationException("No se encontró la venta.");
                        }

                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Tbl_DetallesVentas WHERE id_venta = @id", cn, tx))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = venta.IdVenta;
                            cmd.ExecuteNonQuery();
                        }

                        foreach (DetalleVenta detalle in venta.Detalles)
                            InsertarDetalle(cn, tx, venta.IdVenta, detalle);

                        AplicarDiferenciasStock(cn, tx, venta, anteriores);
                        tx.Commit();
                        return true;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        // ELIMINAR: EliminarPendiente
        public bool EliminarPendiente(int idVenta, int idSucursal)
        {
            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                // Agrupa los cambios relacionados para confirmarlos juntos o revertirlos si ocurre un error.
                using (SqlTransaction tx = cn.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        VerificarPendiente(cn, tx, idVenta);
                        int sucursalRegistrada = ObtenerSucursalVenta(cn, tx, idVenta);
                        if (sucursalRegistrada > 0 && sucursalRegistrada != idSucursal)
                            throw new InvalidOperationException("La sucursal seleccionada no coincide con la sucursal registrada en la venta.");
                        int sucursalInventario = sucursalRegistrada > 0 ? sucursalRegistrada : idSucursal;
                        if (sucursalInventario <= 0)
                            throw new InvalidOperationException("Selecciona la sucursal original de la venta.");
                        List<DetalleVenta> detalles = ListarDetalles(cn, tx, idVenta);
                        foreach (DetalleVenta detalle in detalles)
                            CambiarStock(cn, tx, idVenta, sucursalInventario, detalle.IdProducto, detalle.Cantidad, false, detalle.Producto);

                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Tbl_DetallesVentas WHERE id_venta = @id", cn, tx))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idVenta;
                            cmd.ExecuteNonQuery();
                        }
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Tbl_Ventas WHERE id_venta = @id", cn, tx))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idVenta;
                            if (cmd.ExecuteNonQuery() == 0) throw new InvalidOperationException("No se encontró la venta.");
                        }
                        tx.Commit();
                        return true;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        // VALIDAR: VerificarPendiente
        private static void VerificarPendiente(SqlConnection cn, SqlTransaction tx, int idVenta)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Tbl_Ventas v WITH (UPDLOCK, HOLDLOCK) " +
                "WHERE v.id_venta = @id AND NOT EXISTS (SELECT 1 FROM Tbl_Pagos p WITH (UPDLOCK, HOLDLOCK) WHERE p.id_venta = v.id_venta)", cn, tx))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idVenta;
                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                    throw new InvalidOperationException("Solo se pueden editar o eliminar ventas pendientes que existan.");
            }
        }

        // VALIDAR: VerificarSucursalVenta
        private static void VerificarSucursalVenta(SqlConnection cn, SqlTransaction tx, int idVenta, int idSucursal)
        {
            int sucursalRegistrada = ObtenerSucursalVenta(cn, tx, idVenta);
            if (sucursalRegistrada > 0 && sucursalRegistrada != idSucursal)
                throw new InvalidOperationException("No se puede cambiar la sucursal original de una venta.");
        }

        // CONSULTAR: ObtenerSucursalVenta
        private static int ObtenerSucursalVenta(SqlConnection cn, SqlTransaction tx, int idVenta)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT id_sucursal FROM Tbl_Ventas WITH (UPDLOCK, HOLDLOCK) WHERE id_venta = @id", cn, tx))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idVenta;
                object valor = cmd.ExecuteScalar();
                return valor == null || valor == DBNull.Value ? 0 : Convert.ToInt32(valor);
            }
        }

        // CONSULTAR: ListarDetalles
        private static List<DetalleVenta> ListarDetalles(SqlConnection cn, SqlTransaction tx, int idVenta)
        {
            List<DetalleVenta> detalles = new List<DetalleVenta>();
            using (SqlCommand cmd = new SqlCommand(
                "SELECT d.id_producto, p.codigo, p.nombre, d.cantidad, d.precio_unitario " +
                "FROM Tbl_DetallesVentas d INNER JOIN Tbl_Productos p ON p.id_producto = d.id_producto " +
                "WHERE d.id_venta = @id ORDER BY d.id_detalle_venta", cn, tx))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idVenta;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        detalles.Add(new DetalleVenta { IdProducto = dr.GetInt32(0), Codigo = dr.GetString(1), Producto = dr.GetString(2), Cantidad = dr.GetInt32(3), PrecioUnitario = dr.GetDecimal(4) });
                }
            }
            return detalles;
        }

        // AGREGAR: InsertarDetalle
        private static void InsertarDetalle(SqlConnection cn, SqlTransaction tx, int idVenta, DetalleVenta detalle)
        {
            using (SqlCommand cmd = new SqlCommand(
                "INSERT INTO Tbl_DetallesVentas (id_venta, id_producto, cantidad, precio_unitario) VALUES (@venta, @producto, @cantidad, @precio)", cn, tx))
            {
                cmd.Parameters.Add("@venta", SqlDbType.Int).Value = idVenta;
                cmd.Parameters.Add("@producto", SqlDbType.Int).Value = detalle.IdProducto;
                cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.Cantidad;
                SqlParameter precio = cmd.Parameters.Add("@precio", SqlDbType.Decimal);
                precio.Precision = 10; precio.Scale = 2; precio.Value = detalle.PrecioUnitario;
                cmd.ExecuteNonQuery();
            }
        }

        // CONFIGURACIÓN: AplicarDiferenciasStock
        private static void AplicarDiferenciasStock(SqlConnection cn, SqlTransaction tx, Venta venta, List<DetalleVenta> anteriores)
        {
            Dictionary<int, DetalleVenta> nuevosPorProducto = new Dictionary<int, DetalleVenta>();
            foreach (DetalleVenta detalle in venta.Detalles)
                nuevosPorProducto.Add(detalle.IdProducto, detalle);

            foreach (DetalleVenta anterior in anteriores)
            {
                DetalleVenta nuevo;
                int cantidadNueva = nuevosPorProducto.TryGetValue(anterior.IdProducto, out nuevo) ? nuevo.Cantidad : 0;
                int diferencia = anterior.Cantidad - cantidadNueva;
                if (diferencia > 0)
                    CambiarStock(cn, tx, venta.IdVenta, venta.IdSucursal, anterior.IdProducto, diferencia, false, anterior.Producto);
                else if (diferencia < 0)
                    CambiarStock(cn, tx, venta.IdVenta, venta.IdSucursal, anterior.IdProducto, -diferencia, true, anterior.Producto);
                if (nuevo != null) nuevosPorProducto.Remove(anterior.IdProducto);
            }

            foreach (DetalleVenta nuevo in nuevosPorProducto.Values)
                CambiarStock(cn, tx, venta.IdVenta, venta.IdSucursal, nuevo.IdProducto, nuevo.Cantidad, true, nuevo.Producto);
        }

        // aumentar=false restaura existencias; aumentar=true descuenta la venta editada.
        private static void CambiarStock(SqlConnection cn, SqlTransaction tx, int idVenta, int idSucursal, int idProducto, int cantidad, bool aumentar, string nombreProducto)
        {
            string operador = aumentar ? "-" : "+";
            string condicion = aumentar ? " AND cantidad >= @cantidad" : string.Empty;
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Tbl_Stock SET cantidad = cantidad " + operador + " @cantidad WHERE id_sucursal = @sucursal AND id_producto = @producto" + condicion, cn, tx))
            {
                cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = cantidad;
                cmd.Parameters.Add("@sucursal", SqlDbType.Int).Value = idSucursal;
                cmd.Parameters.Add("@producto", SqlDbType.Int).Value = idProducto;
                if (cmd.ExecuteNonQuery() == 0)
                {
                    string causa = aumentar
                        ? "No hay un registro de inventario con cantidad suficiente"
                        : "No existe un registro de inventario para ese producto y sucursal";
                    throw new InvalidOperationException(causa + " de \"" + nombreProducto + "\". Revisa el inventario de la sucursal seleccionada.");
                }
            }

            int saldo;
            using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(cantidad), 0) FROM Tbl_Stock WHERE id_producto = @producto", cn, tx))
            {
                cmd.Parameters.Add("@producto", SqlDbType.Int).Value = idProducto;
                saldo = Convert.ToInt32(cmd.ExecuteScalar());
            }
            using (SqlCommand cmd = new SqlCommand(
                "INSERT INTO Tbl_Kardex (id_producto, fecha, tipo_movimiento, cantidad, saldo, descripcion) VALUES (@producto, @fecha, @tipo, @cantidad, @saldo, @descripcion)", cn, tx))
            {
                cmd.Parameters.Add("@producto", SqlDbType.Int).Value = idProducto;
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@tipo", SqlDbType.VarChar, 20).Value = aumentar ? "Salida" : "Entrada";
                cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = cantidad;
                cmd.Parameters.Add("@saldo", SqlDbType.Int).Value = saldo;
                cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 150).Value = "Venta No. " + idVenta + (aumentar ? " editada" : " revertida");
                cmd.ExecuteNonQuery();
            }
        }
    }
}
