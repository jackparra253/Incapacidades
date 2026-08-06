using Modelos.Enumeracion;
using Modelos.ValueObjects;

namespace Modelos.Entidades;

public class ReconocimientoEconomico
{
    public int ReconocimientoEconomicoId { get; private set; }
    public int IdEmpleado { get; private set; }
    public DateTime FechaInicial { get; private set; }
    public DateTime FechaFinal { get; private set; }
    public Dinero ValorAPagar { get; private set; } = null!;
    public Entidad ResponsablePago { get; private set; }
    public int IncapacidadId { get; private set; }
    public Incapacidad? Incapacidad { get; private set; }

    public ReconocimientoEconomico(int idEmpleado, DateTime fechaInicial, int cantidadDias, Dinero salarioBase, decimal porcentajeReconocimiento, Entidad responsablePago)
    {
        IdEmpleado = idEmpleado;
        FechaInicial = fechaInicial;
        FechaFinal = fechaInicial.AddDays(cantidadDias - 1);
        ValorAPagar = salarioBase.Por(porcentajeReconocimiento).Por(cantidadDias);
        ResponsablePago = responsablePago;
    }

    private ReconocimientoEconomico() { }
}