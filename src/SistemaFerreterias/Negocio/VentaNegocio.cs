using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Datos;
using Entidad;

namespace Negocio
{
    public class VentaNegocio
    {
        private readonly VentaDatos _datos = new VentaDatos();

        public DataTable ListarVentas()
        {
            return _datos.ListarVentas();
        }

        public List<ItemCombo> ListarSucursales()
        {
            return _datos.ListarSucursales();
        }

        public List<ProductoVenta> ListarProductos(int idSucursal)
        {
            return _datos.ListarProductos(idSucursal);
        }

        public Venta Obtener(int idVenta)
        {
            if (idVenta <= 0) return null;
            return _datos.Obtener(idVenta);
        }

        public Venta Consultar(int idVenta)
        {
            if (idVenta <= 0)
                throw new ArgumentException("Selecciona una venta válida.", "idVenta");
            try
            {
                return _datos.Consultar(idVenta);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("No se pudo cargar la consulta de la venta.", ex);
            }
        }

        public string ActualizarPendiente(Venta venta)
        {
            string error = ValidarVenta(venta);
            if (error != "") return error;
            if (venta.IdVenta <= 0) return "Selecciona una venta válida para editar.";

            venta.Total = venta.Detalles.Sum(d => d.Subtotal);
            try
            {
                return _datos.ActualizarPendiente(venta)
                    ? ""
                    : "No se encontró la venta que intentas actualizar.";
            }
            catch (InvalidOperationException ex) { return ex.Message; }
            catch { return "No se pudo actualizar la venta. Verifica los datos e inténtalo de nuevo."; }
        }

        public string EliminarPendiente(int idVenta, int idSucursal)
        {
            if (idVenta <= 0) return "Selecciona una venta válida para eliminar.";
            if (idSucursal <= 0) return "Selecciona la sucursal donde se registró originalmente la venta.";
            try
            {
                return _datos.EliminarPendiente(idVenta, idSucursal)
                    ? ""
                    : "No se encontró la venta que intentas eliminar.";
            }
            catch (InvalidOperationException ex) { return ex.Message; }
            catch { return "No se pudo eliminar la venta. Verifica los datos e inténtalo de nuevo."; }
        }

        private static string ValidarVenta(Venta venta)
        {
            if (venta == null) return "No se recibió la información de la venta.";
            if (venta.IdSucursal <= 0) return "Selecciona la sucursal donde se registró originalmente la venta.";
            if (venta.IdCliente <= 0) return "Selecciona un cliente.";
            if (venta.Detalles == null || venta.Detalles.Count == 0) return "Agrega al menos un producto a la venta.";
            if (venta.Detalles.Any(d => d == null || d.IdProducto <= 0 || d.Cantidad <= 0 || d.PrecioUnitario <= 0))
                return "Revisa los productos, cantidades y precios de la venta.";
            if (venta.Detalles.Select(d => d.IdProducto).Distinct().Count() != venta.Detalles.Count)
                return "Cada producto debe aparecer una sola vez en el detalle.";
            return "";
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
