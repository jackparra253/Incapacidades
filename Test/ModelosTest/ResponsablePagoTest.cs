using Modelos.Entidades;
using Modelos.Enumeracion;
using Shouldly;
using Xunit;

namespace Test.ModelosTest;

public class ResponsablePagoTest
{
    private static ResponsablePago Empresa() =>
        new ResponsablePago(1, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 1, 2, 1m);

    private static ResponsablePago Eps() =>
        new ResponsablePago(2, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 3, 90, 0.6666m);

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(4, 2)]
    [InlineData(200, 2)]
    public void Debe_DiasQueCubre_LimitarseALosDosPrimerosDias(int diasDeIncapacidad, int diasEsperados)
    {
        ResponsablePago empresa = Empresa();

        int diasQueCubre = empresa.DiasQueCubre(diasDeIncapacidad);

        diasQueCubre.ShouldBe(diasEsperados);
    }

    [Theory]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(90, 88)]
    [InlineData(200, 88)]
    public void Debe_DiasQueCubre_ContarDesdeElDiaTresSinPasarseDelDiaNoventa(int diasDeIncapacidad, int diasEsperados)
    {
        ResponsablePago eps = Eps();

        int diasQueCubre = eps.DiasQueCubre(diasDeIncapacidad);

        diasQueCubre.ShouldBe(diasEsperados);
    }

    [Fact]
    public void Debe_FechaEnQueInicia_SerLaFechaDeLaIncapacidad_Cuando_CubreDesdeElDiaUno()
    {
        ResponsablePago empresa = Empresa();
        var fechaInicialDeLaIncapacidad = new DateTime(2020, 06, 03);

        DateTime fechaEnQueInicia = empresa.FechaEnQueInicia(fechaInicialDeLaIncapacidad);

        fechaEnQueInicia.ShouldBe(new DateTime(2020, 06, 03));
    }

    [Fact]
    public void Debe_FechaEnQueInicia_CorrerseAlDiaTres_Cuando_LaEmpresaYaCubrioDosDias()
    {
        ResponsablePago eps = Eps();
        var fechaInicialDeLaIncapacidad = new DateTime(2020, 06, 03);

        DateTime fechaEnQueInicia = eps.FechaEnQueInicia(fechaInicialDeLaIncapacidad);

        fechaEnQueInicia.ShouldBe(new DateTime(2020, 06, 05));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(30)]
    public void Debe_LaEmpresaYLaEps_CubrirDiasContiguosSinTraslape_ParaCualquierDuracion(int diasDeIncapacidad)
    {
        ResponsablePago empresa = Empresa();
        ResponsablePago eps = Eps();
        var fechaInicial = new DateTime(2020, 06, 03);

        DateTime finDeLaEmpresa = empresa.FechaEnQueInicia(fechaInicial).AddDays(empresa.DiasQueCubre(diasDeIncapacidad) - 1);
        DateTime inicioDeLaEps = eps.FechaEnQueInicia(fechaInicial);
        DateTime finDeLaEps = inicioDeLaEps.AddDays(eps.DiasQueCubre(diasDeIncapacidad) - 1);

        inicioDeLaEps.ShouldBe(finDeLaEmpresa.AddDays(1));
        finDeLaEps.ShouldBe(fechaInicial.AddDays(diasDeIncapacidad - 1));
    }
}
