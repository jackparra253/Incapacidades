namespace Bitakora.Salarios;

public class SalarioMinimoServicio : ISalarioMinimoServicio
{
    private static readonly Dictionary<int, decimal> PorAnio = new()
    {
        { 2026, 1_750_905m },

        { 2020, 877_803m }
    };

    public Dinero ObtenerSalarioMinimoMensual(int anio)
    {
        if (!PorAnio.TryGetValue(anio, out decimal cantidad))
            throw new SalarioMinimoDesconocido(anio);

        return new Dinero(cantidad, Moneda.COP);
    }

    public Dinero ObtenerSalarioMinimoDiario(int anio)
    {
        return ObtenerSalarioMinimoMensual(anio).Entre(30);
    }
}
