using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entidad;

namespace Datos
{
    public class EmpleadoDatos
    {
        private readonly ConexionDatos _conexion = new ConexionDatos();

        public List<Empleado> Listar()
        {
            return Consultar(null);
        }

        public List<Empleado> Buscar(string criterio)
        {
            return Consultar(criterio);
        }

        private List<Empleado> Consultar(string criterio)
        {
            List<Empleado> lista = new List<Empleado>();
            string sql = @"SELECT e.id_empleado, e.id_usuario, u.nombre_usuario,
                                  e.id_sucursal, s.nombre AS sucursal, e.nombre,
                                  e.apellido, e.cargo, e.telefono, e.estado
                           FROM dbo.Tbl_Empleados e
                           INNER JOIN dbo.Tbl_Usuarios u ON u.id_usuario = e.id_usuario
                           INNER JOIN dbo.Tbl_Sucursales s ON s.id_sucursal = e.id_sucursal
                           WHERE @criterio IS NULL
                              OR CONVERT(VARCHAR(20), e.id_empleado) LIKE @criterio
                              OR CONVERT(VARCHAR(20), e.id_usuario) LIKE @criterio
                              OR CONVERT(VARCHAR(20), e.id_sucursal) LIKE @criterio
                              OR e.nombre LIKE @criterio
                              OR e.apellido LIKE @criterio
                              OR e.cargo LIKE @criterio
                              OR e.telefono LIKE @criterio
                              OR u.nombre_usuario LIKE @criterio
                              OR s.nombre LIKE @criterio
                           ORDER BY e.id_empleado;";

            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    string filtro = string.IsNullOrWhiteSpace(criterio)
                        ? null
                        : "%" + criterio.Trim() + "%";
                    cmd.Parameters.Add("@criterio", SqlDbType.VarChar, 100).Value =
                        filtro == null ? (object)DBNull.Value : filtro;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Empleado
                            {
                                IdEmpleado = Convert.ToInt32(dr["id_empleado"]),
                                IdUsuario = Convert.ToInt32(dr["id_usuario"]),
                                Usuario = Convert.ToString(dr["nombre_usuario"]),
                                IdSucursal = Convert.ToInt32(dr["id_sucursal"]),
                                Sucursal = Convert.ToString(dr["sucursal"]),
                                Nombre = Convert.ToString(dr["nombre"]),
                                Apellido = Convert.ToString(dr["apellido"]),
                                Cargo = Convert.ToString(dr["cargo"]),
                                Telefono = dr["telefono"] == DBNull.Value ? "" : Convert.ToString(dr["telefono"]),
                                Estado = Convert.ToBoolean(dr["estado"])
                            });
                        }
                    }
                }

                return lista;
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "consultar los empleados");
            }
        }

        public List<ComboItemEntidad> ListarUsuariosActivos()
        {
            return ListarOpciones(
                "SELECT id_usuario AS Codigo, nombre_usuario + ' (ID ' + CAST(id_usuario AS VARCHAR(10)) + ')' AS Texto " +
                "FROM dbo.Tbl_Usuarios WHERE estado = 1 ORDER BY nombre_usuario, id_usuario;",
                "consultar usuarios activos");
        }

        public List<ComboItemEntidad> ListarSucursalesActivas()
        {
            return ListarOpciones(
                "SELECT id_sucursal AS Codigo, nombre + ' (ID ' + CAST(id_sucursal AS VARCHAR(10)) + ')' AS Texto " +
                "FROM dbo.Tbl_Sucursales WHERE estado = 1 ORDER BY nombre, id_sucursal;",
                "consultar sucursales activas");
        }

        private List<ComboItemEntidad> ListarOpciones(string sql, string accion)
        {
            List<ComboItemEntidad> lista = new List<ComboItemEntidad>();
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ComboItemEntidad(
                                Convert.ToInt32(dr["Codigo"]), Convert.ToString(dr["Texto"])));
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

        public void Agregar(Empleado empleado)
        {
            const string sql = @"INSERT INTO dbo.Tbl_Empleados
                                     (id_usuario, id_sucursal, nombre, apellido, cargo, telefono, estado)
                                 VALUES
                                     (@idUsuario, @idSucursal, @nombre, @apellido, @cargo, @telefono, @estado);";

            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = empleado.IdUsuario;
                    cmd.Parameters.Add("@idSucursal", SqlDbType.Int).Value = empleado.IdSucursal;
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 60).Value = empleado.Nombre;
                    cmd.Parameters.Add("@apellido", SqlDbType.VarChar, 60).Value = empleado.Apellido;
                    cmd.Parameters.Add("@cargo", SqlDbType.VarChar, 50).Value = empleado.Cargo;
                    cmd.Parameters.Add("@telefono", SqlDbType.VarChar, 15).Value =
                        string.IsNullOrWhiteSpace(empleado.Telefono) ? (object)DBNull.Value : empleado.Telefono;
                    cmd.Parameters.Add("@estado", SqlDbType.Bit).Value = empleado.Estado;
                    cn.Open();

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No se pudo agregar el empleado.");
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "agregar el empleado");
            }
        }

        public void Editar(Empleado empleado)
        {
            const string sql = @"UPDATE dbo.Tbl_Empleados
                                 SET id_usuario = @idUsuario,
                                     id_sucursal = @idSucursal,
                                     nombre = @nombre,
                                     apellido = @apellido,
                                     cargo = @cargo,
                                     telefono = @telefono,
                                     estado = @estado
                                 WHERE id_empleado = @idEmpleado;";

            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@idEmpleado", SqlDbType.Int).Value = empleado.IdEmpleado;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = empleado.IdUsuario;
                    cmd.Parameters.Add("@idSucursal", SqlDbType.Int).Value = empleado.IdSucursal;
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 60).Value = empleado.Nombre;
                    cmd.Parameters.Add("@apellido", SqlDbType.VarChar, 60).Value = empleado.Apellido;
                    cmd.Parameters.Add("@cargo", SqlDbType.VarChar, 50).Value = empleado.Cargo;
                    cmd.Parameters.Add("@telefono", SqlDbType.VarChar, 15).Value =
                        string.IsNullOrWhiteSpace(empleado.Telefono) ? (object)DBNull.Value : empleado.Telefono;
                    cmd.Parameters.Add("@estado", SqlDbType.Bit).Value = empleado.Estado;
                    cn.Open();

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No se encontró el empleado que deseas actualizar.");
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "editar el empleado");
            }
        }

        public void Eliminar(int idEmpleado)
        {
            const string sql = "DELETE FROM dbo.Tbl_Empleados WHERE id_empleado = @idEmpleado;";
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@idEmpleado", SqlDbType.Int).Value = idEmpleado;
                    cn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No se encontró el empleado que deseas eliminar.");
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "eliminar el empleado");
            }
        }
    }
}
