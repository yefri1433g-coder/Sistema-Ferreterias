using System;
using System.Data;
using Datos;
using Entidad;

namespace Negocio
{
    public class PagoNegocio
    {
        private readonly PagoDatos _datos = new PagoDatos();

        public DataTable MtdListarVentas()
        {
            return _datos.MtdListarVentas();
        }

        public DataTable MtdListarPagosRecientes()
        {
            return _datos.MtdListarPagosRecientes();
        }

        public DataTable MtdListarPagos(int idVenta)
        {
            return _datos.MtdListarPagosPorVenta(idVenta);
        }

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

            try
            {
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
    }
}
