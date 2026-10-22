using Datos;
using Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class InventariosNegocio
    {
        private readonly InventariosDatos _inventariosDatos = new InventariosDatos();

        public List<InventariosEntidad> MtdConsultar()
        {
            return _inventariosDatos.MtdConsultar();
        }

        public List<InventariosEntidad> MtdBuscar(string filtro)
        {
            return _inventariosDatos.MtdBuscar(filtro == null ? string.Empty : filtro.Trim());
        }

        private void MtdValidar(InventariosEntidad inventario)
        {
            if (inventario.CodigoSucursal <= 0)
            {
                throw new Exception("Selecciona una sucursal.");
            }

            if (string.IsNullOrWhiteSpace(inventario.Descripcion))
            {
                throw new Exception("Escribe una descripcion del inventario.");
            }
        }

        public bool MtdAgregar(InventariosEntidad inventario)
        {
            MtdValidar(inventario);
            return _inventariosDatos.MtdAgregar(inventario);
        }

        public bool MtdEditar(InventariosEntidad inventario)
        {
            if (inventario.CodigoInventario <= 0)
            {
                throw new Exception("Selecciona un inventario para editar.");
            }

            MtdValidar(inventario);
            return _inventariosDatos.MtdEditar(inventario);
        }

        public bool MtdEliminar(int codigoInventario)
        {
            if (codigoInventario <= 0)
            {
                throw new Exception("Selecciona un inventario para eliminar.");
            }

            return _inventariosDatos.MtdEliminar(codigoInventario);
        }
    }
}
