using IAplicacion;
using Modelos;
using IDatos;
using Modelos.Entidades;
using System;
using System.Collections.Generic;
using IDominio;
using Modelos.ValueObjects;

namespace Aplicacion
{
    public class CreadorIncapacidad : ICreadorIncapacidad
    {
        private readonly IServicioDatos _servicioDatos;
        private readonly ICalcularFechas _calcularFechas;
        private readonly IIncapacidadServicio _incapacidadServicio;
        private readonly ICalculadoraReconocimientoEconomico _calculadoraReconocimientoEconomico;

        public CreadorIncapacidad(IServicioDatos servicioDatos, ICalcularFechas calcularFechas, ICalculadoraReconocimientoEconomico calculadoraReconocimientoEconomico, IIncapacidadServicio incapacidadServicio)
        {
            _servicioDatos = servicioDatos;
            _calcularFechas = calcularFechas;
            _calculadoraReconocimientoEconomico = calculadoraReconocimientoEconomico;
            _incapacidadServicio = incapacidadServicio;
        }

        public void Crear(SolicitudIncapacidad solicitudIncapacidad)
        {
            Empleado empleado = _servicioDatos.ObtenerEmpleado(solicitudIncapacidad.IdEmpleado);
            
            var fechaIncial = new DateTime(solicitudIncapacidad.Anio, solicitudIncapacidad.Mes, solicitudIncapacidad.Dia);

            DateTime fechaFinalIncapacidad = _calcularFechas.CalcularSiguienteFecha(fechaIncial, solicitudIncapacidad.CantidadDias);

            List<ResponsablePago> responsablesPagos = _servicioDatos.ObtenerResponsablesPago((Modelos.Enumeracion.TipoIncapacidad)solicitudIncapacidad.TipoIncapacidad, empleado.TipoSalario, solicitudIncapacidad.CantidadDias);

            var reconocimientosEconomicos = new List<ReconocimientoEconomico>();

            //fechaInicial
            //CantidadDias
            //DiasRestantes

            int cantidadDiasRestantes = 2;
            foreach(var responsablePago in responsablesPagos)
            {
                Dinero valorAPagar = _calculadoraReconocimientoEconomico.CalcularReconocimientoEconomico(empleado, responsablePago, cantidadDiasRestantes);

                var reconocimientoEconomico = new ReconocimientoEconomico(empleado.Id, fechaIncial, fechaIncial.AddDays(solicitudIncapacidad.CantidadDias), valorAPagar, responsablePago.Responsable);

                reconocimientosEconomicos.Add(reconocimientoEconomico);
            }

            var incapacidad = new Incapacidad(solicitudIncapacidad.IdEmpleado,(Modelos.Enumeracion.TipoIncapacidad)solicitudIncapacidad.TipoIncapacidad, fechaIncial, fechaFinalIncapacidad, solicitudIncapacidad.CantidadDias, solicitudIncapacidad.Observaciones, reconocimientosEconomicos);

            _incapacidadServicio.Guardar(incapacidad);
        }
    }
}