using System;
using System.Data;
using Datos;
using Entidad;

namespace Negocio
{
    // CLASE PagoNegocio: Coordina operaciones y aplica reglas antes de llamar a Datos.
    public class PagoNegocio
    {
        private readonly PagoDatos _datos = new PagoDatos();

        // CONSULTAR: MtdListarVentas
        public DataTable MtdListarVentas()
        {
            return _datos.MtdListarVentas();
        }

        // CONSULTAR: MtdListarPagosRecientes
        public DataTable MtdListarPagosRecientes()
        {
            return _datos.MtdListarPagosRecientes();
        }

        // CONSULTAR: MtdListarPagos
        public DataTable MtdListarPagos(int idVenta)
        {
            return _datos.MtdListarPagosPorVenta(idVenta);
        }

        // GUARDAR: MtdRegistrar
        public bool MtdRegistrar(Pago pago, out string mensaje)
        {
            if (pago == null || pago.IdVenta <= 0)
            {
                mensaje = "Seleccione una venta.";
                return false;
            }

            if (pago.Monto <= 0)
            {
                mensaje = "El monto debe ser mayor a cero.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(pago.MetodoPago))
            {
                mensaje = "Seleccione el método de pago.";
                return false;
            }

            try
            {
                decimal saldo = _datos.MtdObtenerSaldo(pago.IdVenta);
                if (saldo <= 0)
                {
                    mensaje = "Esta venta ya está pagada por completo.";
                    return false;
                }

                if (pago.Monto > saldo)
                {
                    mensaje = "El monto (Q " + pago.Monto.ToString("N2") +
                              ") supera el saldo pendiente (Q " + saldo.ToString("N2") + ").";
                    return false;
                }

                bool ok = _datos.MtdInsertar(pago);
                mensaje = ok ? "Pago registrado correctamente." : "No se pudo registrar el pago.";
                return ok;
            }
            catch
            {
                mensaje = "No se pudo registrar el pago. Verifica los datos e inténtalo de nuevo.";
                return false;
            }
        }

        // EDITAR: MtdEditar
        public bool MtdEditar(Pago pago, out string mensaje)
        {
            if (pago == null || pago.IdPago <= 0)
            {
                mensaje = "Seleccione un pago válido para editar.";
                return false;
            }

            if (pago.IdVenta <= 0)
            {
                mensaje = "Seleccione una venta válida.";
                return false;
            }

            if (pago.Monto <= 0)
            {
                mensaje = "El monto debe ser mayor a cero.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(pago.MetodoPago) || pago.MetodoPago.Length > 30)
            {
                mensaje = "Seleccione un método de pago válido.";
                return false;
            }

            try
            {
                decimal saldoDisponible = _datos.MtdObtenerSaldoParaEditar(pago.IdVenta, pago.IdPago);
                if (saldoDisponible <= 0)
                {
                    mensaje = "La venta seleccionada no tiene saldo disponible para este pago.";
                    return false;
                }

                if (pago.Monto > saldoDisponible)
                {
                    mensaje = "El monto supera el saldo disponible de la venta seleccionada (Q " +
                              saldoDisponible.ToString("N2") + ").";
                    return false;
                }

                bool actualizado = _datos.MtdActualizar(pago);
                mensaje = actualizado
                    ? "Pago actualizado correctamente."
                    : "No se encontró el pago que intentas actualizar.";
                return actualizado;
            }
            catch
            {
                mensaje = "No se pudo actualizar el pago. Verifica la conexión y los datos e inténtalo de nuevo.";
                return false;
            }
        }

        // ELIMINAR: MtdEliminar
        public bool MtdEliminar(int idPago, out string mensaje)
        {
            if (idPago <= 0)
            {
                mensaje = "Seleccione un pago válido para eliminar.";
                return false;
            }

            try
            {
                bool eliminado = _datos.MtdEliminar(idPago);
                mensaje = eliminado
                    ? "Pago eliminado correctamente."
                    : "No se encontró el pago que intentas eliminar.";
                return eliminado;
            }
            catch
            {
                mensaje = "No se pudo eliminar el pago. Verifica la conexión y vuelve a intentarlo.";
                return false;
            }
        }
    }
}
