using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.Enumeracion;
using Modelos.ValueObjects;
using Shouldly;
using Xunit;

namespace Test.ModelosTest;

public class ReconocimientoEconomicoTest
{
    [Fact]
    public void Debe_ReconocimientoEconomico_CalcularValorAPagar()
    {
        var reconocimientoEconomico = new ReconocimientoEconomico(1, new DateTime(2020, 06, 27), 2, new Dinero(100_000, Moneda.COP), 0.6667m, Entidad.EPS);

        reconocimientoEconomico.ValorAPagar.ShouldBe(new Dinero(133_340m, Moneda.COP));
    }

    [Fact]
    public void Debe_ReconocimientoEconomico_AsignarFechaFinal()
    {
        var reconocimientoEconomico = new ReconocimientoEconomico(1, new DateTime(2020, 06, 27), 2, new Dinero(100_000, Moneda.COP), 0.6667m, Entidad.EPS);

        reconocimientoEconomico.FechaFinal.ShouldBe(new DateTime(2020, 06, 28));
    }
}
