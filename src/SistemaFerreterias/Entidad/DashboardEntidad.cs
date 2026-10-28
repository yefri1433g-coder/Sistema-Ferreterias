using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    // CLASE DashboardEntidad: Representa datos del sistema y permite compartirlos entre las capas.
    public class DashboardEntidad
    {
        private int _totalProductos;
        private int _totalSucursales;
        private int _totalProveedores;
        private int _stockBajo;
        private int _trasladosPendientes;
        public decimal VentasHoy { get; set; }
        public int TotalClientes { get; set; }
        public List<KeyValuePair<string, decimal>> VentasUltimosSieteDias { get; set; } = new List<KeyValuePair<string, decimal>>();
        public List<KeyValuePair<string, int>> EstadoInventario { get; set; } = new List<KeyValuePair<string, int>>();
        public List<ActividadDashboard> ActividadReciente { get; set; } = new List<ActividadDashboard>();
        public List<string> ErroresConsulta { get; set; } = new List<string>();

        public int TotalProductos
        {
            get { return _totalProductos; }
            set { _totalProductos = value; }
        }
        public int TotalSucursales
        {
            get { return _totalSucursales; }
            set { _totalSucursales = value; }
        }
        public int TotalProveedores
        {
            get { return _totalProveedores; }
            set { _totalProveedores = value; }
        }
        public int StockBajo
        {
            get { return _stockBajo; }
            set { _stockBajo = value; }
        }
        public int TrasladosPendientes
        {
            get { return _trasladosPendientes; }
            set { _trasladosPendientes = value; }
        }

        private List<KeyValuePair<string, int>> _stockPorSucursal = new List<KeyValuePair<string, int>>();
        private List<KeyValuePair<string, int>> _trasladosPorEstado = new List<KeyValuePair<string, int>>();

        public List<KeyValuePair<string, int>> StockPorSucursal
        {
            get { return _stockPorSucursal; }
            set { _stockPorSucursal = value; }
        }
        public List<KeyValuePair<string, int>> TrasladosPorEstado
        {
            get { return _trasladosPorEstado; }
            set { _trasladosPorEstado = value; }
        }

        public DashboardEntidad()
        {
            _totalProductos = 0;
            _totalSucursales = 0;
            _totalProveedores = 0;
            _stockBajo = 0;
            _trasladosPendientes = 0;
        }

        public DashboardEntidad
        (
           int totalProductos,
           int totalSucursales,
           int totalProveedores,
           int stockBajo,
           int trasladosPendientes
        )
        {
            _totalProductos = totalProductos;
            _totalSucursales = totalSucursales;
            _totalProveedores = totalProveedores;
            _stockBajo = stockBajo;
            _trasladosPendientes = trasladosPendientes;
        }

    }

    // CLASE ActividadDashboard: Representa datos del sistema y permite compartirlos entre las capas.
    public class ActividadDashboard
    {
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public string Referencia { get; set; }
        public DateTime Fecha { get; set; }
    }
}
