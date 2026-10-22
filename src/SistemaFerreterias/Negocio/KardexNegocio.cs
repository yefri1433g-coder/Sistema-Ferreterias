using Datos;
using Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class KardexNegocio
    {
        private readonly KardexDatos _kardexDatos = new KardexDatos();

        // codigoProducto = 0 trae los movimientos de todos los productos
        public List<KardexEntidad> MtdConsultar(int codigoProducto)
        {
            return _kardexDatos.MtdConsultar(codigoProducto);
        }
    }
}
