using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.Enumeracion;
using Modelos.ValueObjects;
using Shouldly;
using Xunit;

namespace Test.ModelosTest;

public class ReconocimientoEconomicoTest
{
    private static readonly Dinero MinimoDiarioDe2020 = new(29_260m, Moneda.COP);
    private static readonly Dinero MinimoDiarioDe2026 = new(58_363.50m, Moneda.COP);

    [Fact]
    public void Debe_CalcularValorAPagar_AplicarElPorcentajePorLaCantidadDeDias()
    {
        var salarioBase = new Dinero(100_000m, Moneda.COP);

        var reconocimiento = new ReconocimientoEconomico(1, new DateTime(2020, 06, 27), 2, salarioBase, 0.6666m, Entidad.EPS, MinimoDiarioDe2020);

        reconocimiento.ValorAPagar.ShouldBe(new Dinero(133_320m, Moneda.COP));
    }

    [Fact]
    public void Debe_AsignarFechaFinal_ContandoElPrimerDiaComoCubierto()
    {
        var salarioBase = new Dinero(100_000m, Moneda.COP);

        var reconocimiento = new ReconocimientoEconomico(1, new DateTime(2020, 06, 27), 2, salarioBase, 0.6666m, Entidad.EPS, MinimoDiarioDe2020);

        reconocimiento.FechaFinal.ShouldBe(new DateTime(2020, 06, 28));
    }

    [Fact]
    public void Debe_CalcularValorAPagar_SubirAlMinimo_Cuando_ElPorcentajeLoDejaPorDebajo()
    {
        var salarioBaseDeUnMinimo = new Dinero(66_666.67m, Moneda.COP);

        var reconocimiento = new ReconocimientoEconomico(1, new DateTime(2026, 06, 03), 3, salarioBaseDeUnMinimo, 0.6666m, Entidad.EPS, MinimoDiarioDe2026);

        reconocimiento.ValorAPagar.ShouldBe(new Dinero(175_090.50m, Moneda.COP));
    }

    [Fact]
    public void Debe_CalcularValorAPagar_NoTocarElValor_Cuando_YaSuperaElMinimo()
    {
        var salarioBaseAlto = new Dinero(350_000m, Moneda.COP);

        var reconocimiento = new ReconocimientoEconomico(1, new DateTime(2026, 06, 03), 2, salarioBaseAlto, 0.6666m, Entidad.EPS, MinimoDiarioDe2026);

        reconocimiento.ValorAPagar.ShouldBe(new Dinero(466_620m, Moneda.COP));
    }
}
