using Modelos.Constantes;
using Modelos.Excepciones;
using Modelos.ValueObjects;
using Shouldly;
using Xunit;

namespace Test.ModelosTest;

public class DineroTest
{
    [Fact]
    public void Debe_Por_MultiplicarConservandoLaMoneda()
    {
        new Dinero(100_000m, Moneda.COP).Por(3).ShouldBe(new Dinero(300_000m, Moneda.COP));
    }

    [Fact]
    public void Debe_Entre_DividirConservandoLaMoneda()
    {
        new Dinero(3_000_000m, Moneda.COP).Entre(30).ShouldBe(new Dinero(100_000m, Moneda.COP));
    }

    [Fact]
    public void Debe_Entre_Fallar_Cuando_ElDivisorEsCero()
    {
        Should.Throw<DivideByZeroException>(() => new Dinero(100m, Moneda.COP).Entre(0));
    }

    [Fact]
    public void Debe_Mas_Sumar_Cuando_LasMonedasCoinciden()
    {
        new Dinero(200_000m, Moneda.COP)
            .Mas(new Dinero(50_000m, Moneda.COP))
            .ShouldBe(new Dinero(250_000m, Moneda.COP));
    }

    // El motivo de que Dinero tenga comportamiento: mientras los cálculos se hacían afuera sobre
    // .Cantidad, nadie verificaba que las monedas coincidieran.
    [Fact]
    public void Debe_Mas_Fallar_Cuando_LasMonedasDifieren()
    {
        var error = Should.Throw<MonedasIncompatibles>(
            () => new Dinero(1m, Moneda.COP).Mas(new Dinero(1m, "USD")));

        error.Una.ShouldBe("COP");
        error.Otra.ShouldBe("USD");
    }

    [Fact]
    public void Debe_SerInmutable_Cuando_SeOpera()
    {
        var salario = new Dinero(100_000m, Moneda.COP);

        salario.Por(5);
        salario.Mas(new Dinero(1m, Moneda.COP));

        salario.ShouldBe(new Dinero(100_000m, Moneda.COP));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("CO")]
    [InlineData("PESOS")]
    [InlineData("C0P")]
    public void Debe_Construir_Fallar_Cuando_LaMonedaNoEsUnCodigoDeTresLetras(string moneda)
    {
        Should.Throw<MonedaInvalida>(() => new Dinero(100m, moneda));
    }

    [Fact]
    public void Debe_Construir_NormalizarLaMonedaAMayusculas()
    {
        new Dinero(100m, "cop").Moneda.ShouldBe("COP");
    }
}
