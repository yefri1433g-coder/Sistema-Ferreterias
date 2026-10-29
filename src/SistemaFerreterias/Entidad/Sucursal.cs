namespace Entidad
{
    // Representa una sucursal para transportar sus datos entre capas.
    // CLASE Sucursal: Representa datos del sistema y permite compartirlos entre las capas.
    public class Sucursal
    {
        public int IdSucursal { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; }
    }
}
