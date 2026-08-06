using IAplicacion;
using Modelos;
using IDatos;
using Modelos.Entidades;
using System;
using System.Collections.Generic;
using Modelos.Enumeracion;

namespace Aplicacion
{
    public class CreadorIncapacidadLey50 : CreadorIncapacidad, ICreadorIncapacidadLey50
    {
        private readonly IResponsablePagoServicio _responsablePagoServicio;
        private readonly IEmpleadoServicio _empleadoServicio;
        private readonly IIncapacidadServicio _incapacidadServicio;

        public CreadorIncapacidadLey50(IResponsablePagoServicio responsablePagoServicio, IEmpleadoServicio empleadoServicio, IIncapacidadServicio incapacidadServicio)
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
                int cantidadDias = responsablePago.DiasQueCubre(solicitudIncapacidad.CantidadDias);

                DateTime fecha = responsablePago.FechaEnQueInicia(fechaIncial);

                var reconocimientoEconomico = new ReconocimientoEconomico(empleado.Id, fecha, cantidadDias, empleado.SalarioDiarioPorPorcentajeSalario, responsablePago.ReconocimientoPorcentaje, responsablePago.Responsable);

                reconocimientosEconomicos.Add(reconocimientoEconomico);
            }

            return reconocimientosEconomicos;
        }
    }
}