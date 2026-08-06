using Modelos.ValueObjects;

namespace IDatos;

public interface ISalarioMinimoServicio
{
    Dinero ObtenerSalarioMinimoMensual(int anio);

    Dinero ObtenerSalarioMinimoDiario(int anio);
}
