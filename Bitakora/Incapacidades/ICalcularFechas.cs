namespace Bitakora.Incapacidades;

public interface ICalcularFechas
{
    DateTime CalcularSiguienteFecha(DateTime fechaInicial, int cantidadDias);
}
