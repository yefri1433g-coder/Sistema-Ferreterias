using Datos;
using Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    // CLASE ProveedoresNegocio: Coordina operaciones y aplica reglas antes de llamar a Datos.
    public class ProveedoresNegocio
    {
        // Instancia: para que negocio acceda a capa datos
        private readonly ProveedoresDatos _proveedoresDatos = new ProveedoresDatos();

        // =====================================================
        // CRUD: CONSULTAR / BUSCAR
        // =====================================================

        // CONSULTAR: MtdConsultar
        public List<ProveedoresEntidad> MtdConsultar()
        {
            return _proveedoresDatos.MtdConsultar();
        }

        // BUSCAR: MtdBuscar
        public List<ProveedoresEntidad> MtdBuscar(string nombre)
        {
            return _proveedoresDatos.MtdBuscar(nombre == null ? string.Empty : nombre.Trim());
        }

        // =====================================================
        // VALIDACIONES
        // =====================================================

        // VALIDAR: MtdValidar
        private void MtdValidar(ProveedoresEntidad proveedor)
        {
            if (string.IsNullOrWhiteSpace(proveedor.NombreProveedor))
            {
                throw new Exception("El nombre del proveedor es obligatorio.");
            }

            if (!string.IsNullOrWhiteSpace(proveedor.Correo) && !proveedor.Correo.Contains("@"))
            {
                throw new Exception("El correo no es valido.");
            }
        }

        // =====================================================
        // CRUD: AGREGAR / EDITAR / ELIMINAR
        // =====================================================

        // AGREGAR: MtdAgregar
        public bool MtdAgregar(ProveedoresEntidad proveedor)
        {
            MtdValidar(proveedor);
            return _proveedoresDatos.MtdAgregar(proveedor);
        }

        // EDITAR: MtdEditar
        public bool MtdEditar(ProveedoresEntidad proveedor)
        {
            if (proveedor.CodigoProveedor <= 0)
            {
                throw new Exception("Selecciona un proveedor para editar.");
            }

            MtdValidar(proveedor);
            return _proveedoresDatos.MtdEditar(proveedor);
        }

        // ELIMINAR: MtdEliminar
        public bool MtdEliminar(int codigoProveedor)
        {
            if (codigoProveedor <= 0)
            {
                throw new Exception("Selecciona un proveedor para eliminar.");
            }

            return _proveedoresDatos.MtdEliminar(codigoProveedor);
        }
    }
}
