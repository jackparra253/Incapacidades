using Aplicacion;
using Datos;
using IDatos;
using Modelos;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.ValueObjects;
using Shouldly;
using Xunit;

namespace Test.Aplicacion;

public class CreadorIncapacidadSalarioOrdinarioTest : TestBase
{
    private readonly CreadorIncapacidad _creadorIncapacidad;

    public CreadorIncapacidadSalarioOrdinarioTest()
    {
        IResponsablePagoServicio responsablePagoServicio = new ResponsablePagoServicio(Contexto);
        IEmpleadoServicio empleadoServicio = new EmpleadoServicio(Contexto);
        IIncapacidadServicio incapacidadServicio = new IncapacidadServicio(Contexto);

        _creadorIncapacidad = new CreadorIncapacidad(responsablePagoServicio, empleadoServicio, incapacidadServicio);
    }

    [Fact]
    public void Debe_Crear_PersistirIncapacidad_Cuando_EsEnfermedadGeneralPorDosDiasSalarioLey50_4Dias()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(2, 1, 2020, 06, 03, 4, "incapacidad del Richard");

        _creadorIncapacidad.Crear(solicitudIncapacidad);
        Incapacidad incapacidad = Contexto.Incapacidades.FirstOrDefault()!;

        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 06, 06));
        (new Dinero(200_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[0].FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.ReconocimientosEconomicos[0].FechaFinal.ShouldBe(new DateTime(2020, 06, 04));
        // Salario ordinario 3.000.000 -> IBC diario = 100.000 (sin reducción del 70%).
        // Días 3-4: la EPS paga el 66,66% -> 100.000 * 0,6666 * 2.
        (new Dinero(133_320m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[1].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[1].FechaInicial.ShouldBe(new DateTime(2020, 06, 05));
        incapacidad.ReconocimientosEconomicos[1].FechaFinal.ShouldBe(new DateTime(2020, 06, 06));
    }

    // Regresion: con 5 dias la formula vieja arrancaba la EPS el 06/06, dejaba el 05/06 sin cubrir
    // y terminaba el 08/06, un dia despues del fin de la incapacidad.
    [Fact]
    public void Debe_Crear_EncadenarEmpresaYEpsSinHuecos_Cuando_EsEnfermedadGeneralSalarioLey50_5Dias()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(2, 1, 2020, 06, 03, 5, "incapacidad del Richard");

        _creadorIncapacidad.Crear(solicitudIncapacidad);
        Incapacidad incapacidad = Contexto.Incapacidades.FirstOrDefault()!;

        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 06, 07));

        incapacidad.ReconocimientosEconomicos[0].FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.ReconocimientosEconomicos[0].FechaFinal.ShouldBe(new DateTime(2020, 06, 04));

        incapacidad.ReconocimientosEconomicos[1].FechaInicial.ShouldBe(new DateTime(2020, 06, 05));
        incapacidad.ReconocimientosEconomicos[1].FechaFinal.ShouldBe(new DateTime(2020, 06, 07));
    }

    [Fact]
    public void Debe_Crear_PersistirIncapacidad_Cuando_EsLicenciaMaternidadSalarioLey50()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(2, 2, 2020, 06, 03, 126, "incapacidad del Richard");

        _creadorIncapacidad.Crear(solicitudIncapacidad);
        Incapacidad incapacidad = Contexto.Incapacidades.FirstOrDefault()!;

        (new Dinero(12_600_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar).ShouldBeTrue();
        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 10, 06));
    }

    [Fact]
    public void Debe_Crear_PersistirIncapacidad_Cuando_EsLicenciaPaternidadSalarioLey50()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(2, 3, 2020, 06, 03, 8, "incapacidad del Richard");

        _creadorIncapacidad.Crear(solicitudIncapacidad);
        Incapacidad incapacidad = Contexto.Incapacidades.FirstOrDefault()!;

        (new Dinero(800_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar).ShouldBeTrue();
        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 06, 10));
    }
}
