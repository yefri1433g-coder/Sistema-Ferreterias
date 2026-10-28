using Entidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    // CLASE DashboardDatos: Realiza consultas y operaciones de persistencia en SQL Server.
    public class DashboardDatos
    {
        private readonly ConexionDatos _conexionDatos = new ConexionDatos();

        private int MtdEscalar(SqlConnection conn, string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private List<KeyValuePair<string, int>> MtdPares(SqlConnection conn, string query)
        {
            List<KeyValuePair<string, int>> lista = new List<KeyValuePair<string, int>>();

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        lista.Add(new KeyValuePair<string, int>(rd[0].ToString(), Convert.ToInt32(rd[1])));
                    }
                }
            }

            return lista;
        }

        private List<KeyValuePair<string, decimal>> MtdVentasSemana(SqlConnection conn)
        {
            Dictionary<DateTime, decimal> importes = new Dictionary<DateTime, decimal>();
            DateTime hoy = DateTime.Today;
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT CAST(fecha AS date), SUM(total)
                FROM Tbl_Ventas
                WHERE fecha >= @Desde AND fecha < @Hasta
                GROUP BY CAST(fecha AS date);", conn))
            {
                cmd.Parameters.AddWithValue("@Desde", hoy.AddDays(-6));
                cmd.Parameters.AddWithValue("@Hasta", hoy.AddDays(1));
                using (SqlDataReader rd = cmd.ExecuteReader())
                    while (rd.Read()) importes[Convert.ToDateTime(rd[0]).Date] = Convert.ToDecimal(rd[1]);
            }

            List<KeyValuePair<string, decimal>> semana = new List<KeyValuePair<string, decimal>>();
            for (int dia = 6; dia >= 0; dia--)
            {
                DateTime fecha = hoy.AddDays(-dia);
                decimal total;
                importes.TryGetValue(fecha, out total);
                semana.Add(new KeyValuePair<string, decimal>(fecha.ToString("ddd"), total));
            }
            return semana;
        }

        private List<ActividadDashboard> MtdActividad(SqlConnection conn)
        {
            List<ActividadDashboard> lista = new List<ActividadDashboard>();
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT TOP 4 Tipo, Descripcion, Referencia, Fecha
                FROM (
                    SELECT N'Venta registrada' AS Tipo,
                           N'Venta No. ' + CONVERT(nvarchar(20), v.id_venta) AS Descripcion,
                           ISNULL(c.nombre, N'Cliente') AS Referencia, v.fecha AS Fecha
                    FROM Tbl_Ventas v LEFT JOIN Tbl_Clientes c ON c.id_cliente = v.id_cliente
                    UNION ALL
                    SELECT N'Compra recibida', N'Compra No. ' + CONVERT(nvarchar(20), co.id_compra),
                           ISNULL(p.nombre, N'Proveedor'), CAST(co.fecha AS datetime)
                    FROM Tbl_Compras co LEFT JOIN Tbl_Proveedores p ON p.id_proveedor = co.id_proveedor
                ) AS movimientos
                ORDER BY Fecha DESC;", conn))
            using (SqlDataReader rd = cmd.ExecuteReader())
                while (rd.Read()) lista.Add(new ActividadDashboard
                {
                    Tipo = rd.GetString(0), Descripcion = rd.GetString(1),
                    Referencia = rd.GetString(2), Fecha = rd.GetDateTime(3)
                });
            return lista;
        }

        private void MtdConsultaOpcional(Action consulta, List<string> errores, string nombre)
        {
            try { consulta(); }
            catch (Exception ex) { errores.Add(nombre + ": " + ex.Message); }
        }

        public DashboardEntidad MtdResumen()
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    conn.Open();

                    DashboardEntidad resumen = new DashboardEntidad
                    (
                        MtdEscalar(conn, "SELECT COUNT(*) FROM Tbl_Productos WHERE Estado = 1;"),
                        MtdEscalar(conn, "SELECT COUNT(*) FROM Tbl_Sucursales WHERE Estado = 1;"),
                        MtdEscalar(conn, "SELECT COUNT(*) FROM Tbl_Proveedores;"),
                        MtdEscalar(conn, "SELECT COUNT(*) FROM Tbl_Stock WHERE cantidad <= stock_minimo;"),
                        MtdEscalar(conn, "SELECT COUNT(*) FROM Tbl_Traslados WHERE estado = 1;")
                    );

                    MtdConsultaOpcional(() =>
                    {
                        using (SqlCommand ventasHoy = new SqlCommand(
                            "SELECT ISNULL(SUM(total), 0) FROM Tbl_Ventas WHERE fecha >= @Inicio AND fecha < @Fin;", conn))
                        {
                            ventasHoy.Parameters.AddWithValue("@Inicio", DateTime.Today);
                            ventasHoy.Parameters.AddWithValue("@Fin", DateTime.Today.AddDays(1));
                            resumen.VentasHoy = Convert.ToDecimal(ventasHoy.ExecuteScalar());
                        }
                    }, resumen.ErroresConsulta, "ventas de hoy");
                    MtdConsultaOpcional(() => resumen.TotalClientes = MtdEscalar(conn, "SELECT COUNT(*) FROM Tbl_Clientes;"), resumen.ErroresConsulta, "clientes");
                    MtdConsultaOpcional(() => resumen.VentasUltimosSieteDias = MtdVentasSemana(conn), resumen.ErroresConsulta, "ventas de la semana");
                    MtdConsultaOpcional(() => resumen.EstadoInventario = MtdPares(conn, @"
                        SELECT CASE WHEN cantidad <= stock_minimo THEN 'Stock bajo' ELSE 'Disponible' END, COUNT(*)
                        FROM Tbl_Stock
                        GROUP BY CASE WHEN cantidad <= stock_minimo THEN 'Stock bajo' ELSE 'Disponible' END
                        ORDER BY 1;"), resumen.ErroresConsulta, "estado del inventario");
                    MtdConsultaOpcional(() => resumen.ActividadReciente = MtdActividad(conn), resumen.ErroresConsulta, "actividad reciente");

                    resumen.StockPorSucursal = MtdPares(conn, @"
                        SELECT s.nombre, ISNULL(SUM(st.cantidad), 0)
                        FROM Tbl_Sucursales s
                        LEFT JOIN Tbl_Stock st ON st.id_sucursal = s.id_sucursal
                        GROUP BY s.id_sucursal, s.nombre
                        ORDER BY s.nombre;");

                    resumen.TrasladosPorEstado = MtdPares(conn, @"
                        SELECT CASE WHEN estado = 1 THEN 'Pendientes' ELSE 'Completados' END, COUNT(*)
                        FROM Tbl_Traslados
                        GROUP BY estado
                        ORDER BY estado;");

                    return resumen;
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "consultar el dashboard");
            }
        }
    }
}
