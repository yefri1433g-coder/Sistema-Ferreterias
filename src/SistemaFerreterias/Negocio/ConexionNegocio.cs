using Datos;

namespace Negocio
{
    // CLASE ConexionNegocio: Coordina operaciones y aplica reglas antes de llamar a Datos.
    public class ConexionNegocio
    {
        private readonly ConexionDatos _datos = new ConexionDatos();

        public void ProbarConexion()
        {
            _datos.MtdProbarConexion();
        }
    }
}