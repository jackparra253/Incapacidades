using Modelos.Entidades;
using Modelos.Enumeracion;
using Shouldly;
using Xunit;

namespace Test.ModelosTest;

public class ResponsablePagoTest
{
    // Enfermedad general segun normativa (minjusticia): empresa dias 1-2 al 100%,
    // EPS desde el dia 3 hasta el 90 al 66.67%. Periodos contiguos, sin traslape.
    private static ResponsablePago Empresa() =>
        new ResponsablePago(1, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 1, 2, 1m);

    private static ResponsablePago Eps() =>
        new ResponsablePago(2, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 3, 90, 0.6667m);

    [Theory]
    [InlineData(1, 1)]   // incapacidad de 1 dia: la empresa solo cubre ese dia
    [InlineData(2, 2)]
    [InlineData(4, 2)]   // a partir del dia 3 ya no es la empresa
    [InlineData(200, 2)]
    public void Debe_DiasQueCubre_LimitarseAlRangoDeLaEmpresa(int diasDeIncapacidad, int diasEsperados)
    {
        Empresa().DiasQueCubre(diasDeIncapacidad).ShouldBe(diasEsperados);
    }

    [Theory]
    [InlineData(3, 1)]     // arranca justo el ultimo dia de la incapacidad
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(90, 88)]
    [InlineData(200, 88)]  // se corta en su dia final, no sigue de largo
    public void Debe_DiasQueCubre_ContarDesdeElDiaTresHastaElFinal(int diasDeIncapacidad, int diasEsperados)
    {
        Eps().DiasQueCubre(diasDeIncapacidad).ShouldBe(diasEsperados);
    }

    [Fact]
    public void Debe_FechaEnQueInicia_SerLaFechaDeLaIncapacidad_Cuando_CubreDesdeElDiaUno()
    {
        Empresa().FechaEnQueInicia(new DateTime(2020, 06, 03)).ShouldBe(new DateTime(2020, 06, 03));
    }

    [Fact]
    public void Debe_FechaEnQueInicia_CorrerseAlDiaTres_Cuando_LaEmpresaYaCubrioDosDias()
    {
        Eps().FechaEnQueInicia(new DateTime(2020, 06, 03)).ShouldBe(new DateTime(2020, 06, 05));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(30)]
    public void Debe_LaEmpresaYLaEps_CubrirDiasContiguosSinTraslape_ParaCualquierDuracion(int diasDeIncapacidad)
    {
        var fechaInicial = new DateTime(2020, 06, 03);

        DateTime finDeLaEmpresa = Empresa().FechaEnQueInicia(fechaInicial)
            .AddDays(Empresa().DiasQueCubre(diasDeIncapacidad) - 1);
        DateTime inicioDeLaEps = Eps().FechaEnQueInicia(fechaInicial);
        DateTime finDeLaEps = inicioDeLaEps.AddDays(Eps().DiasQueCubre(diasDeIncapacidad) - 1);

        inicioDeLaEps.ShouldBe(finDeLaEmpresa.AddDays(1));
        finDeLaEps.ShouldBe(fechaInicial.AddDays(diasDeIncapacidad - 1));
    }
}
