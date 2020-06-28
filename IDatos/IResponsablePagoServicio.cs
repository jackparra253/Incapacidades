using System.Collections.Generic;
using Modelos.Entidades;
using Modelos.Enumeracion;

namespace IDatos
{
    public interface IResponsablePagoServicio
    {
        List<ResponsablePago> ObtenerResponsablesPago();

        List<ResponsablePago> ObtenerResponsablesPago(TipoIncapacidad tipoIncapacidad, int cantidadDias);
    }     
}
