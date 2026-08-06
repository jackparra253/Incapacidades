using Datos;
using Modelos.Entidades;
using Modelos.Enumeracion;
using Shouldly;
using Xunit;

namespace Test.DatosTest;

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
}
