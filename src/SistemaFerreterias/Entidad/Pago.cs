using System;

namespace Entidad
{
    public class Pago
    {
        public int IdPago { get; set; }
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
    }
}
