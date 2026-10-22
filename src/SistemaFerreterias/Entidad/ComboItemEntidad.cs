using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class ComboItemEntidad
    {
        /*  ==================================================
            POO: ENCASULAMIENTO
            ================================================== */

        private int _codigo;
        private string _texto;

        /*  ==================================================
            POO: PROPIEDADES
            ================================================== */

        public int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }
        public string Texto
        {
            get { return _texto; }
            set { _texto = value; }
        }

        /*  ==================================================
            POO: CONTRUCTORES
            ================================================== */

        // Contructor vacio
        public ComboItemEntidad()
        {
            _codigo = 0;
            _texto = string.Empty;
        }

        // Contructor Editar, Consultar y Buscar
        public ComboItemEntidad
        (
           int codigo,
           string texto
        )
        {
            _codigo = codigo;
            _texto = texto;
        }

    }
}