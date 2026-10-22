using System;
using System.Data;
using System.Data.SqlClient;
using Entidad;

namespace Datos
{
    public class PagoDatos
    {
        public DataTable MtdListarPagosRecientes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new ConexionDatos().MtdObtenerConexionBDD())
            {
                string sql = @"SELECT p.id_pago, v.id_venta, c.nombre AS cliente,
                                      p.metodo_pago, p.fecha, p.monto
                               FROM Tbl_Pagos p
                               INNER JOIN Tbl_Ventas v ON v.id_venta = p.id_venta
                               INNER JOIN Tbl_Clientes c ON c.id_cliente = v.id_cliente
                               ORDER BY p.fecha DESC, p.id_pago DESC";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn)) da.Fill(dt);
            }
            return dt;
        }

        // Ventas con cliente, total y lo que ya se ha pagado de cada una
        public DataTable MtdListarVentas()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new ConexionDatos().MtdObtenerConexionBDD())
            {
                string sql = @"SELECT v.id_venta,
                                      c.nombre AS cliente,
                                      v.fecha,
                                      v.total,
                                      ISNULL(SUM(p.monto), 0) AS pagado
                               FROM Tbl_Ventas v
                               INNER JOIN Tbl_Clientes c ON c.id_cliente = v.id_cliente
                               LEFT JOIN Tbl_Pagos p ON p.id_venta = v.id_venta
                               GROUP BY v.id_venta, c.nombre, v.fecha, v.total
                               ORDER BY v.id_venta DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        // Pagos ya registrados de una venta
        public DataTable MtdListarPagosPorVenta(int idVenta)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new ConexionDatos().MtdObtenerConexionBDD())
            {
                string sql = @"SELECT id_pago AS [No. Pago],
                                      fecha AS [Fecha],
                                      monto AS [Monto],
                                      metodo_pago AS [Método de pago]
                               FROM Tbl_Pagos
                               WHERE id_venta = @id
                               ORDER BY fecha DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", idVenta);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        // Saldo pendiente de una venta (total - pagos)
        public decimal MtdObtenerSaldo(int idVenta)
        {
            using(SqlConnection cn = new ConexionDatos().MtdObtenerConexionBDD())
            {
                string sql = @"SELECT v.total - ISNULL((SELECT SUM(p.monto)
                                                        FROM Tbl_Pagos p
                                                        WHERE p.id_venta = v.id_venta), 0)
                               FROM Tbl_Ventas v
                               WHERE v.id_venta = @id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", idVenta);
                    if (cn.State != ConnectionState.Open) cn.Open();

                    object r = cmd.ExecuteScalar();
                    return (r == null || r == DBNull.Value) ? 0m : Convert.ToDecimal(r);
                }
            }
        }

        public bool MtdInsertar(Pago pago)
        {
            using (SqlConnection cn = new ConexionDatos().MtdObtenerConexionBDD())
            {
                string sql = @"INSERT INTO Tbl_Pagos (id_venta, fecha, monto, metodo_pago)
                               VALUES (@venta, @fecha, @monto, @metodo)";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@venta", pago.IdVenta);
                    cmd.Parameters.AddWithValue("@fecha", pago.Fecha);
                    cmd.Parameters.AddWithValue("@monto", pago.Monto);
                    cmd.Parameters.AddWithValue("@metodo", pago.MetodoPago);
                    if (cn.State != ConnectionState.Open) cn.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
