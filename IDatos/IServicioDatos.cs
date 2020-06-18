using System.Collections.Generic;
using Modelos.Entidades;

namespace IDatos
{
    public interface IServicioDatos
    {
        List<Empleado>  ObtenerEmpleados();
    }     
}
