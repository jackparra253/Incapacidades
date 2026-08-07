using IDatos;
using Modelos.Entidades;
using Modelos.Enumeracion;
using Modelos.Excepciones;

namespace Datos;

public class ResponsablePagoServicio : IResponsablePagoServicio
{
    private readonly IncapacidadesContext _contexto;

    public ResponsablePagoServicio(IncapacidadesContext contexto)
    {
        _contexto = contexto;
    }

    public List<ResponsablePago> ObtenerResponsablesPago()
    {
        return new List<ResponsablePago>
        {
            new ResponsablePago(2, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 3, 90, 0.6666m),
            new ResponsablePago(1, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 1,2, 1m),
            new ResponsablePago(3, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 91, 180, 0.5m),
            new ResponsablePago(8, Entidad.FONDO_PENSIONES, TipoIncapacidad.EnfermedadGeneral, 181, 540, 0.5m),
            new ResponsablePago(4, Entidad.EPS, TipoIncapacidad.LicenciaMaternidad, 1, 126, 1m),
            new ResponsablePago(5, Entidad.EPS, TipoIncapacidad.LicenciaPaternidad, 1, 8, 1m),
            new ResponsablePago(6, Entidad.ARL, TipoIncapacidad.EnfermedadLaboral, 1, 180, 1m),
            new ResponsablePago(7, Entidad.ARL, TipoIncapacidad.AccidenteLaboral, 1, 180, 1m)
        };
    }

    public List<ResponsablePago> ObtenerResponsablesPago(TipoIncapacidad tipoIncapacidad, int cantidadDias)
    {
        List<ResponsablePago> responsablesPagos = ObtenerResponsablesPago()
            .Where(responsablePago => responsablePago.TipoIncapacidad == tipoIncapacidad
                                      && responsablePago.DiasIncapacidadInicial <= cantidadDias)
            .OrderBy(responsablePago => responsablePago.DiasIncapacidadInicial)
            .ToList();

        int ultimoDiaCubierto = responsablesPagos.Count == 0
            ? 0
            : responsablesPagos[^1].DiasIncapacidadFinal;

        if (ultimoDiaCubierto < cantidadDias)
            throw new DiasSinResponsableDePago(tipoIncapacidad, cantidadDias, ultimoDiaCubierto);

        return responsablesPagos;
    }
}