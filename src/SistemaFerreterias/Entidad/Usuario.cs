using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    // Guarda los datos de una cuenta y su rol para moverlos entre capas.
    public class Usuario
    {
        // Corresponde a id_usuario en Tbl_Usuarios.
        public int IdUsuario { get; set; }
        // Corresponde a nombre_usuario en Tbl_Usuarios.
        public string NombreUsuario { get; set; }
        // Corresponde a contrasena en Tbl_Usuarios. Se conserva el formato actual.
        public string Contrasena { get; set; }
        // Corresponde a rol en Tbl_Usuarios.
        public string Rol { get; set; }
        // Indica si la cuenta puede iniciar sesión.
        public bool Estado { get; set; }
    }
}
