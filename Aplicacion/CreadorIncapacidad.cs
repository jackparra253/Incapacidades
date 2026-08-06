using IAplicacion;
using IDatos;
using Modelos;
using Modelos.Entidades;

namespace Aplicacion;

/// <summary>
/// Único creador de incapacidades. La diferencia entre salario ordinario e integral no vive acá:
/// la resuelve el <see cref="TipoSalario"/> del empleado, que define qué fracción del salario es
/// el IBC. Por eso el reparto entre responsables es el mismo algoritmo para los dos.
/// </summary>
public class CreadorIncapacidad : ICreadorIncapacidad
{
    private readonly IResponsablePagoServicio _responsablePagoServicio;
    private readonly IEmpleadoServicio _empleadoServicio;
    private readonly IIncapacidadServicio _incapacidadServicio;

    public CreadorIncapacidad(IResponsablePagoServicio responsablePagoServicio, IEmpleadoServicio empleadoServicio, IIncapacidadServicio incapacidadServicio)
    {
        _responsablePagoServicio = responsablePagoServicio;
        _empleadoServicio = empleadoServicio;
        _incapacidadServicio = incapacidadServicio;
    }

    public void Crear(SolicitudIncapacidad solicitudIncapacidad)
    {
        Empleado empleado = _empleadoServicio.ObtenerEmpleado(solicitudIncapacidad.IdEmpleado);

        List<ResponsablePago> responsablesPagos = _responsablePagoServicio.ObtenerResponsablesPago(
            solicitudIncapacidad.TipoDeIncapacidad, solicitudIncapacidad.CantidadDias);

        List<ReconocimientoEconomico> reconocimientosEconomicos = CalcularReconocimientosEconomicos(
            empleado, solicitudIncapacidad.FechaInicial, solicitudIncapacidad.CantidadDias, responsablesPagos);

        var incapacidad = new Incapacidad(
            solicitudIncapacidad.IdEmpleado,
            solicitudIncapacidad.TipoDeIncapacidad,
            solicitudIncapacidad.FechaInicial,
            solicitudIncapacidad.CantidadDias,
            solicitudIncapacidad.Observaciones,
            reconocimientosEconomicos);

        _incapacidadServicio.Guardar(incapacidad);
    }

    private static List<ReconocimientoEconomico> CalcularReconocimientosEconomicos(Empleado empleado, DateTime fechaInicial, int diasDeIncapacidad, List<ResponsablePago> responsablesPagos)
    {
        var reconocimientosEconomicos = new List<ReconocimientoEconomico>();

        foreach (ResponsablePago responsablePago in responsablesPagos)
        {
            var reconocimientoEconomico = new ReconocimientoEconomico(
                empleado.Id,
                responsablePago.FechaEnQueInicia(fechaInicial),
                responsablePago.DiasQueCubre(diasDeIncapacidad),
                empleado.SalarioDiarioPorPorcentajeSalario,
                responsablePago.ReconocimientoPorcentaje,
                responsablePago.Responsable);

            reconocimientosEconomicos.Add(reconocimientoEconomico);
        }

        return reconocimientosEconomicos;
    }
}
