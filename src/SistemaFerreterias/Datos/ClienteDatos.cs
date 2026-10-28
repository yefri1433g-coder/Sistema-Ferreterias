using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entidad;

namespace Datos
{
    // CLASE ClienteDatos: Realiza consultas y operaciones de persistencia en SQL Server.
    public class ClienteDatos
    {
        private readonly ConexionDatos _conexion = new ConexionDatos();

        private const string Columnas = "id_cliente, nombre, nit, telefono, direccion";

        // nit, telefono y direccion pueden ser NULL en la base de datos
        private static string Texto(SqlDataReader dr, string columna)
        {
            int i = dr.GetOrdinal(columna);
            return dr.IsDBNull(i) ? "" : dr.GetString(i);
        }

        private static object ValorODbNull(string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? (object)DBNull.Value : texto.Trim();
        }

        private Cliente Mapear(SqlDataReader dr)
        {
            return new Cliente
            {
                IdCliente = dr.GetInt32(dr.GetOrdinal("id_cliente")),
                Nombre = dr.GetString(dr.GetOrdinal("nombre")),
                Nit = Texto(dr, "nit"),
                Telefono = Texto(dr, "telefono"),
                Direccion = Texto(dr, "direccion")
            };
        }

        // CONSULTAR: Listar
        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "SELECT " + Columnas + " FROM Tbl_Clientes ORDER BY nombre", cn);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(Mapear(dr));
                }
            }
            return lista;
        }

        // Busca coincidencias parciales en el nombre mediante un parámetro SQL.
        // BUSCAR: BuscarPorNombre
        public List<Cliente> BuscarPorNombre(string nombre)
        {
            List<Cliente> lista = new List<Cliente>();

            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT " + Columnas + " FROM Tbl_Clientes " +
                    "WHERE nombre LIKE @nombre ORDER BY nombre", cn))
                {
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 102).Value = "%" + nombre.Trim() + "%";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                            lista.Add(Mapear(dr));
                    }
                }
            }

            return lista;
        }

        public bool ExisteNit(string nit, int idExcluir)
        {
            using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                var cmd = new SqlCommand(
                    "SELECT COUNT(1) FROM Tbl_Clientes " +
                    "WHERE nit = @nit AND id_cliente <> @id", cn);
                cmd.Parameters.AddWithValue("@nit", nit);
                cmd.Parameters.AddWithValue("@id", idExcluir);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        // AGREGAR: Insertar
        public void Insertar(Cliente c)
        {
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "INSERT INTO dbo.Tbl_Clientes (nombre, nit, telefono, direccion) " +
                        "VALUES (@nombre, @nit, @telefono, @direccion)", cn))
                    {
                        // Los tipos y tamaños coinciden con las columnas de Tbl_Clientes.
                        cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = c.Nombre;
                        cmd.Parameters.Add("@nit", SqlDbType.VarChar, 15).Value = ValorODbNull(c.Nit);
                        cmd.Parameters.Add("@telefono", SqlDbType.VarChar, 15).Value = ValorODbNull(c.Telefono);
                        cmd.Parameters.Add("@direccion", SqlDbType.VarChar, 150).Value = ValorODbNull(c.Direccion);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "agregar el cliente");
            }
        }

        // EDITAR: Actualizar
        public void Actualizar(Cliente c)
        {
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE Tbl_Clientes SET nombre = @nombre, nit = @nit, " +
                    "telefono = @telefono, direccion = @direccion " +
                    "WHERE id_cliente = @id", cn))
                {
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = c.Nombre;
                    cmd.Parameters.Add("@nit", SqlDbType.VarChar, 15).Value = ValorODbNull(c.Nit);
                    cmd.Parameters.Add("@telefono", SqlDbType.VarChar, 15).Value = ValorODbNull(c.Telefono);
                    cmd.Parameters.Add("@direccion", SqlDbType.VarChar, 150).Value = ValorODbNull(c.Direccion);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = c.IdCliente;

                    cn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No se encontró el cliente que deseas editar.");
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "editar el cliente");
            }
        }

        // ELIMINAR: Eliminar
        public void Eliminar(int idCliente)
        {
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Tbl_Clientes WHERE id_cliente = @id", cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = idCliente;
                    cn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No se encontró el cliente que deseas eliminar.");
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "eliminar el cliente");
            }
        }

        // Elimina los registros agregados después del conjunto original identificado por el usuario.
        // ELIMINAR: EliminarPosterioresA
        public int EliminarPosterioresA(int ultimoIdOriginal)
        {
            try
            {
                using (SqlConnection cn = _conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM dbo.Tbl_Clientes WHERE id_cliente > @ultimoIdOriginal", cn))
                {
                    cmd.Parameters.Add("@ultimoIdOriginal", SqlDbType.Int).Value = ultimoIdOriginal;
                    cn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "limpiar los clientes de prueba");
            }
        }
    }
}
