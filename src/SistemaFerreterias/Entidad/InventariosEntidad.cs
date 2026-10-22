using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class InventariosEntidad
    {
        /*  ==================================================
            POO: ENCASULAMIENTO
            ================================================== */

        private int _codigoInventario;
        private int _codigoSucursal;
        private string _nombreSucursal;
        private DateTime _fechaInventario;
        private string _descripcion;

        /*  ==================================================
            POO: PROPIEDADES
            ================================================== */

        public int CodigoInventario
        {
            get { return _codigoInventario; }
            set { _codigoInventario = value; }
        }
        public int CodigoSucursal
        {
            get { return _codigoSucursal; }
            set { _codigoSucursal = value; }
        }
        public string NombreSucursal
        {
            get { return _nombreSucursal; }
            set { _nombreSucursal = value; }
        }
        public DateTime FechaInventario
        {
            get { return _fechaInventario; }
            set { _fechaInventario = value; }
        }
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        /*  ==================================================
            POO: CONTRUCTORES
            ================================================== */

        // Contructor vacio
        public InventariosEntidad()
        {
            _codigoInventario = 0;
            _codigoSucursal = 0;
            _nombreSucursal = string.Empty;
            _fechaInventario = DateTime.Now;
            _descripcion = string.Empty;
        }

        // Contructor Editar, Consultar y Buscar
        public InventariosEntidad
        (
           int codigoInventario,
           int codigoSucursal,
           string nombreSucursal,
           DateTime fechaInventario,
           string descripcion
        )
        {
            _codigoInventario = codigoInventario;
            _codigoSucursal = codigoSucursal;
            _nombreSucursal = nombreSucursal;
            _fechaInventario = fechaInventario;
            _descripcion = descripcion;
        }

        // Contructor Agregar
        public InventariosEntidad
        (
           int codigoSucursal,
           DateTime fechaInventario,
           string descripcion
        )
        {
            _codigoSucursal = codigoSucursal;
            _fechaInventario = fechaInventario;
            _descripcion = descripcion;
        }

    }
}