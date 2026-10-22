using Datos;
using Entidad;
using System;
using System.Collections.Generic;
using System.Data;

namespace Negocio
{
    public class CatalogosGestionNegocio
    {
        private readonly CatalogosGestionDatos datos = new CatalogosGestionDatos();
        // El conjunto de ejemplo conservado corresponde a los IDs del 1 al 5.
        private const int UltimoIdSucursalOriginal = 5;
        public DataTable Categorias() { return datos.Categorias(); }
        public DataTable Productos() { return datos.Productos(); }
        public DataTable Sucursales() { return datos.Sucursales(); }
        public DataTable BuscarSucursales(string criterio)
        {
            return String.IsNullOrWhiteSpace(criterio) ? datos.Sucursales() : datos.BuscarSucursales(criterio.Trim());
        }
        public DataTable Proveedores() { return datos.Proveedores(); }
        public DataTable Compras() { return datos.Compras(); }
        public DataTable ProductosActivos() { return datos.ProductosActivos(); }
        public DataTable SucursalesActivas() { return datos.SucursalesActivas(); }
        public void GuardarCategoria(int? id, string nombre, string descripcion)
        {
            if (String.IsNullOrWhiteSpace(nombre)) throw new InvalidOperationException("Escribe el nombre de la categoría.");
            datos.GuardarCategoria(id, nombre, descripcion);
        }
        public void EliminarCategoria(int id) { datos.EliminarCategoria(id); }
        public void GuardarProducto(int? id, string codigo, string nombre, string descripcion, decimal costo, decimal precio, int categoria, bool activo)
        {
            if (String.IsNullOrWhiteSpace(codigo) || String.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("Código y nombre son obligatorios.");
            if (costo < 0 || precio < 0 || categoria <= 0) throw new InvalidOperationException("Selecciona una categoría y un precio válido.");
            datos.GuardarProducto(id, codigo, nombre, descripcion, costo, precio, categoria, activo);
        }
        public void EliminarProducto(int id) { datos.EliminarProducto(id); }
        public void GuardarSucursal(int? id, string nombre, string direccion, string telefono, bool activo)
        {
            if (String.IsNullOrWhiteSpace(nombre) || String.IsNullOrWhiteSpace(direccion))
                throw new InvalidOperationException("Nombre y dirección son obligatorios.");
            datos.GuardarSucursal(id, nombre, direccion, telefono, activo);
        }
        public void AgregarSucursal(Sucursal sucursal)
        {
            ValidarSucursal(sucursal);
            datos.AgregarSucursal(sucursal);
        }
        public void EditarSucursal(Sucursal sucursal)
        {
            ValidarSucursal(sucursal);
            if (sucursal.IdSucursal <= 0) throw new InvalidOperationException("Selecciona una sucursal válida para editar.");
            datos.EditarSucursal(sucursal);
        }
        private static void ValidarSucursal(Sucursal sucursal)
        {
            if (sucursal == null) throw new InvalidOperationException("No se recibieron los datos de la sucursal.");
            if (String.IsNullOrWhiteSpace(sucursal.Nombre)) throw new InvalidOperationException("Escribe el nombre de la sucursal.");
            if (String.IsNullOrWhiteSpace(sucursal.Direccion)) throw new InvalidOperationException("Escribe la dirección de la sucursal.");

            sucursal.Nombre = sucursal.Nombre.Trim();
            sucursal.Direccion = sucursal.Direccion.Trim();
            sucursal.Telefono = (sucursal.Telefono ?? "").Trim();

            if (sucursal.Nombre.Length > 80) throw new InvalidOperationException("El nombre no puede pasar de 80 caracteres.");
            if (sucursal.Direccion.Length > 150) throw new InvalidOperationException("La dirección no puede pasar de 150 caracteres.");
            if (sucursal.Telefono.Length > 15) throw new InvalidOperationException("El teléfono no puede pasar de 15 caracteres.");
        }
        public void EliminarSucursal(int id)
        {
            if (id <= 0) throw new InvalidOperationException("Selecciona una sucursal válida para eliminar.");
            datos.EliminarSucursal(id);
        }
        public int LimpiarSucursalesDePrueba()
        {
            return datos.EliminarSucursalesDePrueba(UltimoIdSucursalOriginal);
        }
        public int GuardarCompra(int proveedor, int sucursal, DateTime fecha, IList<CompraLineaCatalogo> detalles)
        {
            if (proveedor <= 0 || sucursal <= 0) throw new InvalidOperationException("Selecciona proveedor y sucursal.");
            foreach (CompraLineaCatalogo d in detalles)
                if (d.Cantidad <= 0 || d.CostoUnitario < 0) throw new InvalidOperationException("La cantidad debe ser mayor que cero y el costo no puede ser negativo.");
            return datos.GuardarCompra(proveedor, sucursal, fecha, detalles);
        }
    }
}
