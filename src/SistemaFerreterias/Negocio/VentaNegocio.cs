using System;
using System.Collections.Generic;
using System.Linq;
using Datos;
using Entidad;

namespace Negocio
{
    public class VentaNegocio
    {
        private readonly VentaDatos _datos = new VentaDatos();

        public List<ItemCombo> ListarSucursales()
        {
            return _datos.ListarSucursales();
        }

        public List<ProductoVenta> ListarProductos(int idSucursal)
        {
            return _datos.ListarProductos(idSucursal);
        }

        // Devuelve "" si todo salió bien, o el mensaje de error.
        public string Guardar(Venta v)
        {
            if (v.IdSucursal <= 0)
                return "Selecciona una sucursal.";

            if (v.IdCliente <= 0)
                return "Selecciona un cliente.";

            if (v.Detalles == null || v.Detalles.Count == 0)
                return "Agrega al menos un producto a la venta.";

            if (string.IsNullOrWhiteSpace(v.MetodoPago))
                return "Selecciona el método de pago.";

            if (v.Detalles.Any(d => d.Cantidad <= 0))
                return "Todas las cantidades deben ser mayores a cero.";

            v.Fecha = DateTime.Now;
            v.Total = v.Detalles.Sum(d => d.Subtotal);

            try
            {
                v.IdVenta = _datos.Guardar(v);
                return "";
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;   // por ejemplo: stock insuficiente
            }
            catch
            {
                return "No se pudo guardar la venta. Verifica los datos e inténtalo de nuevo.";
            }
        }
    }
}