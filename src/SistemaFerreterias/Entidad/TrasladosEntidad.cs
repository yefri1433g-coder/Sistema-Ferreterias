using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class TrasladosEntidad
    {
        /*  ==================================================
            POO: ENCASULAMIENTO
            ================================================== */

        private int _codigoTraslado;
        private int _codigoEmpleado;
        private string _nombreEmpleado;
        private int _sucursalOrigen;
        private string _nombreOrigen;
        private int _sucursalDestino;
        private string _nombreDestino;
        private DateTime _fechaTraslado;
        private string _estado;

        /*  ==================================================
            POO: PROPIEDADES
            ================================================== */

        public int CodigoTraslado
        {
            get { return _codigoTraslado; }
            set { _codigoTraslado = value; }
        }
        public int CodigoEmpleado
        {
            get { return _codigoEmpleado; }
            set { _codigoEmpleado = value; }
        }
        public string NombreEmpleado
        {
            get { return _nombreEmpleado; }
            set { _nombreEmpleado = value; }
        }
        public int SucursalOrigen
        {
            get { return _sucursalOrigen; }
            set { _sucursalOrigen = value; }
        }
        public string NombreOrigen
        {
            get { return _nombreOrigen; }
            set { _nombreOrigen = value; }
        }
        public int SucursalDestino
        {
            get { return _sucursalDestino; }
            set { _sucursalDestino = value; }
        }
        public string NombreDestino
        {
            get { return _nombreDestino; }
            set { _nombreDestino = value; }
        }
        public DateTime FechaTraslado
        {
            get { return _fechaTraslado; }
            set { _fechaTraslado = value; }
        }
        public string Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }

        // Detalle del traslado (productos y cantidades)
        private List<DetalleTrasladoEntidad> _detalles = new List<DetalleTrasladoEntidad>();

        [Browsable(false)]
        public List<DetalleTrasladoEntidad> Detalles
        {
            get { return _detalles; }
            set { _detalles = value; }
        }

        /*  ==================================================
            POO: CONTRUCTORES
            ================================================== */

        // Contructor vacio
        public TrasladosEntidad()
        {
            _codigoTraslado = 0;
            _codigoEmpleado = 0;
            _nombreEmpleado = string.Empty;
            _sucursalOrigen = 0;
            _nombreOrigen = string.Empty;
            _sucursalDestino = 0;
            _nombreDestino = string.Empty;
            _fechaTraslado = DateTime.Now;
            _estado = string.Empty;
        }

        // Contructor Editar, Consultar y Buscar
        public TrasladosEntidad
        (
           int codigoTraslado,
           int codigoEmpleado,
           string nombreEmpleado,
           int sucursalOrigen,
           string nombreOrigen,
           int sucursalDestino,
           string nombreDestino,
           DateTime fechaTraslado,
           string estado
        )
        {
            _codigoTraslado = codigoTraslado;
            _codigoEmpleado = codigoEmpleado;
            _nombreEmpleado = nombreEmpleado;
            _sucursalOrigen = sucursalOrigen;
            _nombreOrigen = nombreOrigen;
            _sucursalDestino = sucursalDestino;
            _nombreDestino = nombreDestino;
            _fechaTraslado = fechaTraslado;
            _estado = estado;
        }

        // Contructor Agregar
        public TrasladosEntidad
        (
           int codigoEmpleado,
           int sucursalOrigen,
           int sucursalDestino,
           DateTime fechaTraslado,
           string estado
        )
        {
            _codigoEmpleado = codigoEmpleado;
            _sucursalOrigen = sucursalOrigen;
            _sucursalDestino = sucursalDestino;
            _fechaTraslado = fechaTraslado;
            _estado = estado;
        }

    }
}