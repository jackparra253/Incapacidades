using Bitakora.Salarios;
using Shouldly;
using Xunit;

namespace Test.Salarios;

public class SalarioMinimoServicioTest
{
    [Fact]
    public void Debe_ObtenerSalarioMinimoMensual_DevolverElDecretadoParaElAnio()
    {
        var salarioMinimoServicio = new SalarioMinimoServicio();

        Dinero salarioMinimo = salarioMinimoServicio.ObtenerSalarioMinimoMensual(2026);

        salarioMinimo.ShouldBe(new Dinero(1_750_905m, Moneda.COP));
    }

    [Fact]
    public void Debe_ObtenerSalarioMinimoDiario_DividirElMensualEnTreinta()
    {
        var salarioMinimoServicio = new SalarioMinimoServicio();

        Dinero minimoDiario = salarioMinimoServicio.ObtenerSalarioMinimoDiario(2026);

        minimoDiario.ShouldBe(new Dinero(58_363.50m, Moneda.COP));
    }

    [Fact]
    public void Debe_ObtenerSalarioMinimo_Fallar_Cuando_NoSeConoceElAnio()
    {
        var salarioMinimoServicio = new SalarioMinimoServicio();

        Action consulta = () => salarioMinimoServicio.ObtenerSalarioMinimoMensual(2025);

        SalarioMinimoDesconocido error = Should.Throw<SalarioMinimoDesconocido>(consulta);

        error.Anio.ShouldBe(2025);
    }
}
