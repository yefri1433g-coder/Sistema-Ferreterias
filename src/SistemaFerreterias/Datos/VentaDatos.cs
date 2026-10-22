using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Entidad;

namespace Datos
{
    public class VentaDatos
    {
        private readonly ConexionDatos _conexion = new ConexionDatos();

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

        // Guarda TODO en una transacción: venta, detalle, stock, kardex y pago.
        // Si algo falla, no se guarda nada.
        public int Guardar(Venta v)
        {
            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();

                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        // 1) Venta
                        int idVenta;
                        using (var cmd = new SqlCommand(
                            "INSERT INTO Tbl_Ventas (id_cliente, fecha, total) VALUES (@c, @f, @t); " +
                            "SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@c", v.IdCliente);
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

                        // 5) Pago
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
    }
}
