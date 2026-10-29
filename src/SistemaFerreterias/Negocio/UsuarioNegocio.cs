using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Datos;
using Entidad;

namespace Negocio
{
    // CLASE UsuarioNegocio: Coordina operaciones y aplica reglas antes de llamar a Datos.
    public class UsuarioNegocio
    {
        private readonly UsuarioDatos _datos = new UsuarioDatos();
        private readonly RolNegocio _roles = new RolNegocio();
        // Se conservan los cinco usuarios de ejemplo y la cuenta admin.
        private const int UltimoIdUsuarioOriginal = 6;

        // Valida los campos y coordina la consulta de acceso con la capa Datos.
        // INICIO DE SESIÓN: Login
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

            Usuario usuario;
            try
            {
                // Pide a Datos validar la cuenta y devolver su rol.
                usuario = _datos.Validar(nombreUsuario.Trim(), contrasena);
            }
            catch (Exception ex)
            {
                mensaje = "No fue posible validar el acceso. " + ex.Message;
                return null;
            }

            if (usuario == null)
                mensaje = "Usuario o contraseña incorrectos.";

            return usuario;
        }

        // CONSULTAR: Listar
        public List<Usuario> Listar()
        {
            return _datos.Listar();
        }

        // CONSULTAR: ListarRoles
        public List<string> ListarRoles()
        {
            // Devuelve los roles activos que se pueden asignar a usuarios.
            return _roles.Listar().Select(r => r.NombreRol).ToList();
        }

        // BUSCAR: Buscar
        public List<Usuario> Buscar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return _datos.Listar();

            return _datos.Buscar(texto.Trim());
        }

        // AGREGAR: Agregar
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

        // EDITAR: Editar
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

        // GUARDAR: Guardar
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

        // ELIMINAR: Eliminar
        public void Eliminar(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new InvalidOperationException("Selecciona un usuario para eliminar.");

            _datos.Eliminar(idUsuario);
        }

        // LIMPIAR: LimpiarUsuariosDePrueba
        public int LimpiarUsuariosDePrueba()
        {
            return _datos.EliminarUsuariosDePrueba(UltimoIdUsuarioOriginal);
        }
    }


}
