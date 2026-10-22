using System;
using System.Collections.Generic;
using Datos;
using Entidad;

namespace Negocio
{
    public class EmpleadoNegocio
    {
        private readonly EmpleadoDatos _datos = new EmpleadoDatos();

        public List<Empleado> Listar()
        {
            return _datos.Listar();
        }

        public List<Empleado> Buscar(string criterio)
        {
            return _datos.Buscar(criterio);
        }

        public List<ComboItemEntidad> ListarUsuariosActivos()
        {
            return _datos.ListarUsuariosActivos();
        }

        public List<ComboItemEntidad> ListarSucursalesActivas()
        {
            return _datos.ListarSucursalesActivas();
        }

        public void Agregar(Empleado empleado)
        {
            ValidarEmpleado(empleado);
            _datos.Agregar(empleado);
        }

        public void Editar(Empleado empleado)
        {
            ValidarEmpleado(empleado);
            if (empleado.IdEmpleado <= 0)
                throw new InvalidOperationException("Selecciona un empleado válido para editar.");

            _datos.Editar(empleado);
        }

        public void Eliminar(int idEmpleado)
        {
            if (idEmpleado <= 0)
                throw new InvalidOperationException("Selecciona un empleado válido para eliminar.");

            _datos.Eliminar(idEmpleado);
        }

        private static void ValidarEmpleado(Empleado empleado)
        {
            if (empleado == null)
                throw new ArgumentNullException("empleado", "No se recibieron los datos del empleado.");
            if (empleado.IdUsuario <= 0)
                throw new InvalidOperationException("Selecciona un usuario válido.");
            if (empleado.IdSucursal <= 0)
                throw new InvalidOperationException("Selecciona una sucursal válida.");

            empleado.Nombre = (empleado.Nombre ?? "").Trim();
            empleado.Apellido = (empleado.Apellido ?? "").Trim();
            empleado.Cargo = (empleado.Cargo ?? "").Trim();
            empleado.Telefono = (empleado.Telefono ?? "").Trim();

            if (empleado.Nombre.Length == 0 || empleado.Apellido.Length == 0 || empleado.Cargo.Length == 0)
                throw new InvalidOperationException("Nombre, apellido y cargo son obligatorios.");
            if (empleado.Nombre.Length > 60)
                throw new InvalidOperationException("El nombre no puede superar 60 caracteres.");
            if (empleado.Apellido.Length > 60)
                throw new InvalidOperationException("El apellido no puede superar 60 caracteres.");
            if (empleado.Cargo.Length > 50)
                throw new InvalidOperationException("El cargo no puede superar 50 caracteres.");
            if (empleado.Telefono.Length > 15)
                throw new InvalidOperationException("El teléfono no puede superar 15 caracteres.");

        }
    }
}
