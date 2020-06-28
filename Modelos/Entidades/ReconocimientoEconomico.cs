using System;
using Modelos.Enumeracion;
using Modelos.ValueObjects;

namespace Modelos.Entidades
{
    public class ReconocimientoEconomico
    {
        public int ReconocimientoEconomicoId { get; private set; }
        public int IdEmpleado { get; private set; }
        public DateTime FechaInicial { get; private set; }
        public DateTime FechaFinal { get; private set; }
        public Dinero ValorAPagar { get; private set; }
        public Entidad ResponsablePago { get; private set; }
        public int IncapacidadId { get; private set; }
        public Incapacidad Incapacidad { get; private set; }

        public ReconocimientoEconomico(int idEmpleado, DateTime fechaInicial, int cantidadDias, Dinero salarioBase, decimal porcentajeReconocimiento, Entidad responsablePago)
        {
            IdEmpleado = idEmpleado;
            FechaInicial = fechaInicial;
            FechaFinal = fechaInicial.AddDays(cantidadDias - 1);
            ValorAPagar = CalcularValorAPagar(cantidadDias, salarioBase, porcentajeReconocimiento);
            ResponsablePago = responsablePago;
        }

        private Dinero CalcularValorAPagar(int cantidadDias, Dinero salarioBase, decimal porcentajeReconocimiento)
        {
            decimal valorAPagarDiario = salarioBase.Cantidad * porcentajeReconocimiento;
            decimal valorAPagarTotal = valorAPagarDiario * cantidadDias;
            return new Dinero(valorAPagarTotal, salarioBase.Moneda);
        }

        private ReconocimientoEconomico() { }
    }
}