using System.Collections.Generic;
using Modelos.Entidades;

namespace IDatos
{
    public interface IEmpleadoServicio
    {
        List<Empleado> ObtenerEmpleados();

        Empleado ObtenerEmpleado(int id);
    }
}