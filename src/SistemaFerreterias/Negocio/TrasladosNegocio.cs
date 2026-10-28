using Datos;
using Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    // CLASE TrasladosNegocio: Coordina operaciones y aplica reglas antes de llamar a Datos.
    public class TrasladosNegocio
    {
        private readonly TrasladosDatos _trasladosDatos = new TrasladosDatos();

        // CONSULTAR: MtdConsultar
        public List<TrasladosEntidad> MtdConsultar()
        {
            return _trasladosDatos.MtdConsultar();
        }

        // BUSCAR: MtdBuscar
        public List<TrasladosEntidad> MtdBuscar(string filtro)
        {
            return _trasladosDatos.MtdBuscar(filtro == null ? string.Empty : filtro.Trim());
        }

        // CONSULTAR: MtdConsultarDetalle
        public List<DetalleTrasladoEntidad> MtdConsultarDetalle(int codigoTraslado)
        {
            return _trasladosDatos.MtdConsultarDetalle(codigoTraslado);
        }

        // VALIDAR: MtdValidar
        private void MtdValidar(TrasladosEntidad traslado)
        {
            if (traslado.CodigoEmpleado <= 0)
            {
                throw new Exception("Selecciona el empleado que realiza el traslado.");
            }

            if (traslado.SucursalOrigen <= 0 || traslado.SucursalDestino <= 0)
            {
                throw new Exception("Selecciona la sucursal de origen y la de destino.");
            }

            if (traslado.SucursalOrigen == traslado.SucursalDestino)
            {
                throw new Exception("La sucursal de origen y la de destino deben ser diferentes.");
            }

            if (traslado.Detalles == null || traslado.Detalles.Count == 0)
            {
                throw new Exception("Agrega al menos un producto al traslado.");
            }

            foreach (DetalleTrasladoEntidad det in traslado.Detalles)
            {
                if (det.Cantidad <= 0)
                {
                    throw new Exception("Todas las cantidades deben ser mayores a cero.");
                }
            }
        }

        // Devuelve el codigo del nuevo traslado
        // AGREGAR: MtdAgregar
        public int MtdAgregar(TrasladosEntidad traslado)
        {
            MtdValidar(traslado);
            return _trasladosDatos.MtdAgregar(traslado);
        }

        // EDITAR: MtdEditar
        public bool MtdEditar(TrasladosEntidad traslado)
        {
            if (traslado.CodigoTraslado <= 0)
            {
                throw new Exception("Selecciona un traslado para editar.");
            }

            MtdValidar(traslado);
            return _trasladosDatos.MtdEditar(traslado);
        }

        public bool MtdCambiarEstado(int codigoTraslado, string nuevoEstado)
        {
            if (codigoTraslado <= 0)
            {
                throw new Exception("Selecciona un traslado.");
            }

            if (nuevoEstado != EstadosTraslado.Pendiente && nuevoEstado != EstadosTraslado.EnTransito && nuevoEstado != EstadosTraslado.Completado)
            {
                throw new Exception("Estado no valido.");
            }

            return _trasladosDatos.MtdCambiarEstado(codigoTraslado, nuevoEstado);
        }

        // ELIMINAR: MtdEliminar
        public bool MtdEliminar(int codigoTraslado)
        {
            if (codigoTraslado <= 0)
            {
                throw new Exception("Selecciona un traslado para eliminar.");
            }

            return _trasladosDatos.MtdEliminar(codigoTraslado);
        }
    }
}
