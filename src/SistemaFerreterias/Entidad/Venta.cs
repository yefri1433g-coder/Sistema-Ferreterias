using System;
using System.Collections.Generic;

namespace Entidad
{
    // CLASE Venta: Representa datos del sistema y permite compartirlos entre las capas.
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public int IdSucursal { get; set; }   // se guarda en la venta y se usa para el inventario
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }

    // CLASE DetalleVenta: Representa datos del sistema y permite compartirlos entre las capas.
    public class DetalleVenta
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get { return Cantidad * PrecioUnitario; } }
    }

    // Producto con su existencia en la sucursal elegida (para el combo)
    // CLASE ProductoVenta: Representa datos del sistema y permite compartirlos entre las capas.
    public class ProductoVenta
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Existencia { get; set; }
        public string Texto
        {
            get { return Codigo + " - " + Nombre + "   (Q " + PrecioVenta.ToString("N2") + " | Stock: " + Existencia + ")"; }
        }
    }

    // Elemento sencillo para combos (id + texto), por ejemplo las sucursales
    // CLASE ItemCombo: Representa datos del sistema y permite compartirlos entre las capas.
    public class ItemCombo
    {
        public int Id { get; set; }
        public string Texto { get; set; }
    }
}
