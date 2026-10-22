using Entidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class InventariosDatos
    {
        // Instanciar: Que la clase actual (Datos) pueda acceder a otra clase (ConexionDatos)
        private readonly ConexionDatos _conexionDatos = new ConexionDatos();

        // =====================================================
        // CRUD: CONSULTAR / BUSCAR
        // =====================================================

        private List<InventariosEntidad> MtdLeer(string filtro)
        {
            List<InventariosEntidad> lista = new List<InventariosEntidad>();

            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    string QueryConsultar = @"
                        SELECT i.id_inventario AS CodigoInventario, i.id_sucursal AS CodigoSucursal,
                               s.nombre AS NombreSucursal, i.fecha AS FechaInventario, i.descripcion AS Descripcion
                        FROM Tbl_Inventarios i
                        INNER JOIN Tbl_Sucursales s ON s.id_sucursal = i.id_sucursal
                        -- El mismo filtro permite localizar los cuatro campos principales del inventario.
                        WHERE (@Filtro = ''
                               OR CONVERT(VARCHAR(20), i.id_inventario) LIKE '%' + @Filtro + '%'
                               OR CONVERT(VARCHAR(20), i.id_sucursal) LIKE '%' + @Filtro + '%'
                               OR s.nombre LIKE '%' + @Filtro + '%'
                               OR CONVERT(VARCHAR(10), i.fecha, 103) LIKE '%' + @Filtro + '%'
                               OR i.descripcion LIKE '%' + @Filtro + '%')
                        ORDER BY i.id_inventario DESC;
                    ";

                    using (SqlCommand cmd = new SqlCommand(QueryConsultar, conn))
                    {
                        cmd.Parameters.AddWithValue("@Filtro", filtro ?? string.Empty);
                        conn.Open();

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                InventariosEntidad item = new InventariosEntidad
                                (
                                    Convert.ToInt32(rd["CodigoInventario"]),
                                    Convert.ToInt32(rd["CodigoSucursal"]),
                                    rd["NombreSucursal"].ToString(),
                                    Convert.ToDateTime(rd["FechaInventario"]),
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
                throw ErrorDatos.Traducir(ex, "consultar los inventarios");
            }
        }

        public List<InventariosEntidad> MtdConsultar()
        {
            return MtdLeer(string.Empty);
        }

        public List<InventariosEntidad> MtdBuscar(string filtro)
        {
            return MtdLeer(filtro);
        }

        // =====================================================
        // CRUD: AGREGAR
        // =====================================================

        public bool MtdAgregar(InventariosEntidad entidad)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    string QueryAgregar = @"
                        INSERT INTO Tbl_Inventarios
                        (
                                id_sucursal,
                fecha,
                descripcion
                        )
                        VALUES
                        (
                                @CodigoSucursal,
                @FechaInventario,
                @Descripcion
                        );
                    ";

                    using (SqlCommand cmd = new SqlCommand(QueryAgregar, conn))
                    {
                        cmd.Parameters.AddWithValue("@CodigoSucursal", entidad.CodigoSucursal);
                        cmd.Parameters.AddWithValue("@FechaInventario", entidad.FechaInventario);
                        cmd.Parameters.AddWithValue("@Descripcion", entidad.Descripcion);

                        conn.Open();

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "agregar el inventario");
            }
        }

        // =====================================================
        // CRUD: EDITAR
        // =====================================================

        public bool MtdEditar(InventariosEntidad entidad)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    string QueryEditar = @"
                        UPDATE Tbl_Inventarios
                        SET
                                id_sucursal = @CodigoSucursal,
                fecha = @FechaInventario,
                descripcion = @Descripcion
                        WHERE id_inventario = @CodigoInventario;
                    ";

                    using (SqlCommand cmd = new SqlCommand(QueryEditar, conn))
                    {
                        cmd.Parameters.AddWithValue("@CodigoSucursal", entidad.CodigoSucursal);
                        cmd.Parameters.AddWithValue("@FechaInventario", entidad.FechaInventario);
                        cmd.Parameters.AddWithValue("@Descripcion", entidad.Descripcion);
                        cmd.Parameters.AddWithValue("@CodigoInventario", entidad.CodigoInventario);

                        conn.Open();

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "editar el inventario");
            }
        }

        // =====================================================
        // CRUD: ELIMINAR
        // =====================================================

        public bool MtdEliminar(int codigo)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    string QueryEliminar = "DELETE FROM Tbl_Inventarios WHERE id_inventario = @Codigo;";

                    using (SqlCommand cmd = new SqlCommand(QueryEliminar, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigo);

                        conn.Open();

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "eliminar el inventario");
            }
        }

    } //Clase
} // namespace
