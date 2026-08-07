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
        var salarioDiario = new Dinero(100_000m, Moneda.COP);

        Dinero resultado = salarioDiario.Por(3);

        resultado.ShouldBe(new Dinero(300_000m, Moneda.COP));
    }

    [Fact]
    public void Debe_Entre_DividirConservandoLaMoneda()
    {
        var salarioMensual = new Dinero(3_000_000m, Moneda.COP);

        Dinero resultado = salarioMensual.Entre(30);

        resultado.ShouldBe(new Dinero(100_000m, Moneda.COP));
    }

    [Fact]
    public void Debe_Entre_Fallar_Cuando_ElDivisorEsCero()
    {
        var salario = new Dinero(100m, Moneda.COP);

        Action division = () => salario.Entre(0);

        Should.Throw<DivideByZeroException>(division);
    }

    [Fact]
    public void Debe_Mas_Sumar_Cuando_LasMonedasCoinciden()
    {
        var unPago = new Dinero(200_000m, Moneda.COP);
        var otroPago = new Dinero(50_000m, Moneda.COP);

        Dinero resultado = unPago.Mas(otroPago);

        resultado.ShouldBe(new Dinero(250_000m, Moneda.COP));
    }

    [Fact]
    public void Debe_Mas_Fallar_Cuando_LasMonedasDifieren()
    {
        var pesos = new Dinero(1m, Moneda.COP);
        var dolares = new Dinero(1m, "USD");

        Action suma = () => pesos.Mas(dolares);

        MonedasIncompatibles error = Should.Throw<MonedasIncompatibles>(suma);
        error.Una.ShouldBe("COP");
        error.Otra.ShouldBe("USD");
    }

    [Fact]
    public void Debe_EsMenorQue_CompararCantidades_Cuando_LasMonedasCoinciden()
    {
        var minimoDiario = new Dinero(58_363.50m, Moneda.COP);
        var valorDiario = new Dinero(44_440m, Moneda.COP);

        bool esMenor = valorDiario.EsMenorQue(minimoDiario);

        esMenor.ShouldBeTrue();
    }

    [Fact]
    public void Debe_EsMenorQue_Fallar_Cuando_LasMonedasDifieren()
    {
        var pesos = new Dinero(1m, Moneda.COP);
        var dolares = new Dinero(1m, "USD");

        Action comparacion = () => pesos.EsMenorQue(dolares);

        Should.Throw<MonedasIncompatibles>(comparacion);
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
        Action construccion = () => new Dinero(100m, moneda);

        Should.Throw<MonedaInvalida>(construccion);
    }

    [Fact]
    public void Debe_Construir_NormalizarLaMonedaAMayusculas()
    {
        var dinero = new Dinero(100m, "cop");

        string moneda = dinero.Moneda;

        moneda.ShouldBe("COP");
    }
}
