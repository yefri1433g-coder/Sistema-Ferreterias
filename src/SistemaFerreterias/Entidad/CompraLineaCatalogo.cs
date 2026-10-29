using System;

namespace Entidad
{
    // CLASE CompraLineaCatalogo: Representa datos del sistema y permite compartirlos entre las capas.
    public sealed class CompraLineaCatalogo
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Subtotal { get { return Math.Round(Cantidad * CostoUnitario, 2); } }
    }
}