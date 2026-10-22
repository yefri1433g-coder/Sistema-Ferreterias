using System;
using System.Collections.Generic;
using Datos;
using Entidad;

namespace Negocio
{
    public class ClienteNegocio
    {
        private readonly ClienteDatos _datos = new ClienteDatos();
        // El conjunto de ejemplo conservado corresponde a los IDs del 1 al 5.
        private const int UltimoIdClienteOriginal = 5;

        public List<Cliente> Listar()
        {
            return _datos.Listar();
        }

        // Si el criterio está vacío, devuelve todos los clientes.
        public List<Cliente> Buscar(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return _datos.Listar();

            return _datos.BuscarPorNombre(nombre.Trim());
        }

        // Alta independiente para mantener clara la operación Agregar.
        public string Agregar(Cliente cliente)
        {
            if (cliente == null)
                return "No se recibieron los datos del cliente.";

            if (cliente.IdCliente != 0)
                return "Para agregar un cliente, inicia un registro nuevo.";

            return Guardar(cliente);
        }

        // Devuelve "" si todo salió bien, o el mensaje de error.
        public string Guardar(Cliente c)
        {
            if (string.IsNullOrWhiteSpace(c.Nombre))
                return "Escribe el nombre del cliente.";

            c.Nombre = c.Nombre.Trim();
            c.Nit = (c.Nit ?? "").Trim();
            c.Telefono = (c.Telefono ?? "").Trim();
            c.Direccion = (c.Direccion ?? "").Trim();

            if (c.Nombre.Length > 100)
                return "El nombre no puede pasar de 100 caracteres.";

            if (c.Nit.Length > 15)
                return "El NIT no puede pasar de 15 caracteres.";

            if (c.Telefono.Length > 15)
                return "El teléfono no puede pasar de 15 caracteres.";

            if (c.Direccion.Length > 150)
                return "La dirección no puede pasar de 150 caracteres.";

            // El NIT no se repite, excepto "CF" (consumidor final) o vacío.
            bool esCF = c.Nit.Equals("CF", StringComparison.OrdinalIgnoreCase);
            if (c.Nit != "" && !esCF && _datos.ExisteNit(c.Nit, c.IdCliente))
                return "Ya existe un cliente con ese NIT.";

            if (c.IdCliente == 0)
                _datos.Insertar(c);
            else
                _datos.Actualizar(c);

            return "";
        }

        // Valida los datos y actualiza únicamente el cliente identificado por su PK.
        public string Editar(Cliente cliente)
        {
            if (cliente == null)
                return "No se recibieron los datos del cliente.";

            if (cliente.IdCliente <= 0)
                return "Selecciona un cliente válido para editar.";

            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                return "Escribe el nombre del cliente.";

            cliente.Nombre = cliente.Nombre.Trim();
            cliente.Nit = (cliente.Nit ?? "").Trim();
            cliente.Telefono = (cliente.Telefono ?? "").Trim();
            cliente.Direccion = (cliente.Direccion ?? "").Trim();

            if (cliente.Nombre.Length > 100)
                return "El nombre no puede pasar de 100 caracteres.";
            if (cliente.Nit.Length > 15)
                return "El NIT no puede pasar de 15 caracteres.";
            if (cliente.Telefono.Length > 15)
                return "El teléfono no puede pasar de 15 caracteres.";
            if (cliente.Direccion.Length > 150)
                return "La dirección no puede pasar de 150 caracteres.";

            bool esCF = cliente.Nit.Equals("CF", StringComparison.OrdinalIgnoreCase);
            if (cliente.Nit != "" && !esCF && _datos.ExisteNit(cliente.Nit, cliente.IdCliente))
                return "Ya existe un cliente con ese NIT.";

            _datos.Actualizar(cliente);
            return "";
        }

        public void Eliminar(int idCliente)
        {
            if (idCliente <= 0)
                throw new InvalidOperationException("Selecciona un cliente para eliminar.");

            _datos.Eliminar(idCliente);
        }

        public int EliminarClientesDePrueba()
        {
            return _datos.EliminarPosterioresA(UltimoIdClienteOriginal);
        }
    }
}
