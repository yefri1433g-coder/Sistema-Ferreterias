using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Datos;
using Entidad;

namespace Negocio
{
    public class UsuarioNegocio
    {
        private readonly UsuarioDatos _datos = new UsuarioDatos();
        // Se conservan los cinco usuarios de ejemplo y la cuenta admin.
        private const int UltimoIdUsuarioOriginal = 6;

        public Usuario Login(string nombreUsuario, string contrasena, out string mensaje)
        {
            mensaje = "";

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                mensaje = "Escribe tu usuario.";
                return null;
            }

            if (string.IsNullOrWhiteSpace(contrasena))
            {
                mensaje = "Escribe tu contraseña.";
                return null;
            }

            Usuario usuario = _datos.Validar(nombreUsuario.Trim(), contrasena);

            if (usuario == null)
                mensaje = "Usuario o contraseña incorrectos.";

            return usuario;
        }

        public List<Usuario> Listar()
        {
            return _datos.Listar();
        }

        public List<Usuario> Buscar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return _datos.Listar();

            return _datos.Buscar(texto.Trim());
        }

        public string Agregar(Usuario u)
        {
            if (u == null)
                return "No se recibieron los datos del usuario.";

            if (string.IsNullOrWhiteSpace(u.NombreUsuario))
                return "Escribe el nombre de usuario.";

            if (string.IsNullOrWhiteSpace(u.Contrasena))
                return "Escribe la contraseña.";

            if (string.IsNullOrWhiteSpace(u.Rol))
                return "Selecciona un rol.";

            u.NombreUsuario = u.NombreUsuario.Trim();

            if (_datos.ExisteNombre(u.NombreUsuario, 0))
                return "Ese nombre de usuario ya existe.";

            _datos.Insertar(u);
            return "";
        }

        public string Editar(Usuario u)
        {
            if (u == null)
                return "No se recibieron los datos del usuario.";

            if (u.IdUsuario <= 0)
                return "Selecciona un usuario para editar.";

            if (string.IsNullOrWhiteSpace(u.NombreUsuario))
                return "Escribe el nombre de usuario.";

            if (string.IsNullOrWhiteSpace(u.Contrasena))
                return "Escribe la contraseña.";

            if (string.IsNullOrWhiteSpace(u.Rol))
                return "Selecciona un rol.";

            u.NombreUsuario = u.NombreUsuario.Trim();

            if (_datos.ExisteNombre(u.NombreUsuario, u.IdUsuario))
                return "Ese nombre de usuario ya existe.";

            _datos.Actualizar(u);
            return "";
        }

        public string Guardar(Usuario u)
        {
            if (string.IsNullOrWhiteSpace(u.NombreUsuario))
                return "Escribe el nombre de usuario.";

            if (string.IsNullOrWhiteSpace(u.Contrasena))
                return "Escribe la contraseña.";

            if (string.IsNullOrWhiteSpace(u.Rol))
                return "Selecciona un rol.";

            u.NombreUsuario = u.NombreUsuario.Trim();

            if (_datos.ExisteNombre(u.NombreUsuario, u.IdUsuario))
                return "Ese nombre de usuario ya existe.";

            if (u.IdUsuario == 0)
                _datos.Insertar(u);
            else
                _datos.Actualizar(u);

            return "";
        }

        public void Eliminar(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new InvalidOperationException("Selecciona un usuario para eliminar.");

            _datos.Eliminar(idUsuario);
        }

        public int LimpiarUsuariosDePrueba()
        {
            return _datos.EliminarUsuariosDePrueba(UltimoIdUsuarioOriginal);
        }
    }


}
