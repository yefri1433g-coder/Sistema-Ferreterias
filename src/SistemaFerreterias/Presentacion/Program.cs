using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    // CLASE Program: Inicia la aplicación Windows Forms y abre el formulario Login.
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Antes de abrir el menu se prueba la conexion a SQL Server (cadena en App.config)
            try
            {
                new ConexionNegocio().ProbarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo iniciar la aplicación porque falló la conexión a SQL Server.\n\n" + ex.Message,
                    "Error de conexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new LoginForms());
        }
    }
}
