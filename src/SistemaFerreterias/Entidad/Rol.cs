using System.Collections.Generic;

namespace Entidad
{
    // Representa un rol y los módulos que tiene permitidos.
    // CLASE Rol: Representa datos del sistema y permite compartirlos entre las capas.
    public class Rol
    {
        // Llave primaria CodigoRol de Tbl_Roles.
        public int CodigoRol { get; set; }
        // Nombre del rol asignado a Tbl_Usuarios.rol.
        public string NombreRol { get; set; }
        public bool Estado { get; set; }
        // NombreModulo de Tbl_RolesAccesos.
        public List<string> Modulos { get; set; } = new List<string>();
    }
}
