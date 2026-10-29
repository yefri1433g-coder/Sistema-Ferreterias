using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    // CLASE StockEntidad: Representa datos del sistema y permite compartirlos entre las capas.
    public class StockEntidad
    {
        /*  ==================================================
            POO: ENCASULAMIENTO
            ================================================== */

        private int _codigoStock;
        private int _codigoSucursal;
        private string _nombreSucursal;
        private int _codigoProducto;
        private string _nombreProducto;
        private int _cantidad;
        private int _stockMinimo;

        /*  ==================================================
            POO: PROPIEDADES
            ================================================== */

        public int CodigoStock
        {
            get { return _codigoStock; }
            set { _codigoStock = value; }
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
        public int StockMinimo
        {
            get { return _stockMinimo; }
            set { _stockMinimo = value; }
        }

        /*  ==================================================
            POO: CONTRUCTORES
            ================================================== */

        // Contructor vacio
        public StockEntidad()
        {
            _codigoStock = 0;
            _codigoSucursal = 0;
            _nombreSucursal = string.Empty;
            _codigoProducto = 0;
            _nombreProducto = string.Empty;
            _cantidad = 0;
            _stockMinimo = 0;
        }

        // Contructor Editar, Consultar y Buscar
        public StockEntidad
        (
           int codigoStock,
           int codigoSucursal,
           string nombreSucursal,
           int codigoProducto,
           string nombreProducto,
           int cantidad,
           int stockMinimo
        )
        {
            _codigoStock = codigoStock;
            _codigoSucursal = codigoSucursal;
            _nombreSucursal = nombreSucursal;
            _codigoProducto = codigoProducto;
            _nombreProducto = nombreProducto;
            _cantidad = cantidad;
            _stockMinimo = stockMinimo;
        }

        // Contructor Agregar
        public StockEntidad
        (
           int codigoSucursal,
           int codigoProducto,
           int cantidad,
           int stockMinimo
        )
        {
            _codigoSucursal = codigoSucursal;
            _codigoProducto = codigoProducto;
            _cantidad = cantidad;
            _stockMinimo = stockMinimo;
        }

    }
}