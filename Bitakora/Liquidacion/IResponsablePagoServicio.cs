using Bitakora.Incapacidades;

namespace Bitakora.Liquidacion;

public interface IResponsablePagoServicio
{
    List<ResponsablePago> ObtenerResponsablesPago();

    List<ResponsablePago> ObtenerResponsablesPago(TipoIncapacidad tipoIncapacidad, int cantidadDias);
}
