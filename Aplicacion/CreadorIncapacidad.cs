using IAplicacion;
using Modelos;
using IDatos;
using Modelos.Entidades;

namespace Aplicacion
{
    public class CreadorIncapacidad : ICreadorIncapacidad
    {
        private readonly IServicioDatos _servicioDatos;

        public CreadorIncapacidad(IServicioDatos servicioDatos)
        {
            _servicioDatos = servicioDatos;
        }

        public void Crear(SolicitudIncapacidad solicitudIncapacidad)
        {
            var empleado = _servicioDatos.ObtenerEmpleados();
        }
    }
}