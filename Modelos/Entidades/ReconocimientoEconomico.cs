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

    public ReconocimientoEconomico(int idEmpleado, DateTime fechaInicial, int cantidadDias, Dinero salarioBase, decimal porcentajeReconocimiento, Entidad responsablePago, Dinero minimoDiario)
    {
        IdEmpleado = idEmpleado;
        FechaInicial = fechaInicial;
        FechaFinal = fechaInicial.AddDays(cantidadDias - 1);
        ValorAPagar = ValorDiario(salarioBase, porcentajeReconocimiento, minimoDiario).Por(cantidadDias);
        ResponsablePago = responsablePago;
    }

    private static Dinero ValorDiario(Dinero salarioBase, decimal porcentajeReconocimiento, Dinero minimoDiario)
    {
        Dinero valorDiario = salarioBase.Por(porcentajeReconocimiento);

        return valorDiario.EsMenorQue(minimoDiario) ? minimoDiario : valorDiario;
    }

    private ReconocimientoEconomico() { }
}