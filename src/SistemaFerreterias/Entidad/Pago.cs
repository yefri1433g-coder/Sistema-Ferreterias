using System;

namespace Entidad
{
    // CLASE Pago: Representa datos del sistema y permite compartirlos entre las capas.
    public class Pago
    {
        public int IdPago { get; set; }
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
    }
}
