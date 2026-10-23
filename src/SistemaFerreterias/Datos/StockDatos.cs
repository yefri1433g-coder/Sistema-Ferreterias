using Entidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class StockDatos
    {
        private readonly ConexionDatos _conexionDatos = new ConexionDatos();

        private const string QueryBase = @"
            SELECT st.id_stock AS CodigoStock,
                   st.id_sucursal AS CodigoSucursal,
                   s.nombre AS NombreSucursal,
                   st.id_producto AS CodigoProducto,
                   p.nombre AS NombreProducto,
                   st.cantidad AS Cantidad,
                   st.stock_minimo AS StockMinimo
            FROM Tbl_Stock st
            INNER JOIN Tbl_Sucursales s ON s.id_sucursal = st.id_sucursal
            INNER JOIN Tbl_Productos p ON p.id_producto = st.id_producto ";

        // =====================================================
        // CRUD: CONSULTAR / BUSCAR
        // =====================================================

        private List<StockEntidad> MtdLeer(string where, string filtro, string accion)
        {
            List<StockEntidad> lista = new List<StockEntidad>();

            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    string QueryConsultar = QueryBase + where + " ORDER BY s.nombre, p.nombre;";

                    using (SqlCommand cmd = new SqlCommand(QueryConsultar, conn))
                    {
                        if (where.Contains("@Filtro"))
                        {
                            cmd.Parameters.Add("@Filtro", System.Data.SqlDbType.VarChar, 100).Value = filtro ?? string.Empty;
                        }

                        conn.Open();

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                StockEntidad item = new StockEntidad
                                (
                                    Convert.ToInt32(rd["CodigoStock"]),
                                    Convert.ToInt32(rd["CodigoSucursal"]),
                                    rd["NombreSucursal"].ToString(),
                                    Convert.ToInt32(rd["CodigoProducto"]),
                                    rd["NombreProducto"].ToString(),
                                    Convert.ToInt32(rd["Cantidad"]),
                                    Convert.ToInt32(rd["StockMinimo"])
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
                throw ErrorDatos.Traducir(ex, accion);
            }
        }

        public List<StockEntidad> MtdConsultar()
        {
            return MtdLeer(string.Empty, string.Empty, "consultar el stock");
        }

        public List<StockEntidad> MtdBuscar(string filtro)
        {
            return MtdLeer(
                "WHERE (CONVERT(VARCHAR(20), st.id_stock) LIKE '%' + @Filtro + '%' " +
                "OR s.nombre LIKE '%' + @Filtro + '%' " +
                "OR p.nombre LIKE '%' + @Filtro + '%' " +
                "OR CONVERT(VARCHAR(20), st.cantidad) LIKE '%' + @Filtro + '%')",
                filtro,
                "buscar en el stock");
        }

        // Productos cuya cantidad es menor o igual al minimo (lo usa el Dashboard)
        public List<StockEntidad> MtdConsultarBajo()
        {
            return MtdLeer("WHERE st.cantidad <= st.stock_minimo", string.Empty, "consultar el stock bajo");
        }

        // =====================================================
        // CRUD: AGREGAR (registra la entrada en el Kardex)
        // =====================================================

        public bool MtdAgregar(StockEntidad stock)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    conn.Open();

                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        string QueryAgregar = @"
                            INSERT INTO Tbl_Stock (id_sucursal, id_producto, cantidad, stock_minimo)
                            VALUES (@CodigoSucursal, @CodigoProducto, @Cantidad, @StockMinimo);
                        ";

                        using (SqlCommand cmd = new SqlCommand(QueryAgregar, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@CodigoSucursal", stock.CodigoSucursal);
                            cmd.Parameters.AddWithValue("@CodigoProducto", stock.CodigoProducto);
                            cmd.Parameters.AddWithValue("@Cantidad", stock.Cantidad);
                            cmd.Parameters.AddWithValue("@StockMinimo", stock.StockMinimo);
                            cmd.ExecuteNonQuery();
                        }

                        if (stock.Cantidad > 0)
                        {
                            string sucursal = KardexDatos.MtdNombreSucursal(conn, tx, stock.CodigoSucursal);
                            KardexDatos.MtdRegistrar(conn, tx, stock.CodigoProducto, "Entrada", stock.Cantidad, "Alta de stock en " + sucursal);
                        }

                        tx.Commit();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "agregar el stock");
            }
        }

        // =====================================================
        // CRUD: EDITAR (solo cantidad y minimo; registra el ajuste en el Kardex)
        // =====================================================

        public bool MtdEditar(StockEntidad stock)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    conn.Open();

                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        int codigoProducto;
                        int codigoSucursal;
                        int anterior;

                        using (SqlCommand sel = new SqlCommand("SELECT id_producto AS CodigoProducto, id_sucursal AS CodigoSucursal, cantidad AS Cantidad FROM Tbl_Stock WITH (UPDLOCK) WHERE id_stock = @CodigoStock;", conn, tx))
                        {
                            sel.Parameters.AddWithValue("@CodigoStock", stock.CodigoStock);

                            using (SqlDataReader rd = sel.ExecuteReader())
                            {
                                if (!rd.Read())
                                {
                                    throw new Exception("El registro de stock ya no existe.");
                                }

                                codigoProducto = Convert.ToInt32(rd["CodigoProducto"]);
                                codigoSucursal = Convert.ToInt32(rd["CodigoSucursal"]);
                                anterior = Convert.ToInt32(rd["Cantidad"]);
                            }
                        }

                        using (SqlCommand upd = new SqlCommand("UPDATE Tbl_Stock SET cantidad = @Cantidad, stock_minimo = @StockMinimo WHERE id_stock = @CodigoStock;", conn, tx))
                        {
                            upd.Parameters.AddWithValue("@Cantidad", stock.Cantidad);
                            upd.Parameters.AddWithValue("@StockMinimo", stock.StockMinimo);
                            upd.Parameters.AddWithValue("@CodigoStock", stock.CodigoStock);
                            upd.ExecuteNonQuery();
                        }

                        int diferencia = stock.Cantidad - anterior;
                        if (diferencia != 0)
                        {
                            string sucursal = KardexDatos.MtdNombreSucursal(conn, tx, codigoSucursal);
                            KardexDatos.MtdRegistrar(conn, tx, codigoProducto, diferencia > 0 ? "Entrada" : "Salida", Math.Abs(diferencia), "Ajuste de stock en " + sucursal);
                        }

                        tx.Commit();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "editar el stock");
            }
        }

        // =====================================================
        // CRUD: ELIMINAR (registra la salida en el Kardex)
        // =====================================================

        public bool MtdEliminar(int codigoStock)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    conn.Open();

                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        int codigoProducto;
                        int codigoSucursal;
                        int cantidad;

                        using (SqlCommand sel = new SqlCommand("SELECT id_producto AS CodigoProducto, id_sucursal AS CodigoSucursal, cantidad AS Cantidad FROM Tbl_Stock WITH (UPDLOCK) WHERE id_stock = @CodigoStock;", conn, tx))
                        {
                            sel.Parameters.AddWithValue("@CodigoStock", codigoStock);

                            using (SqlDataReader rd = sel.ExecuteReader())
                            {
                                if (!rd.Read())
                                {
                                    throw new Exception("El registro de stock ya no existe.");
                                }

                                codigoProducto = Convert.ToInt32(rd["CodigoProducto"]);
                                codigoSucursal = Convert.ToInt32(rd["CodigoSucursal"]);
                                cantidad = Convert.ToInt32(rd["Cantidad"]);
                            }
                        }

                        using (SqlCommand del = new SqlCommand("DELETE FROM Tbl_Stock WHERE id_stock = @CodigoStock;", conn, tx))
                        {
                            del.Parameters.AddWithValue("@CodigoStock", codigoStock);
                            del.ExecuteNonQuery();
                        }

                        if (cantidad > 0)
                        {
                            string sucursal = KardexDatos.MtdNombreSucursal(conn, tx, codigoSucursal);
                            KardexDatos.MtdRegistrar(conn, tx, codigoProducto, "Salida", cantidad, "Baja de stock en " + sucursal);
                        }

                        tx.Commit();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "eliminar el stock");
            }
        }
    }
}
