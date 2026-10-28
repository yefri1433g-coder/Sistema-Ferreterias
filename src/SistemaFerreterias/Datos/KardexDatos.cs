using Entidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    // CLASE KardexDatos: Realiza consultas y operaciones de persistencia en SQL Server.
    public class KardexDatos
    {
        private readonly ConexionDatos _conexionDatos = new ConexionDatos();

        // =====================================================
        // CRUD: CONSULTAR (codigoProducto = 0 trae todos)
        // =====================================================

        // CONSULTAR: MtdConsultar
        public List<KardexEntidad> MtdConsultar(int codigoProducto)
        {
            List<KardexEntidad> lista = new List<KardexEntidad>();

            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    string QueryConsultar = @"
                        SELECT k.id_kardex AS CodigoKardex, k.id_producto AS CodigoProducto,
                               p.nombre AS NombreProducto, k.fecha AS FechaMovimiento,
                               k.tipo_movimiento AS TipoMovimiento, k.cantidad AS Cantidad,
                               k.saldo AS Saldo, k.descripcion AS Descripcion
                        FROM Tbl_Kardex k
                        INNER JOIN Tbl_Productos p ON p.id_producto = k.id_producto
                        WHERE (@CodigoProducto = 0 OR k.id_producto = @CodigoProducto)
                        ORDER BY k.fecha DESC, k.id_kardex DESC;
                    ";

                    using (SqlCommand cmd = new SqlCommand(QueryConsultar, conn))
                    {
                        cmd.Parameters.AddWithValue("@CodigoProducto", codigoProducto);
                        conn.Open();

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                KardexEntidad item = new KardexEntidad
                                (
                                    Convert.ToInt32(rd["CodigoKardex"]),
                                    Convert.ToInt32(rd["CodigoProducto"]),
                                    rd["NombreProducto"].ToString(),
                                    Convert.ToDateTime(rd["FechaMovimiento"]),
                                    rd["TipoMovimiento"].ToString(),
                                    Convert.ToInt32(rd["Cantidad"]),
                                    Convert.ToInt32(rd["Saldo"]),
                                    rd["Descripcion"].ToString()
                                );

                                lista.Add(item);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "consultar el kardex");
            }
        }

        // =====================================================
        // Metodos internos: los usan Stock y Traslados dentro de su transaccion
        // El saldo es la existencia total del producto (suma de Tbl_Stock) en ese momento
        // =====================================================

        // GUARDAR: MtdRegistrar
        internal static void MtdRegistrar(SqlConnection conn, SqlTransaction tx, int codigoProducto, string tipo, int cantidad, string descripcion)
        {
            string QueryRegistrar = @"
                INSERT INTO Tbl_Kardex (id_producto, fecha, tipo_movimiento, cantidad, saldo, descripcion)
                VALUES
                (
                    @CodigoProducto,
                    GETDATE(),
                    @TipoMovimiento,
                    @Cantidad,
                    (SELECT ISNULL(SUM(cantidad), 0) FROM Tbl_Stock WHERE id_producto = @CodigoProducto),
                    @Descripcion
                );
            ";

            using (SqlCommand cmd = new SqlCommand(QueryRegistrar, conn, tx))
            {
                cmd.Parameters.AddWithValue("@CodigoProducto", codigoProducto);
                cmd.Parameters.AddWithValue("@TipoMovimiento", tipo);
                cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                cmd.ExecuteNonQuery();
            }
        }

        internal static string MtdNombreSucursal(SqlConnection conn, SqlTransaction tx, int codigoSucursal)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT nombre FROM Tbl_Sucursales WHERE id_sucursal = @Codigo;", conn, tx))
            {
                cmd.Parameters.AddWithValue("@Codigo", codigoSucursal);
                return Convert.ToString(cmd.ExecuteScalar());
            }
        }
    }
}
