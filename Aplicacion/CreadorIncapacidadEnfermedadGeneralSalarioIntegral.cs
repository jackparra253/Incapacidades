using IAplicacion;
using Modelos;
using IDatos;
using Modelos.Entidades;
using System;
using System.Collections.Generic;
using Modelos.Enumeracion;

namespace Aplicacion
{
    public class CreadorIncapacidadEnfermedadGeneralSalarioIntegral : CreadorIncapacidad, ICreadorIncapacidadEnfermedadGeneralSalarioIntegral
    {
        private readonly IResponsablePagoServicio _responsablePagoServicio;
        private readonly IEmpleadoServicio _empleadoServicio;
        private readonly IIncapacidadServicio _incapacidadServicio;

        public CreadorIncapacidadEnfermedadGeneralSalarioIntegral(IResponsablePagoServicio responsablePagoServicio, IEmpleadoServicio empleadoServicio, IIncapacidadServicio incapacidadServicio)
        {
            _responsablePagoServicio = responsablePagoServicio;
            _empleadoServicio = empleadoServicio;
            _incapacidadServicio = incapacidadServicio;
        }

        public override void Crear(SolicitudIncapacidad solicitudIncapacidad)
        {
            Empleado empleado = _empleadoServicio.ObtenerEmpleado(solicitudIncapacidad.IdEmpleado);

            var fechaIncial = new DateTime(solicitudIncapacidad.Anio, solicitudIncapacidad.Mes, solicitudIncapacidad.Dia);

            List<ResponsablePago> responsablesPagos = _responsablePagoServicio.ObtenerResponsablesPago((TipoIncapacidad)solicitudIncapacidad.TipoIncapacidad, solicitudIncapacidad.CantidadDias);

            List<ReconocimientoEconomico> reconocimientosEconomicos = CalcularReconocimientosEconomicosEnfermedadGeneral(solicitudIncapacidad, empleado, fechaIncial, responsablesPagos);

            var incapacidad = new Incapacidad(solicitudIncapacidad.IdEmpleado, (Modelos.Enumeracion.TipoIncapacidad)solicitudIncapacidad.TipoIncapacidad, fechaIncial, solicitudIncapacidad.CantidadDias, solicitudIncapacidad.Observaciones, reconocimientosEconomicos);

            _incapacidadServicio.Guardar(incapacidad);
        }

        private List<ReconocimientoEconomico> CalcularReconocimientosEconomicosEnfermedadGeneral(SolicitudIncapacidad solicitudIncapacidad, Empleado empleado, DateTime fechaIncial, List<ResponsablePago> responsablesPagos)
        {
            var reconocimientosEconomicos = new List<ReconocimientoEconomico>();

            foreach (var responsablePago in responsablesPagos)
            {
                int cantidadDias = CalcularCantidadDias(solicitudIncapacidad.CantidadDias, responsablePago);

                DateTime fecha = CalcularFechaInicial(fechaIncial, solicitudIncapacidad.CantidadDias, cantidadDias, responsablePago);

                if (responsablePago.DiasIncapacidadFinal <= solicitudIncapacidad.CantidadDias)
                {
                    var reconocimientoEconomico = new ReconocimientoEconomico(empleado.Id, fecha, cantidadDias, empleado.SalarioDiario, responsablePago.ReconocimientoPorcentaje, responsablePago.Responsable);
                    reconocimientosEconomicos.Add(reconocimientoEconomico);
                }

                if (responsablePago.DiasIncapacidadFinal > solicitudIncapacidad.CantidadDias)
                {
                    var reconocimientoEconomico = new ReconocimientoEconomico(empleado.Id, fecha, cantidadDias, empleado.SalarioDiarioPorPorcentajeSalario, responsablePago.ReconocimientoPorcentaje, responsablePago.Responsable);
                    var reconocimientoEconomicoCompensacion = new ReconocimientoEconomico(empleado.Id, fecha, cantidadDias, empleado.SalarioDiarioPorPorcentajeCompensacion, 1, responsablePago.Responsable);
                    reconocimientosEconomicos.Add(reconocimientoEconomico);
                    reconocimientosEconomicos.Add(reconocimientoEconomicoCompensacion);
                }

            }

            return reconocimientosEconomicos;
        }

        public int CalcularCantidadDias(int cantidadDias, ResponsablePago responsablePago)
        {
            if (responsablePago.DiasIncapacidadInicial == cantidadDias || responsablePago.DiasIncapacidadFinal == cantidadDias)
                return cantidadDias;

            if (responsablePago.DiasIncapacidadFinal < cantidadDias)
                return responsablePago.DiasIncapacidadFinal;

            return (cantidadDias + 1) - responsablePago.DiasIncapacidadInicial;
        }

        public DateTime CalcularFechaInicial(DateTime fecha, int cantidadDiasInicial, int cantidadDias, ResponsablePago responsablePago)
        {
            if (responsablePago.DiasIncapacidadInicial == cantidadDiasInicial || responsablePago.DiasIncapacidadFinal == cantidadDiasInicial || responsablePago.DiasIncapacidadFinal < cantidadDiasInicial)
                return fecha;

            return fecha.AddDays(cantidadDias-1);
        }

    }
}
