using System.Collections.Generic;
using Modelos.Entidades;

namespace IAplicacion
{
    public interface IConsultarEmpleados
    {
        List<Empleado> ObtenerEmpleados();
    }
}
