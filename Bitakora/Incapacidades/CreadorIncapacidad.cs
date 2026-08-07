using Bitakora.Empleados;
using Bitakora.Liquidacion;
using Bitakora.Salarios;

namespace Bitakora.Incapacidades;

public class CreadorIncapacidad : ICreadorIncapacidad
{
    private readonly IResponsablePagoServicio _responsablePagoServicio;
    private readonly IEmpleadoServicio _empleadoServicio;
    private readonly IIncapacidadServicio _incapacidadServicio;
    private readonly ISalarioMinimoServicio _salarioMinimoServicio;

    public CreadorIncapacidad(IResponsablePagoServicio responsablePagoServicio, IEmpleadoServicio empleadoServicio, IIncapacidadServicio incapacidadServicio, ISalarioMinimoServicio salarioMinimoServicio)
    {
        _responsablePagoServicio = responsablePagoServicio;
        _empleadoServicio = empleadoServicio;
        _incapacidadServicio = incapacidadServicio;
        _salarioMinimoServicio = salarioMinimoServicio;
    }

    public void Crear(SolicitudIncapacidad solicitudIncapacidad)
    {
        Empleado empleado = _empleadoServicio.ObtenerEmpleado(solicitudIncapacidad.IdEmpleado);

        List<ResponsablePago> responsablesPagos = _responsablePagoServicio.ObtenerResponsablesPago(
            solicitudIncapacidad.TipoDeIncapacidad, solicitudIncapacidad.CantidadDias);

        Dinero minimoDiario = _salarioMinimoServicio.ObtenerSalarioMinimoDiario(solicitudIncapacidad.FechaInicial.Year);

        List<ReconocimientoEconomico> reconocimientosEconomicos = CalcularReconocimientosEconomicos(
            empleado, solicitudIncapacidad.FechaInicial, solicitudIncapacidad.CantidadDias, responsablesPagos, minimoDiario);

        var incapacidad = new Incapacidad(
            solicitudIncapacidad.IdEmpleado,
            solicitudIncapacidad.TipoDeIncapacidad,
            solicitudIncapacidad.FechaInicial,
            solicitudIncapacidad.CantidadDias,
            solicitudIncapacidad.Observaciones,
            reconocimientosEconomicos);

        _incapacidadServicio.Guardar(incapacidad);
    }

    private static List<ReconocimientoEconomico> CalcularReconocimientosEconomicos(Empleado empleado, DateTime fechaInicial, int diasDeIncapacidad, List<ResponsablePago> responsablesPagos, Dinero minimoDiario)
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
                responsablePago.Responsable,
                minimoDiario);

            reconocimientosEconomicos.Add(reconocimientoEconomico);
        }

        return reconocimientosEconomicos;
    }
}
