using Entidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Datos
{
    public class RolDatos
    {
        private readonly ConexionDatos _conexion = new ConexionDatos();

        // Consulta los roles; puede devolver solo los activos para asignarlos a usuarios.
        public List<Rol> Listar(bool soloActivos)
        {
            List<Rol> roles = new List<Rol>();
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT CodigoRol, NombreRol, Estado FROM dbo.Tbl_Roles " +
                    (soloActivos ? "WHERE Estado = 1 " : "") + "ORDER BY NombreRol", cn))
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            roles.Add(new Rol { CodigoRol = dr.GetInt32(0), NombreRol = dr.GetString(1), Estado = dr.GetBoolean(2) });
                }
            }
            catch (SqlException ex) { throw ErrorDatos.Traducir(ex, "consultar los roles"); }
            return roles;
        }

        // Busca el rol por su nombre y carga también los módulos permitidos.
        public Rol Obtener(string nombreRol)
        {
            Rol rol = null;
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT CodigoRol, NombreRol, Estado FROM dbo.Tbl_Roles WHERE NombreRol = @nombre", cn))
                {
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 30).Value = nombreRol;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                        if (dr.Read()) rol = new Rol { CodigoRol = dr.GetInt32(0), NombreRol = dr.GetString(1), Estado = dr.GetBoolean(2) };
                }
                if (rol != null) rol.Modulos = ListarModulos(rol.NombreRol);
            }
            catch (SqlException ex) { throw ErrorDatos.Traducir(ex, "consultar el rol"); }
            return rol;
        }

        // Consulta en Tbl_RolesAccesos los módulos activos de un rol.
        public List<string> ListarModulos(string nombreRol)
        {
            List<string> modulos = new List<string>();
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT a.NombreModulo FROM dbo.Tbl_RolesAccesos a " +
                    "INNER JOIN dbo.Tbl_Roles r ON r.CodigoRol = a.CodigoRol " +
                    "WHERE r.NombreRol = @nombre AND r.Estado = 1 ORDER BY a.NombreModulo", cn))
                {
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 30).Value = nombreRol;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                        while (dr.Read()) modulos.Add(dr.GetString(0));
                }
            }
            catch (SqlException ex) { throw ErrorDatos.Traducir(ex, "consultar los accesos del rol"); }
            return modulos;
        }

        // Guarda el rol y reemplaza su lista de accesos dentro de una transacción.
        public void Guardar(Rol rol)
        {
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                {
                    cn.Open();
                    using (SqlTransaction tx = cn.BeginTransaction())
                    {
                        try
                        {
                            int codigo;
                            if (rol.CodigoRol == 0)
                            {
                                using (SqlCommand cmd = new SqlCommand(
                                    "INSERT dbo.Tbl_Roles (NombreRol, Estado) VALUES (@nombre, @estado); " +
                                    "SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                                {
                                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 30).Value = rol.NombreRol;
                                    cmd.Parameters.Add("@estado", SqlDbType.Bit).Value = rol.Estado;
                                    codigo = (int)cmd.ExecuteScalar();
                                }
                            }
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand(
                                    "UPDATE dbo.Tbl_Roles SET NombreRol = @nombre, Estado = @estado WHERE CodigoRol = @codigo", cn, tx))
                                {
                                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 30).Value = rol.NombreRol;
                                    cmd.Parameters.Add("@estado", SqlDbType.Bit).Value = rol.Estado;
                                    cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = rol.CodigoRol;
                                    if (cmd.ExecuteNonQuery() == 0) throw new InvalidOperationException("El rol ya no existe.");
                                }
                                codigo = rol.CodigoRol;
                            }

                            using (SqlCommand cmd = new SqlCommand("DELETE dbo.Tbl_RolesAccesos WHERE CodigoRol = @codigo", cn, tx))
                            {
                                cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = codigo;
                                cmd.ExecuteNonQuery();
                            }
                            foreach (string modulo in rol.Modulos)
                            {
                                using (SqlCommand cmd = new SqlCommand(
                                    "INSERT dbo.Tbl_RolesAccesos (CodigoRol, NombreModulo) VALUES (@codigo, @modulo)", cn, tx))
                                {
                                    cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = codigo;
                                    cmd.Parameters.Add("@modulo", SqlDbType.VarChar, 40).Value = modulo;
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            using (SqlCommand cmd = new SqlCommand(
                                "SELECT COUNT(1) FROM dbo.Tbl_Roles r INNER JOIN dbo.Tbl_RolesAccesos a " +
                                "ON a.CodigoRol = r.CodigoRol WHERE r.Estado = 1 AND a.NombreModulo = 'Roles y accesos'", cn, tx))
                            {
                                if ((int)cmd.ExecuteScalar() == 0)
                                    throw new InvalidOperationException("Al menos un rol activo debe conservar acceso a Roles y accesos.");
                            }
                            tx.Commit();
                        }
                        catch { tx.Rollback(); throw; }
                    }
                }
            }
            catch (SqlException ex) { throw ErrorDatos.Traducir(ex, "guardar el rol"); }
        }

        // Elimina un rol solo cuando ningún usuario está asignado a él.
        public void Eliminar(int codigoRol)
        {
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE dbo.Tbl_RolesAccesos WHERE CodigoRol = @codigo AND NOT EXISTS " +
                    "(SELECT 1 FROM dbo.Tbl_Usuarios u INNER JOIN dbo.Tbl_Roles r ON r.NombreRol = u.rol WHERE r.CodigoRol = @codigo); " +
                    "DELETE dbo.Tbl_Roles WHERE CodigoRol = @codigo AND NOT EXISTS " +
                    "(SELECT 1 FROM dbo.Tbl_Usuarios u INNER JOIN dbo.Tbl_Roles r ON r.NombreRol = u.rol WHERE r.CodigoRol = @codigo);", cn))
                {
                    cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = codigoRol;
                    cn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No se puede eliminar un rol que no existe o que tiene usuarios asignados.");
                }
            }
            catch (SqlException ex) { throw ErrorDatos.Traducir(ex, "eliminar el rol"); }
        }
    }
}
