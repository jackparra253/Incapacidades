using IDatos;
using Modelos.Entidades;
using Modelos;
using Modelos.Enumeracion;
using Modelos.Excepciones;

namespace Datos;

public class IncapacidadServicio : IIncapacidadServicio
{

    private readonly IncapacidadesContext _contexto;

    public IncapacidadServicio(IncapacidadesContext contexto)
    {
        _contexto = contexto;
    }

    public void Guardar(Incapacidad incapacidad)
    {
        _contexto.Incapacidades.Add(incapacidad);
        _contexto.SaveChanges();
    }

    public List<DetalleIncapacidad> ObtenerIncapacidadesDetalle(int idEmpleado)
    {
        List<DetalleIncapacidad> incapacidadesDetalle = _contexto.Incapacidades
            .Where(i => i.IdEmpleado == idEmpleado)
            .Select(i => new DetalleIncapacidad(i.IncapacidadId,
                (TransformarATextoTipoIncapacida(i.TipoIncapacidad)),
                i.FechaIncial.ToShortDateString(),
                i.FechaFinal.ToShortDateString(),
                i.CantidadDias))
            .ToList();

        return incapacidadesDetalle;
    }

    private static string TransformarATextoTipoIncapacida(TipoIncapacidad tipoIncapacidad)
    {
        return tipoIncapacidad switch
        {
            TipoIncapacidad.EnfermedadGeneral => "Enfermedad General",
            TipoIncapacidad.LicenciaMaternidad => "Licencia Maternidad",
            TipoIncapacidad.LicenciaPaternidad => "Licencia Paternidad",
            TipoIncapacidad.EnfermedadLaboral => "Enfermedad Laboral",
            TipoIncapacidad.AccidenteLaboral => "Accidente Laboral",
            _ => throw new TipoIncapacidadInvalido((int)tipoIncapacidad)
        };
    }

    public List<DetalleReconocimientoEconomico> ObtenerReconocimientosEconomicosDetalle(int idEmpleado)
    {
        var reconocimientosEconomicos = _contexto.ReconocimientosEconomicos.ToList();

        List<DetalleReconocimientoEconomico> reconocimientosEconomicosDetalle = reconocimientosEconomicos
            .Where(re => re.IdEmpleado == idEmpleado)
            .Select(re => new DetalleReconocimientoEconomico(
                re.IncapacidadId,
                re.FechaInicial.ToShortDateString(),
                re.FechaFinal.ToShortDateString(),
                re.ValorAPagar,
                TransformarATextoResponsable(re.ResponsablePago)
            )).ToList();

        return reconocimientosEconomicosDetalle;
    }
    private static string TransformarATextoResponsable(Entidad responsable)
    {
        return responsable switch
        {
            Entidad.EPS => "EPS",
            Entidad.ARL => "ARL",
            Entidad.EMPRESA => "EMPRESA",
            Entidad.FONDO_PENSIONES => "FONDO DE PENSIONES",
            _ => throw new ArgumentOutOfRangeException(nameof(responsable), responsable, "Responsable de pago desconocido.")
        };
    }
}