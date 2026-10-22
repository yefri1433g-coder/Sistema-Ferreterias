using Datos;
using Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class CatalogosNegocio
    {
        private readonly CatalogosDatos _catalogosDatos = new CatalogosDatos();

        public List<ComboItemEntidad> MtdSucursales()
        {
            return _catalogosDatos.MtdSucursales();
        }

        public List<ComboItemEntidad> MtdProductos()
        {
            return _catalogosDatos.MtdProductos();
        }

        public List<ComboItemEntidad> MtdEmpleados()
        {
            return _catalogosDatos.MtdEmpleados();
        }

        // Lista de productos con la opcion "Todos" al inicio (para filtros)
        public List<ComboItemEntidad> MtdProductosConTodos()
        {
            List<ComboItemEntidad> lista = _catalogosDatos.MtdProductos();
            lista.Insert(0, new ComboItemEntidad(0, "(Todos los productos)"));
            return lista;
        }
    }
}
