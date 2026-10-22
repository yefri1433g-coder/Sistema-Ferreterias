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
            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "SELECT id_usuario, nombre_usuario, contrasena, rol, estado " +
                    "FROM Tbl_Usuarios " +
                    "WHERE nombre_usuario = @u AND contrasena = @c AND estado = 1", cn);
                cmd.Parameters.AddWithValue("@u", nombreUsuario);
                cmd.Parameters.AddWithValue("@c", contrasena);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        return Mapear(dr);
                }
            }
            return null;
        }

        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "SELECT id_usuario, nombre_usuario, contrasena, rol, estado " +
                    "FROM Tbl_Usuarios ORDER BY nombre_usuario", cn);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(Mapear(dr));
                }
            }
            return lista;
        }

        public List<Usuario> Buscar(string texto)
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "SELECT id_usuario, nombre_usuario, contrasena, rol, estado " +
                    "FROM Tbl_Usuarios " +
                    "WHERE nombre_usuario LIKE @buscar OR rol LIKE @buscar " +
                    "ORDER BY nombre_usuario, id_usuario", cn);
                cmd.Parameters.Add("@buscar", SqlDbType.VarChar, 102).Value = "%" + texto.Trim() + "%";

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(Mapear(dr));
                }
            }

            return lista;
        }

        public bool ExisteNombre(string nombreUsuario, int idExcluir)
        {
            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "SELECT COUNT(1) FROM Tbl_Usuarios " +
                    "WHERE nombre_usuario = @u AND id_usuario <> @id", cn);
                cmd.Parameters.AddWithValue("@u", nombreUsuario);
                cmd.Parameters.AddWithValue("@id", idExcluir);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public void Insertar(Usuario u)
        {
            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Tbl_Usuarios (nombre_usuario, contrasena, rol, estado) " +
                    "VALUES (@u, @c, @r, @e)", cn);
                cmd.Parameters.AddWithValue("@u", u.NombreUsuario);
                cmd.Parameters.AddWithValue("@c", u.Contrasena);
                cmd.Parameters.AddWithValue("@r", u.Rol);
                cmd.Parameters.AddWithValue("@e", u.Estado);
                cmd.ExecuteNonQuery();
            }
        }

        public void Actualizar(Usuario u)
        {
            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "UPDATE Tbl_Usuarios SET nombre_usuario = @u, contrasena = @c, " +
                    "rol = @r, estado = @e WHERE id_usuario = @id", cn);
                cmd.Parameters.AddWithValue("@u", u.NombreUsuario);
                cmd.Parameters.AddWithValue("@c", u.Contrasena);
                cmd.Parameters.AddWithValue("@r", u.Rol);
                cmd.Parameters.AddWithValue("@e", u.Estado);
                cmd.Parameters.AddWithValue("@id", u.IdUsuario);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException("No se encontró el usuario que deseas editar.");
            }
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
