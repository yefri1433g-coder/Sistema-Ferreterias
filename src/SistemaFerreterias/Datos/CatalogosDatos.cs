using Entidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    // Listas para llenar los ComboBox (sucursales, productos, empleados)
    public class CatalogosDatos
    {
        private readonly ConexionDatos _conexionDatos = new ConexionDatos();

        private List<ComboItemEntidad> MtdLeerCombo(string query, string accion)
        {
            List<ComboItemEntidad> lista = new List<ComboItemEntidad>();

            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                lista.Add(new ComboItemEntidad(Convert.ToInt32(rd["Codigo"]), rd["Texto"].ToString()));
                            }
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

        public List<ComboItemEntidad> MtdSucursales()
        {
            return MtdLeerCombo("SELECT id_sucursal AS Codigo, nombre AS Texto FROM Tbl_Sucursales WHERE estado = 1 ORDER BY nombre;", "consultar las sucursales");
        }

        public List<ComboItemEntidad> MtdProductos()
        {
            return MtdLeerCombo("SELECT id_producto AS Codigo, codigo + ' - ' + nombre AS Texto FROM Tbl_Productos WHERE estado = 1 ORDER BY nombre;", "consultar los productos");
        }

        public List<ComboItemEntidad> MtdEmpleados()
        {
            return MtdLeerCombo("SELECT id_empleado AS Codigo, nombre + ' ' + apellido AS Texto FROM Tbl_Empleados WHERE estado = 1 ORDER BY nombre;", "consultar los empleados");
        }
    }
}
