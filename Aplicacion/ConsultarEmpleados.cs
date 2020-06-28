using System.Collections.Generic;
using IAplicacion;
using IDatos;
using Modelos.Entidades;

namespace Aplicacion
{
    public class ConsultarEmpleados: IConsultarEmpleados
    {
        private readonly IEmpleadoServicio _empleadoServicio;

        public ConsultarEmpleados(IEmpleadoServicio empleadoServicio)
        {
            _empleadoServicio = empleadoServicio;
        }

        public List<Empleado> ObtenerEmpleados()
        {
            return _empleadoServicio.ObtenerEmpleados();
        }
    }
}
