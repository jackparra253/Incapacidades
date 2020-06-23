using System.Collections.Generic;
using Modelos.Entidades;

namespace IDatos
{
    public interface IServicioDatos
    {
        List<Empleado>  ObtenerEmpleados();

        Empleado ObtenerEmpleado(int id);

        List<ResponsablePago> ObtenerResponsablesPago();

        ResponsablePago ObtenerResponsablePago(int id);
    }     
}
