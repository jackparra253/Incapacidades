namespace Bitakora.Salarios;

public interface ISalarioMinimoServicio
{
    Dinero ObtenerSalarioMinimoMensual(int anio);

    Dinero ObtenerSalarioMinimoDiario(int anio);
}
