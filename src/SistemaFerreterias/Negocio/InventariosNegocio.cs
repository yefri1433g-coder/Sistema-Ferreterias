using Datos;
using Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    // CLASE InventariosNegocio: Coordina operaciones y aplica reglas antes de llamar a Datos.
    public class InventariosNegocio
    {
        private readonly InventariosDatos _inventariosDatos = new InventariosDatos();

        // CONSULTAR: MtdConsultar
        public List<InventariosEntidad> MtdConsultar()
        {
            return _inventariosDatos.MtdConsultar();
        }

        // BUSCAR: MtdBuscar
        public List<InventariosEntidad> MtdBuscar(string filtro)
        {
            return _inventariosDatos.MtdBuscar(filtro == null ? string.Empty : filtro.Trim());
        }

        // VALIDAR: MtdValidar
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

        // AGREGAR: MtdAgregar
        public bool MtdAgregar(InventariosEntidad inventario)
        {
            MtdValidar(inventario);
            return _inventariosDatos.MtdAgregar(inventario);
        }

        // EDITAR: MtdEditar
        public bool MtdEditar(InventariosEntidad inventario)
        {
            if (inventario.CodigoInventario <= 0)
            {
                throw new Exception("Selecciona un inventario para editar.");
            }

            MtdValidar(inventario);
            return _inventariosDatos.MtdEditar(inventario);
        }

        // ELIMINAR: MtdEliminar
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
