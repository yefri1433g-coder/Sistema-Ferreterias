using Entidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ProveedoresDatos
    {
        // Instanciar: Que la clase actual (Datos) pueda acceder a otra clase (ConexionDatos)
        private readonly ConexionDatos _conexionDatos = new ConexionDatos();

        // =====================================================
        // CRUD: CONSULTAR / BUSCAR
        // =====================================================

        private List<ProveedoresEntidad> MtdLeer(string filtro)
        {
            List<ProveedoresEntidad> lista = new List<ProveedoresEntidad>();

            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    string QueryConsultar = @"
                        SELECT id_proveedor AS CodigoProveedor, nombre AS NombreProveedor, Contacto, Telefono, Correo, Direccion
                        FROM Tbl_Proveedores
                        -- Búsqueda por PK y atributos principales del proveedor.
                        WHERE (@Filtro = ''
                               OR CONVERT(VARCHAR(20), id_proveedor) LIKE '%' + @Filtro + '%'
                               OR nombre LIKE '%' + @Filtro + '%'
                               OR contacto LIKE '%' + @Filtro + '%'
                               OR telefono LIKE '%' + @Filtro + '%'
                               OR correo LIKE '%' + @Filtro + '%')
                        ORDER BY id_proveedor;
                    ";

                    using (SqlCommand cmd = new SqlCommand(QueryConsultar, conn))
                    {
                        cmd.Parameters.AddWithValue("@Filtro", filtro ?? string.Empty);
                        conn.Open();

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                ProveedoresEntidad item = new ProveedoresEntidad
                                (
                                    Convert.ToInt32(rd["CodigoProveedor"]),
                                    rd["NombreProveedor"].ToString(),
                                    rd["Contacto"].ToString(),
                                    rd["Telefono"].ToString(),
                                    rd["Correo"].ToString(),
                                    rd["Direccion"].ToString()
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
                throw ErrorDatos.Traducir(ex, "consultar los proveedores");
            }
        }

        public List<ProveedoresEntidad> MtdConsultar()
        {
            return MtdLeer(string.Empty);
        }

        public List<ProveedoresEntidad> MtdBuscar(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                return MtdConsultar();
            }

            // MtdLeer concentra el SELECT parametrizado para Consultar y Buscar.
            return MtdLeer(filtro.Trim());
        }

        // =====================================================
        // CRUD: AGREGAR
        // =====================================================

        public bool MtdAgregar(ProveedoresEntidad entidad)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    string QueryAgregar = @"
                        INSERT INTO dbo.Tbl_Proveedores
                        (
                            nombre,
                            contacto,
                            telefono,
                            correo,
                            direccion
                        )
                        VALUES
                        (
                            @NombreProveedor,
                            @Contacto,
                            @Telefono,
                            @Correo,
                            @Direccion
                        );
                    ";

                    using (SqlCommand cmd = new SqlCommand(QueryAgregar, conn))
                    {
                        // Los tamaños y tipos coinciden con dbo.Tbl_Proveedores.
                        cmd.Parameters.Add("@NombreProveedor", SqlDbType.VarChar, 100).Value = entidad.NombreProveedor;
                        cmd.Parameters.Add("@Contacto", SqlDbType.VarChar, 80).Value =
                            string.IsNullOrWhiteSpace(entidad.Contacto) ? (object)DBNull.Value : entidad.Contacto;
                        cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 15).Value =
                            string.IsNullOrWhiteSpace(entidad.Telefono) ? (object)DBNull.Value : entidad.Telefono;
                        cmd.Parameters.Add("@Correo", SqlDbType.VarChar, 100).Value =
                            string.IsNullOrWhiteSpace(entidad.Correo) ? (object)DBNull.Value : entidad.Correo;
                        cmd.Parameters.Add("@Direccion", SqlDbType.VarChar, 150).Value =
                            string.IsNullOrWhiteSpace(entidad.Direccion) ? (object)DBNull.Value : entidad.Direccion;

                        conn.Open();

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "agregar el proveedor");
            }
        }

        // =====================================================
        // CRUD: EDITAR
        // =====================================================

        public bool MtdEditar(ProveedoresEntidad entidad)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    string QueryEditar = @"
                        UPDATE dbo.Tbl_Proveedores
                        SET
                            nombre = @NombreProveedor,
                            contacto = @Contacto,
                            telefono = @Telefono,
                            correo = @Correo,
                            direccion = @Direccion
                        WHERE id_proveedor = @CodigoProveedor;
                    ";

                    using (SqlCommand cmd = new SqlCommand(QueryEditar, conn))
                    {
                        // id_proveedor es la PK; los tamaños corresponden al esquema real.
                        cmd.Parameters.Add("@NombreProveedor", SqlDbType.VarChar, 100).Value = entidad.NombreProveedor;
                        cmd.Parameters.Add("@Contacto", SqlDbType.VarChar, 80).Value =
                            string.IsNullOrWhiteSpace(entidad.Contacto) ? (object)DBNull.Value : entidad.Contacto;
                        cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 15).Value =
                            string.IsNullOrWhiteSpace(entidad.Telefono) ? (object)DBNull.Value : entidad.Telefono;
                        cmd.Parameters.Add("@Correo", SqlDbType.VarChar, 100).Value =
                            string.IsNullOrWhiteSpace(entidad.Correo) ? (object)DBNull.Value : entidad.Correo;
                        cmd.Parameters.Add("@Direccion", SqlDbType.VarChar, 150).Value =
                            string.IsNullOrWhiteSpace(entidad.Direccion) ? (object)DBNull.Value : entidad.Direccion;
                        cmd.Parameters.Add("@CodigoProveedor", SqlDbType.Int).Value = entidad.CodigoProveedor;

                        conn.Open();

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "editar el proveedor");
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
                    string QueryEliminar = "DELETE FROM dbo.Tbl_Proveedores WHERE id_proveedor = @CodigoProveedor;";

                    using (SqlCommand cmd = new SqlCommand(QueryEliminar, conn))
                    {
                        // Se elimina únicamente por la clave primaria id_proveedor.
                        cmd.Parameters.Add("@CodigoProveedor", SqlDbType.Int).Value = codigo;

                        conn.Open();

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "eliminar el proveedor");
            }
        }

    } //Clase
} // namespace
