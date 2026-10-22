using System;
using System.Data.SqlClient;

namespace Datos
{
    // Convierte los errores de SQL Server en mensajes entendibles para el usuario
    public static class ErrorDatos
    {
        public static Exception Traducir(SqlException ex, string accion)
        {
            switch (ex.Number)
            {
                case 547:
                    return new Exception("No se puede " + accion + ": el registro tiene relacion con otros datos (llave foranea).", ex);
                case 2627:
                case 2601:
                    return new Exception("Ya existe un registro con esos datos.", ex);
                default:
                    return new Exception("Ocurrió un error al " + accion + ". Verifica los datos e inténtalo de nuevo.", ex);
            }
        }
    }
}
