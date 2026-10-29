using Datos;
using Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    // CLASE DashboardNegocio: Coordina operaciones y aplica reglas antes de llamar a Datos.
    public class DashboardNegocio
    {
        private readonly DashboardDatos _dashboardDatos = new DashboardDatos();

        public DashboardEntidad MtdResumen()
        {
            return _dashboardDatos.MtdResumen();
        }
    }
}
