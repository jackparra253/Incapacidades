using System.Collections.Generic;
using Modelos.Entidades;
using Modelos.Enumeracion;

namespace IDatos
{
    public interface IServicioDatos
    {
        List<Empleado>  ObtenerEmpleados();

        Empleado ObtenerEmpleado(int id);

        List<ResponsablePago> ObtenerResponsablesPago();

        ResponsablePago ObtenerResponsablePago(int id);

        List<ResponsablePago> ObtenerResponsablesPago(TipoIncapacidad tipoIncapacidad, TipoSalario tipoSalario, int cantidadDias);
    }     
}
