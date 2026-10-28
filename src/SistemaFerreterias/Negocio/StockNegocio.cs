using Datos;
using Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    // CLASE StockNegocio: Coordina operaciones y aplica reglas antes de llamar a Datos.
    public class StockNegocio
    {
        private readonly StockDatos _stockDatos = new StockDatos();

        // CONSULTAR: MtdConsultar
        public List<StockEntidad> MtdConsultar()
        {
            return _stockDatos.MtdConsultar();
        }

        // BUSCAR: MtdBuscar
        public List<StockEntidad> MtdBuscar(string filtro)
        {
            return _stockDatos.MtdBuscar(filtro == null ? string.Empty : filtro.Trim());
        }

        // CONSULTAR: MtdConsultarBajo
        public List<StockEntidad> MtdConsultarBajo()
        {
            return _stockDatos.MtdConsultarBajo();
        }

        // VALIDAR: MtdValidar
        private void MtdValidar(StockEntidad stock)
        {
            if (stock.CodigoSucursal <= 0)
            {
                throw new Exception("Selecciona una sucursal.");
            }

            if (stock.CodigoProducto <= 0)
            {
                throw new Exception("Selecciona un producto.");
            }

            if (stock.Cantidad < 0)
            {
                throw new Exception("La cantidad no puede ser negativa.");
            }

            if (stock.StockMinimo < 0)
            {
                throw new Exception("El stock minimo no puede ser negativo.");
            }
        }

        // AGREGAR: MtdAgregar
        public bool MtdAgregar(StockEntidad stock)
        {
            MtdValidar(stock);
            return _stockDatos.MtdAgregar(stock);
        }

        // EDITAR: MtdEditar
        public bool MtdEditar(StockEntidad stock)
        {
            if (stock.CodigoStock <= 0)
            {
                throw new Exception("Selecciona un registro de stock para editar.");
            }

            MtdValidar(stock);
            return _stockDatos.MtdEditar(stock);
        }

        // ELIMINAR: MtdEliminar
        public bool MtdEliminar(int codigoStock)
        {
            if (codigoStock <= 0)
            {
                throw new Exception("Selecciona un registro de stock para eliminar.");
            }

            return _stockDatos.MtdEliminar(codigoStock);
        }
    }
}
