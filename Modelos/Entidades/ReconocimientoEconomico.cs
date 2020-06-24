using System;
using Modelos.Enumeracion;
using Modelos.ValueObjects;

namespace Modelos.Entidades
{
    public class ReconocimientoEconomico
    {
        public int Id { get; private set; }
        public int IdEmpleado { get; private set; }
        public DateTime FechaInicial { get; private set; }
        public DateTime FechaFinal { get; private set; }
        public Dinero ValorAPagar { get; private set; }
        public Entidad ResponsablePago { get; private set; }
        public int IdIncapacidad { get; private set; }
        public Incapacidad Incapacidad { get; private set; }

        public ReconocimientoEconomico(int idEmpleado, DateTime fechaInicial, DateTime fechaFinal, Dinero valorAPagar, Entidad responsablePago)
        {
            IdEmpleado = idEmpleado;
            FechaInicial = fechaInicial;
            FechaFinal = fechaFinal;
            ValorAPagar = valorAPagar;
            ResponsablePago = responsablePago;
        }

        private ReconocimientoEconomico() { }
    }
}