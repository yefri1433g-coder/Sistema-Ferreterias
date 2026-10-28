using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    // CLASE KardexEntidad: Representa datos del sistema y permite compartirlos entre las capas.
    public class KardexEntidad
    {
        /*  ==================================================
            POO: ENCASULAMIENTO
            ================================================== */

        private int _codigoKardex;
        private int _codigoProducto;
        private string _nombreProducto;
        private DateTime _fechaMovimiento;
        private string _tipoMovimiento;
        private int _cantidad;
        private int _saldo;
        private string _descripcion;

        /*  ==================================================
            POO: PROPIEDADES
            ================================================== */

        public int CodigoKardex
        {
            get { return _codigoKardex; }
            set { _codigoKardex = value; }
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
        public DateTime FechaMovimiento
        {
            get { return _fechaMovimiento; }
            set { _fechaMovimiento = value; }
        }
        public string TipoMovimiento
        {
            get { return _tipoMovimiento; }
            set { _tipoMovimiento = value; }
        }
        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }
        public int Saldo
        {
            get { return _saldo; }
            set { _saldo = value; }
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
        public KardexEntidad()
        {
            _codigoKardex = 0;
            _codigoProducto = 0;
            _nombreProducto = string.Empty;
            _fechaMovimiento = DateTime.Now;
            _tipoMovimiento = string.Empty;
            _cantidad = 0;
            _saldo = 0;
            _descripcion = string.Empty;
        }

        // Contructor Editar, Consultar y Buscar
        public KardexEntidad
        (
           int codigoKardex,
           int codigoProducto,
           string nombreProducto,
           DateTime fechaMovimiento,
           string tipoMovimiento,
           int cantidad,
           int saldo,
           string descripcion
        )
        {
            _codigoKardex = codigoKardex;
            _codigoProducto = codigoProducto;
            _nombreProducto = nombreProducto;
            _fechaMovimiento = fechaMovimiento;
            _tipoMovimiento = tipoMovimiento;
            _cantidad = cantidad;
            _saldo = saldo;
            _descripcion = descripcion;
        }

        // Contructor Agregar
        public KardexEntidad
        (
           int codigoProducto,
           DateTime fechaMovimiento,
           string tipoMovimiento,
           int cantidad,
           int saldo,
           string descripcion
        )
        {
            _codigoProducto = codigoProducto;
            _fechaMovimiento = fechaMovimiento;
            _tipoMovimiento = tipoMovimiento;
            _cantidad = cantidad;
            _saldo = saldo;
            _descripcion = descripcion;
        }

    }
}