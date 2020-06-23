using IAplicacion;
using Modelos;
using IDatos;
using Modelos.Entidades;
using System;

namespace Aplicacion
{
    public class CreadorIncapacidad : ICreadorIncapacidad
    {
        private readonly IServicioDatos _servicioDatos;
        private readonly ICalcularFechas _calcularFechas;

        public CreadorIncapacidad(IServicioDatos servicioDatos, ICalcularFechas calcularFechas)
        {
            _servicioDatos = servicioDatos;
            _calcularFechas = calcularFechas;
        }

        public void Crear(SolicitudIncapacidad solicitudIncapacidad)
        {
            Empleado empleado = _servicioDatos.ObtenerEmpleado(solicitudIncapacidad.IdEmpleado);
            
            var fechaIncial = new DateTime(solicitudIncapacidad.Anio, solicitudIncapacidad.Mes, solicitudIncapacidad.Dia);
            DateTime fechaFinalIncapacidad = _calcularFechas.CalcularSiguienteFecha(fechaIncial, solicitudIncapacidad.CantidadDias);
        }
    }
}