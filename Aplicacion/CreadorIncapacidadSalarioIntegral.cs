using IAplicacion;
using Modelos;
using IDatos;
using Modelos.Entidades;
using Modelos.Enumeracion;

namespace Aplicacion;

public class CreadorIncapacidadSalarioIntegral : CreadorIncapacidad, ICreadorIncapacidadSalarioIntegral
{
    private readonly IResponsablePagoServicio _responsablePagoServicio;
    private readonly IEmpleadoServicio _empleadoServicio;
    private readonly IIncapacidadServicio _incapacidadServicio;

    public CreadorIncapacidadSalarioIntegral(IResponsablePagoServicio responsablePagoServicio, IEmpleadoServicio empleadoServicio, IIncapacidadServicio incapacidadServicio)
    {
        _responsablePagoServicio = responsablePagoServicio;
        _empleadoServicio = empleadoServicio;
        _incapacidadServicio = incapacidadServicio;
    }

    public override void Crear(SolicitudIncapacidad solicitudIncapacidad)
    {
        Empleado empleado = _empleadoServicio.ObtenerEmpleado(solicitudIncapacidad.IdEmpleado);

        DateTime fechaIncial = solicitudIncapacidad.FechaInicial;

        List<ResponsablePago> responsablesPagos = _responsablePagoServicio.ObtenerResponsablesPago(solicitudIncapacidad.TipoDeIncapacidad, solicitudIncapacidad.CantidadDias);

        List<ReconocimientoEconomico> reconocimientosEconomicos = CalcularReconocimientosEconomicosEnfermedadGeneral(solicitudIncapacidad, empleado, fechaIncial, responsablesPagos);

        var incapacidad = new Incapacidad(solicitudIncapacidad.IdEmpleado, solicitudIncapacidad.TipoDeIncapacidad, fechaIncial, solicitudIncapacidad.CantidadDias, solicitudIncapacidad.Observaciones, reconocimientosEconomicos);

        _incapacidadServicio.Guardar(incapacidad);
    }

    private List<ReconocimientoEconomico> CalcularReconocimientosEconomicosEnfermedadGeneral(SolicitudIncapacidad solicitudIncapacidad, Empleado empleado, DateTime fechaIncial, List<ResponsablePago> responsablesPagos)
    {
        var reconocimientosEconomicos = new List<ReconocimientoEconomico>();

        foreach (var responsablePago in responsablesPagos)
        {
            int cantidadDias = responsablePago.DiasQueCubre(solicitudIncapacidad.CantidadDias);

            DateTime fecha = responsablePago.FechaEnQueInicia(fechaIncial);

            if (responsablePago.DiasIncapacidadFinal <= solicitudIncapacidad.CantidadDias)
            {
                if (responsablePago.Responsable == Entidad.EMPRESA)
                {
                    var reconocimientoEconomico = new ReconocimientoEconomico(empleado.Id, fecha, cantidadDias, empleado.SalarioDiario, responsablePago.ReconocimientoPorcentaje, responsablePago.Responsable);
                    reconocimientosEconomicos.Add(reconocimientoEconomico);
                }

                if (responsablePago.Responsable != Entidad.EMPRESA)
                {
                    var reconocimientoEconomico = new ReconocimientoEconomico(empleado.Id, fecha, cantidadDias, empleado.SalarioDiarioPorPorcentajeSalario, responsablePago.ReconocimientoPorcentaje, responsablePago.Responsable);
                    reconocimientosEconomicos.Add(reconocimientoEconomico);

                    var reconocimientoEconomicoCompensacion = new ReconocimientoEconomico(empleado.Id, fecha, cantidadDias, empleado.SalarioDiarioPorPorcentajeCompensacion, 1, Entidad.EMPRESA);
                    reconocimientosEconomicos.Add(reconocimientoEconomicoCompensacion);
                }
            }

            if (responsablePago.DiasIncapacidadFinal > solicitudIncapacidad.CantidadDias)
            {
                var reconocimientoEconomico = new ReconocimientoEconomico(empleado.Id, fecha, cantidadDias, empleado.SalarioDiarioPorPorcentajeSalario, responsablePago.ReconocimientoPorcentaje, responsablePago.Responsable);
                var reconocimientoEconomicoCompensacion = new ReconocimientoEconomico(empleado.Id, fecha, cantidadDias, empleado.SalarioDiarioPorPorcentajeCompensacion, 1, Entidad.EMPRESA);
                reconocimientosEconomicos.Add(reconocimientoEconomico);
                reconocimientosEconomicos.Add(reconocimientoEconomicoCompensacion);
            }

        }
        return reconocimientosEconomicos;
    }
}