using Bitakora.Incapacidades;
using Bitakora.Liquidacion;
using Shouldly;
using Xunit;

namespace Test.Liquidacion;

public class ResponsablePagoServicioTest : TestBase
{
    private readonly ResponsablePagoServicio _responsablePagoServicio;

    public ResponsablePagoServicioTest()
    {
        _responsablePagoServicio = new ResponsablePagoServicio(Contexto);
    }

    [Fact]
    public void Debe_ObtenerResponsablesPago_RetornarTodosLosTramosConocidos()
    {
        List<ResponsablePago> responsablesPago = _responsablePagoServicio.ObtenerResponsablesPago();

        responsablesPago.Count.ShouldBe(8);
    }

    [Fact]
    public void Debe_ObtenerResponsablesPago_DescartarLosTramosQueEmpiezanDespues_Cuando_SeFiltraPorDuracion()
    {
        var tipoIncapacidad = TipoIncapacidad.EnfermedadGeneral;
        const int cantidadDias = 4;

        List<ResponsablePago> responsablesPagos = _responsablePagoServicio.ObtenerResponsablesPago(tipoIncapacidad, cantidadDias);

        responsablesPagos.Count.ShouldBe(2);
        responsablesPagos[0].Responsable.ShouldBe(Entidad.EMPRESA);
        responsablesPagos[1].Responsable.ShouldBe(Entidad.EPS);
    }

    [Fact]
    public void Debe_ObtenerResponsablesPago_IncluirAlFondoDePensiones_Cuando_LaIncapacidadPasaLosCientoOchentaDias()
    {
        var tipoIncapacidad = TipoIncapacidad.EnfermedadGeneral;
        const int cantidadDias = 200;

        List<ResponsablePago> responsablesPagos = _responsablePagoServicio.ObtenerResponsablesPago(tipoIncapacidad, cantidadDias);

        responsablesPagos.Count.ShouldBe(4);
        responsablesPagos[3].Responsable.ShouldBe(Entidad.FONDO_PENSIONES);
    }

    [Fact]
    public void Debe_ObtenerResponsablesPago_Fallar_Cuando_LaEnfermedadGeneralPasaElDiaQuinientosCuarenta()
    {
        var tipoIncapacidad = TipoIncapacidad.EnfermedadGeneral;
        const int cantidadDias = 600;

        Action consulta = () => _responsablePagoServicio.ObtenerResponsablesPago(tipoIncapacidad, cantidadDias);

        DiasSinResponsableDePago error = Should.Throw<DiasSinResponsableDePago>(consulta);

        error.UltimoDiaCubierto.ShouldBe(540);
        error.CantidadDias.ShouldBe(600);
    }

    [Theory]
    [InlineData(TipoIncapacidad.EnfermedadLaboral, 181, 180)]
    [InlineData(TipoIncapacidad.AccidenteLaboral, 181, 180)]
    [InlineData(TipoIncapacidad.LicenciaMaternidad, 127, 126)]
    [InlineData(TipoIncapacidad.LicenciaPaternidad, 9, 8)]
    public void Debe_ObtenerResponsablesPago_Fallar_Cuando_LaIncapacidadPasaElUltimoTramoDeSuTipo(
        TipoIncapacidad tipoIncapacidad, int cantidadDias, int ultimoDiaCubierto)
    {
        Action consulta = () => _responsablePagoServicio.ObtenerResponsablesPago(tipoIncapacidad, cantidadDias);

        DiasSinResponsableDePago error = Should.Throw<DiasSinResponsableDePago>(consulta);

        error.UltimoDiaCubierto.ShouldBe(ultimoDiaCubierto);
    }

    [Theory]
    [InlineData(TipoIncapacidad.EnfermedadGeneral, 540)]
    [InlineData(TipoIncapacidad.EnfermedadLaboral, 180)]
    [InlineData(TipoIncapacidad.AccidenteLaboral, 180)]
    [InlineData(TipoIncapacidad.LicenciaMaternidad, 126)]
    [InlineData(TipoIncapacidad.LicenciaPaternidad, 8)]
    public void Debe_ObtenerResponsablesPago_AceptarElUltimoDiaCubierto(TipoIncapacidad tipoIncapacidad, int cantidadDias)
    {
        List<ResponsablePago> responsablesPagos = _responsablePagoServicio.ObtenerResponsablesPago(tipoIncapacidad, cantidadDias);

        responsablesPagos[^1].DiasIncapacidadFinal.ShouldBe(cantidadDias);
    }
}
