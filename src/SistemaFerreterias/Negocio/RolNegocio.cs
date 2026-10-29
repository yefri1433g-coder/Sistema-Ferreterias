using Datos;
using Entidad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocio
{
    // CLASE RolNegocio: Coordina operaciones y aplica reglas antes de llamar a Datos.
    public class RolNegocio
    {
        private readonly RolDatos _datos = new RolDatos();

        // Lista todos los roles o solo los que están activos.
        // CONSULTAR: Listar
        public List<Rol> Listar(bool soloActivos = true)
        {
            try { return _datos.Listar(soloActivos); }
            catch (Exception ex) { throw new InvalidOperationException("No se pudieron consultar los roles. " + ex.Message, ex); }
        }

        // Obtiene los módulos permitidos para el menú del usuario conectado.
        public List<string> ModulosPermitidos(string nombreRol)
        {
            try { return _datos.ListarModulos(nombreRol); }
            catch (Exception ex) { throw new InvalidOperationException("No se pudieron consultar los accesos del rol. " + ex.Message, ex); }
        }

        // Busca un rol y carga sus módulos asignados.
        // CONSULTAR: Obtener
        public Rol Obtener(string nombreRol)
        {
            try { return _datos.Obtener(nombreRol); }
            catch (Exception ex) { throw new InvalidOperationException("No se pudo consultar el rol. " + ex.Message, ex); }
        }

        // Valida el nombre y los módulos antes de guardar un rol.
        // GUARDAR: Guardar
        public void Guardar(Rol rol)
        {
            if (rol == null) throw new InvalidOperationException("No se recibieron los datos del rol.");
            rol.NombreRol = (rol.NombreRol ?? "").Trim();
            if (rol.NombreRol.Length == 0) throw new InvalidOperationException("Escribe el nombre del rol.");
            if (rol.NombreRol.Length > 30) throw new InvalidOperationException("El nombre del rol no puede superar 30 caracteres.");
            if (rol.Modulos == null) rol.Modulos = new List<string>();
            rol.Modulos = rol.Modulos.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            if (rol.Estado && !rol.Modulos.Contains("Dashboard"))
                throw new InvalidOperationException("Un rol activo debe tener acceso al Dashboard.");
            try { _datos.Guardar(rol); }
            catch (Exception ex) { throw new InvalidOperationException("No se pudo guardar el rol. " + ex.Message, ex); }
        }

        // Solicita borrar el rol seleccionado si no está asignado a usuarios.
        // ELIMINAR: Eliminar
        public void Eliminar(int codigoRol)
        {
            if (codigoRol <= 0) throw new InvalidOperationException("Selecciona un rol para eliminar.");
            try { _datos.Eliminar(codigoRol); }
            catch (Exception ex) { throw new InvalidOperationException("No se pudo eliminar el rol. " + ex.Message, ex); }
        }
    }
}
