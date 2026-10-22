using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ConexionDatos
    {
        private readonly string _cadenaConexion;

        public ConexionDatos()
        {
            try
            {
                string conexionPorEntorno = Environment.GetEnvironmentVariable("FERRETERIA_CONNECTION_STRING");

                if (!string.IsNullOrWhiteSpace(conexionPorEntorno))
                {
                    _cadenaConexion = conexionPorEntorno;
                    return;
                }

                ConnectionStringSettings configuracion = ConfigurationManager.ConnectionStrings["ConexionSQLServer"];
                if (configuracion == null || string.IsNullOrWhiteSpace(configuracion.ConnectionString))
                {
                    throw new ConfigurationErrorsException(
                        "No existe una cadena de conexión llamada 'ConexionSQLServer' en App.config.");
                }

                _cadenaConexion = configuracion.ConnectionString;
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new InvalidOperationException(
                    "No se pudo leer la configuración de conexión a SQL Server. " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Ocurrió un error al preparar la conexión a SQL Server. " + ex.Message, ex);
            }
        }

        public SqlConnection MtdObtenerConexionBDD()
        {
            if (string.IsNullOrWhiteSpace(_cadenaConexion))
            {
                throw new InvalidOperationException(
                    "La cadena de conexión está vacía. Configura FERRETERIA_CONNECTION_STRING o ConexionSQLServer.");
            }

            return new SqlConnection(_cadenaConexion);
        }

        public void MtdProbarConexion()
        {
            try
            {
                using (SqlConnection conn = MtdObtenerConexionBDD())
                {
                    conn.Open();
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(
                    "No fue posible conectar con SQL Server. Comprueba que el servidor esté encendido y que el puerto, " +
                    "la base de datos y la autenticación sean correctos. Detalle: " + ex.Message, ex);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Ocurrió un error inesperado durante la prueba de conexión. Detalle: " + ex.Message, ex);
            }
        }
    }
}
