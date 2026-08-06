using Aplicacion;
using Datos;
using IDatos;
using Modelos;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.Enumeracion;
using Modelos.ValueObjects;
using Shouldly;
using Xunit;

namespace Test.Aplicacion;

public class CreadorIncapacidadSalarioIntegralTest : TestBase
{
    private const int Alan = 1;
    private const int EnfermedadGeneral = 1;
    private const int LicenciaMaternidad = 2;
    private const int LicenciaPaternidad = 3;

    private readonly CreadorIncapacidad _creadorIncapacidad;

    public CreadorIncapacidadSalarioIntegralTest()
    {
        IResponsablePagoServicio responsablePagoServicio = new ResponsablePagoServicio(Contexto);
        IEmpleadoServicio empleadoServicio = new EmpleadoServicio(Contexto);
        IIncapacidadServicio incapacidadServicio = new IncapacidadServicio(Contexto);
        ISalarioMinimoServicio salarioMinimoServicio = new SalarioMinimoServicio();

        _creadorIncapacidad = new CreadorIncapacidad(responsablePagoServicio, empleadoServicio, incapacidadServicio, salarioMinimoServicio);
    }

    private Incapacidad IncapacidadPersistida() => Contexto.Incapacidades.FirstOrDefault()!;

    [Fact]
    public void Debe_Crear_LiquidarSobreElIbcSinLineaPrestacional_Cuando_EsEnfermedadGeneralDe5Dias()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(Alan, EnfermedadGeneral, 2020, 06, 03, 5, "incapacidad del señor Alan");

        _creadorIncapacidad.Crear(solicitudIncapacidad);

        Incapacidad incapacidad = IncapacidadPersistida();
        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 06, 07));
        incapacidad.ReconocimientosEconomicos.Count.ShouldBe(2);
        (new Dinero(700_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[0].FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.ReconocimientosEconomicos[0].FechaFinal.ShouldBe(new DateTime(2020, 06, 04));
        incapacidad.ReconocimientosEconomicos[0].ResponsablePago.ShouldBe(Entidad.EMPRESA);
        (new Dinero(699_930m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[1].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[1].FechaInicial.ShouldBe(new DateTime(2020, 06, 05));
        incapacidad.ReconocimientosEconomicos[1].FechaFinal.ShouldBe(new DateTime(2020, 06, 07));
        incapacidad.ReconocimientosEconomicos[1].ResponsablePago.ShouldBe(Entidad.EPS);
    }

    [Fact]
    public void Debe_Crear_EncadenarEmpresaYEpsSinTraslape_Cuando_EsEnfermedadGeneralDe4Dias()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(Alan, EnfermedadGeneral, 2020, 06, 03, 4, "incapacidad del señor Alan");

        _creadorIncapacidad.Crear(solicitudIncapacidad);

        Incapacidad incapacidad = IncapacidadPersistida();
        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 06, 06));
        incapacidad.ReconocimientosEconomicos.Count.ShouldBe(2);
        incapacidad.ReconocimientosEconomicos[0].FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.ReconocimientosEconomicos[0].FechaFinal.ShouldBe(new DateTime(2020, 06, 04));
        incapacidad.ReconocimientosEconomicos[0].ResponsablePago.ShouldBe(Entidad.EMPRESA);
        incapacidad.ReconocimientosEconomicos[1].FechaInicial.ShouldBe(new DateTime(2020, 06, 05));
        incapacidad.ReconocimientosEconomicos[1].FechaFinal.ShouldBe(new DateTime(2020, 06, 06));
        incapacidad.ReconocimientosEconomicos[1].ResponsablePago.ShouldBe(Entidad.EPS);
    }

    [Fact]
    public void Debe_Crear_DejarTodoACargoDeLaEps_Cuando_EsLicenciaMaternidad()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(Alan, LicenciaMaternidad, 2020, 06, 03, 126, "incapacidad del señor Alan");

        _creadorIncapacidad.Crear(solicitudIncapacidad);

        Incapacidad incapacidad = IncapacidadPersistida();
        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 10, 06));
        incapacidad.ReconocimientosEconomicos.Count.ShouldBe(1);
        (new Dinero(44_100_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[0].FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.ReconocimientosEconomicos[0].FechaFinal.ShouldBe(new DateTime(2020, 10, 06));
        incapacidad.ReconocimientosEconomicos[0].ResponsablePago.ShouldBe(Entidad.EPS);
    }

    [Fact]
    public void Debe_Crear_DejarTodoACargoDeLaEps_Cuando_EsLicenciaPaternidad()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(Alan, LicenciaPaternidad, 2020, 06, 03, 8, "incapacidad del señor Alan");

        _creadorIncapacidad.Crear(solicitudIncapacidad);

        Incapacidad incapacidad = IncapacidadPersistida();
        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 06, 10));
        incapacidad.ReconocimientosEconomicos.Count.ShouldBe(1);
        (new Dinero(2_800_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[0].FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.ReconocimientosEconomicos[0].FechaFinal.ShouldBe(new DateTime(2020, 06, 10));
        incapacidad.ReconocimientosEconomicos[0].ResponsablePago.ShouldBe(Entidad.EPS);
    }
}
