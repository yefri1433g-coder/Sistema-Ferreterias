using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    // CLASE ProveedoresEntidad: Representa datos del sistema y permite compartirlos entre las capas.
    public class ProveedoresEntidad
    {
        /*  ==================================================
            POO: ENCASULAMIENTO
            ================================================== */

        private int _codigoProveedor;
        private string _nombreProveedor;
        private string _contacto;
        private string _telefono;
        private string _correo;
        private string _direccion;

        /*  ==================================================
            POO: PROPIEDADES
            ================================================== */

        public int CodigoProveedor
        {
            get { return _codigoProveedor; }
            set { _codigoProveedor = value; }
        }
        public string NombreProveedor
        {
            get { return _nombreProveedor; }
            set { _nombreProveedor = value; }
        }
        public string Contacto
        {
            get { return _contacto; }
            set { _contacto = value; }
        }
        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
        }
        public string Correo
        {
            get { return _correo; }
            set { _correo = value; }
        }
        public string Direccion
        {
            get { return _direccion; }
            set { _direccion = value; }
        }

        /*  ==================================================
            POO: CONTRUCTORES
            ================================================== */

        // Contructor vacio
        public ProveedoresEntidad()
        {
            _codigoProveedor = 0;
            _nombreProveedor = string.Empty;
            _contacto = string.Empty;
            _telefono = string.Empty;
            _correo = string.Empty;
            _direccion = string.Empty;
        }

        // Contructor Editar, Consultar y Buscar
        public ProveedoresEntidad
        (
           int codigoProveedor,
           string nombreProveedor,
           string contacto,
           string telefono,
           string correo,
           string direccion
        )
        {
            _codigoProveedor = codigoProveedor;
            _nombreProveedor = nombreProveedor;
            _contacto = contacto;
            _telefono = telefono;
            _correo = correo;
            _direccion = direccion;
        }

        // Contructor Agregar
        public ProveedoresEntidad
        (
           string nombreProveedor,
           string contacto,
           string telefono,
           string correo,
           string direccion
        )
        {
            _nombreProveedor = nombreProveedor;
            _contacto = contacto;
            _telefono = telefono;
            _correo = correo;
            _direccion = direccion;
        }

    }
}