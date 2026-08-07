using Bitakora.Salarios;

namespace Bitakora.Incapacidades;

public class DetalleIncapacidad
{
    public int Id { get; private set; }
    public string Tipo { get; private set; } = string.Empty;
    public string FechaInicial { get; private set; } = string.Empty;
    public string FechaFinal { get; private set; } = string.Empty;
    public int CantidadDias { get; private set; }
    public Dinero TotalAPagar { get; private set; } = null!;

    public DetalleIncapacidad(int id,string tipo,string fechaInicial, string fechaFinal, int cantidadDias, Dinero totalAPagar)
    {
        Id = id;
        Tipo = tipo;
        FechaInicial = fechaInicial;
        FechaFinal = fechaFinal;
        CantidadDias = cantidadDias;
        TotalAPagar = totalAPagar;
    }

}
