using Modelos.ValueObjects;

namespace Modelos;

public class DetalleReconocimientoEconomico
{
    public int IdIncapacidad { get; private set; }
    public string FechaInicial { get; private set; } = string.Empty;
    public string FechaFinal { get; private set; } = string.Empty;
    public Dinero ValorAPagar { get; private set; } = null!;
    public string ResponsablePago { get; private set; } = string.Empty;
        


    public DetalleReconocimientoEconomico(int idIncapacidad, string fechaInicial, string fechaFinal, Dinero valorAPagar, string responsablePago)
    {
        IdIncapacidad = idIncapacidad;
        FechaInicial = fechaInicial;
        FechaFinal = fechaFinal;
        ValorAPagar = valorAPagar;
        ResponsablePago = responsablePago;
            
    }
}