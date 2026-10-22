namespace Entidad
{
    // Representa un empleado y los nombres relacionados que se muestran en Presentación.
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public int IdSucursal { get; set; }
        public string Sucursal { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cargo { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; }
    }
}
