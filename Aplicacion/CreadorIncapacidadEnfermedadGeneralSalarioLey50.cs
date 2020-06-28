using IAplicacion;
using Modelos;
using IDatos;
using Modelos.Entidades;
using System;
using System.Collections.Generic;
using IDominio;
using Modelos.Enumeracion;

namespace Aplicacion
{
    public class CreadorIncapacidadEnfermedadGeneralSalarioLey50 : CreadorIncapacidad, ICreadorIncapacidadEnfermedadGeneralSalarioLey50
    {
        private readonly IResponsablePagoServicio _responsablePagoServicio;
        private readonly IEmpleadoServicio _empleadoServicio;
        private readonly IIncapacidadServicio _incapacidadServicio;

        public CreadorIncapacidadEnfermedadGeneralSalarioLey50(IResponsablePagoServicio responsablePagoServicio, IEmpleadoServicio empleadoServicio , IIncapacidadServicio incapacidadServicio)
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

            var reconocimientosEconomicos = new List<ReconocimientoEconomico>();

            CalcularReconocimientosEconomicosEnfermedadGeneral(solicitudIncapacidad, empleado, fechaIncial, responsablesPagos, reconocimientosEconomicos);

            var incapacidad = new Incapacidad(solicitudIncapacidad.IdEmpleado, (Modelos.Enumeracion.TipoIncapacidad)solicitudIncapacidad.TipoIncapacidad, fechaIncial, solicitudIncapacidad.CantidadDias, solicitudIncapacidad.Observaciones, reconocimientosEconomicos);

            _incapacidadServicio.Guardar(incapacidad);
        }

        private void CalcularReconocimientosEconomicosEnfermedadGeneral(SolicitudIncapacidad solicitudIncapacidad, Empleado empleado, DateTime fechaIncial, List<ResponsablePago> responsablesPagos, List<ReconocimientoEconomico> reconocimientosEconomicos)
        {
            foreach (var responsablePago in responsablesPagos)
            {
                int cantidadDias = CalcularCantidadDias(solicitudIncapacidad.CantidadDias, responsablePago);

                DateTime fecha = CalcularFechaInicial(fechaIncial, solicitudIncapacidad.CantidadDias, cantidadDias,responsablePago);

                var reconocimientoEconomico = new ReconocimientoEconomico(empleado.Id, fecha, cantidadDias, empleado.SalarioDiarioPorPorcentajeSalario, responsablePago.ReconocimientoPorcentaje, responsablePago.Responsable);

                reconocimientosEconomicos.Add(reconocimientoEconomico);
            }
        }

        public int CalcularCantidadDias(int cantidadDias, ResponsablePago responsablePago)
        {
            if (responsablePago.DiasIncapacidadInicial == cantidadDias)
                return cantidadDias;

            if (responsablePago.DiasIncapacidadFinal == cantidadDias)
                return cantidadDias;

            if (responsablePago.DiasIncapacidadFinal < cantidadDias)
                return responsablePago.DiasIncapacidadFinal;

            return (cantidadDias + 1) - responsablePago.DiasIncapacidadInicial;
        }

        public DateTime CalcularFechaInicial(DateTime fecha, int cantidadDiasInicial, int cantidadDias,ResponsablePago responsablePago)
        {
            if (responsablePago.DiasIncapacidadInicial == cantidadDiasInicial)
                return fecha;

            if (responsablePago.DiasIncapacidadFinal == cantidadDiasInicial)
                return fecha;

            if (responsablePago.DiasIncapacidadFinal < cantidadDiasInicial)
                return fecha;

            return fecha.AddDays(cantidadDias);
        }

    }
}