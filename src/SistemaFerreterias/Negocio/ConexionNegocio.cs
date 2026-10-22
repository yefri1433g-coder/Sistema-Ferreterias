using Datos;

namespace Negocio
{
    public class ConexionNegocio
    {
        private readonly ConexionDatos _datos = new ConexionDatos();

        public void ProbarConexion()
        {
            _datos.MtdProbarConexion();
        }
    }
}