using System;
using System.Collections.Generic;
using Modelos.Enumeracion;

namespace Modelos.Entidades
{
    public class Incapacidad
    {
        public int IncapacidadId { get; private set; }
        public int IdEmpleado { get; private set; }
        public TipoIncapacidad TipoIncapacidad { get; private set; }
        public DateTime FechaIncial { get; private set; }
        public DateTime FechaFinal { get; private set; }
        public int CantidadDias { get; private set; }
        public string Observaciones { get; private set; }
        public List<ReconocimientoEconomico> ReconocimientosEconomicos { get; private set; }
        public Incapacidad(int idEmpleado, TipoIncapacidad tipoIncapacidad, DateTime fechaInicial, int cantidadDias, string observaciones,List<ReconocimientoEconomico> reconocimientosEconomicos)
        {
            IdEmpleado = idEmpleado;
            TipoIncapacidad = tipoIncapacidad;
            FechaIncial = fechaInicial;
            FechaFinal = fechaInicial.AddDays(cantidadDias -1);
            CantidadDias = cantidadDias;
            Observaciones = observaciones;
            ReconocimientosEconomicos = reconocimientosEconomicos;
        }

        private Incapacidad(){}
    }
}
