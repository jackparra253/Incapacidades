namespace Bitakora.Incapacidades;

public class CalcularFechas : ICalcularFechas
{
    public DateTime CalcularSiguienteFecha(DateTime fechaInicial, int cantidadDias)
    {
        return fechaInicial.AddDays(cantidadDias-1);
    }
}
