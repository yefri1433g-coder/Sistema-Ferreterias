using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entidad;
using System.Data;
using System.Data.SqlClient;

namespace Datos
{
    public class UsuarioDatos
    {
        private readonly ConexionDatos _conexion = new ConexionDatos();

        private Usuario Mapear(SqlDataReader dr)
        {
            // Convierte las columnas que devuelve SQL Server en una entidad Usuario.
            return new Usuario
            {
                IdUsuario = dr.GetInt32(dr.GetOrdinal("id_usuario")),
                NombreUsuario = dr.GetString(dr.GetOrdinal("nombre_usuario")),
                Contrasena = dr.GetString(dr.GetOrdinal("contrasena")),
                Rol = dr.GetString(dr.GetOrdinal("rol")),
                Estado = dr.GetBoolean(dr.GetOrdinal("estado"))
            };
        }

        public Usuario Validar(string nombreUsuario, string contrasena)
        {
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                {
                    cn.Open();
                    // Compara usuario y contraseña y solo acepta cuentas y roles activos.
                    var cmd = new SqlCommand(
                        "SELECT u.id_usuario, u.nombre_usuario, u.contrasena, u.rol, u.estado " +
                        "FROM dbo.Tbl_Usuarios u INNER JOIN dbo.Tbl_Roles r ON r.NombreRol = u.rol " +
                        "WHERE u.nombre_usuario = @u AND u.contrasena = @c AND u.estado = 1 AND r.Estado = 1", cn);
                    cmd.Parameters.Add("@u", SqlDbType.VarChar, 50).Value = nombreUsuario;
                    cmd.Parameters.Add("@c", SqlDbType.VarChar, 100).Value = contrasena;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read()) return Mapear(dr);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "validar el acceso");
            }
            return null;
        }

        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                {
                    cn.Open();
                    var cmd = new SqlCommand(
                        "SELECT id_usuario, nombre_usuario, contrasena, rol, estado " +
                        "FROM dbo.Tbl_Usuarios ORDER BY nombre_usuario", cn);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                        while (dr.Read()) lista.Add(Mapear(dr));
                }
            }
            catch (SqlException ex) { throw ErrorDatos.Traducir(ex, "consultar los usuarios"); }
            return lista;
        }

        public List<Usuario> Buscar(string texto)
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                {
                    cn.Open();
                    var cmd = new SqlCommand(
                        "SELECT id_usuario, nombre_usuario, contrasena, rol, estado " +
                        "FROM dbo.Tbl_Usuarios " +
                        "WHERE nombre_usuario LIKE @buscar OR rol LIKE @buscar " +
                        "ORDER BY nombre_usuario, id_usuario", cn);
                    cmd.Parameters.Add("@buscar", SqlDbType.VarChar, 102).Value = "%" + texto.Trim() + "%";
                    using (SqlDataReader dr = cmd.ExecuteReader())
                        while (dr.Read()) lista.Add(Mapear(dr));
                }
            }
            catch (SqlException ex) { throw ErrorDatos.Traducir(ex, "buscar usuarios"); }

            return lista;
        }

        public bool ExisteNombre(string nombreUsuario, int idExcluir)
        {
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                {
                    cn.Open();
                    var cmd = new SqlCommand(
                        "SELECT COUNT(1) FROM dbo.Tbl_Usuarios " +
                        "WHERE nombre_usuario = @u AND id_usuario <> @id", cn);
                    cmd.Parameters.Add("@u", SqlDbType.VarChar, 50).Value = nombreUsuario;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = idExcluir;
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
            catch (SqlException ex) { throw ErrorDatos.Traducir(ex, "validar el nombre de usuario"); }
        }

        public void Insertar(Usuario u)
        {
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                {
                    cn.Open();
                    var cmd = new SqlCommand(
                        "INSERT INTO dbo.Tbl_Usuarios (nombre_usuario, contrasena, rol, estado) " +
                        "VALUES (@u, @c, @r, @e)", cn);
                    cmd.Parameters.Add("@u", SqlDbType.VarChar, 50).Value = u.NombreUsuario;
                    cmd.Parameters.Add("@c", SqlDbType.VarChar, 100).Value = u.Contrasena;
                    cmd.Parameters.Add("@r", SqlDbType.VarChar, 30).Value = u.Rol;
                    cmd.Parameters.Add("@e", SqlDbType.Bit).Value = u.Estado;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw ErrorDatos.Traducir(ex, "guardar el usuario"); }
        }

        public void Actualizar(Usuario u)
        {
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                {
                    cn.Open();
                    var cmd = new SqlCommand(
                        "UPDATE dbo.Tbl_Usuarios SET nombre_usuario = @u, contrasena = @c, " +
                        "rol = @r, estado = @e WHERE id_usuario = @id", cn);
                    cmd.Parameters.Add("@u", SqlDbType.VarChar, 50).Value = u.NombreUsuario;
                    cmd.Parameters.Add("@c", SqlDbType.VarChar, 100).Value = u.Contrasena;
                    cmd.Parameters.Add("@r", SqlDbType.VarChar, 30).Value = u.Rol;
                    cmd.Parameters.Add("@e", SqlDbType.Bit).Value = u.Estado;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = u.IdUsuario;
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No se encontró el usuario que deseas editar.");
                }
            }
            catch (SqlException ex) { throw ErrorDatos.Traducir(ex, "editar el usuario"); }
        }

        public void Eliminar(int idUsuario)
        {
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Tbl_Usuarios WHERE id_usuario = @id", cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUsuario;
                    cn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No se encontró el usuario que deseas eliminar.");
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "eliminar el usuario");
            }
        }

        public int EliminarUsuariosDePrueba(int ultimoIdOriginal)
        {
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM dbo.Tbl_Usuarios WHERE id_usuario > @ultimoIdOriginal", cn))
                {
                    cmd.Parameters.Add("@ultimoIdOriginal", SqlDbType.Int).Value = ultimoIdOriginal;
                    cn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "limpiar los usuarios de prueba");
            }
        }
    }
}
