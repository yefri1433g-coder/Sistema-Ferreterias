using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    // CLASE DetalleTrasladoEntidad: Representa datos del sistema y permite compartirlos entre las capas.
    public class DetalleTrasladoEntidad
    {
        /*  ==================================================
            POO: ENCASULAMIENTO
            ================================================== */

        private int _codigoProducto;
        private string _nombreProducto;
        private int _cantidad;

        /*  ==================================================
            POO: PROPIEDADES
            ================================================== */

        public int CodigoProducto
        {
            get { return _codigoProducto; }
            set { _codigoProducto = value; }
        }
        public string NombreProducto
        {
            get { return _nombreProducto; }
            set { _nombreProducto = value; }
        }
        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        /*  ==================================================
            POO: CONTRUCTORES
            ================================================== */

        // Contructor vacio
        public DetalleTrasladoEntidad()
        {
            _codigoProducto = 0;
            _nombreProducto = string.Empty;
            _cantidad = 0;
        }

        // Contructor Editar, Consultar y Buscar
        public DetalleTrasladoEntidad
        (
           int codigoProducto,
           string nombreProducto,
           int cantidad
        )
        {
            _codigoProducto = codigoProducto;
            _nombreProducto = nombreProducto;
            _cantidad = cantidad;
        }

        // Contructor Agregar
        public DetalleTrasladoEntidad
        (
           int codigoProducto,
           int cantidad
        )
        {
            _codigoProducto = codigoProducto;
            _cantidad = cantidad;
        }

    }
}