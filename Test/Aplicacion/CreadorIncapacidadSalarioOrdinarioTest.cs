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
    private const int Richard = 2;
    private const int EnfermedadGeneral = 1;
    private const int LicenciaMaternidad = 2;
    private const int LicenciaPaternidad = 3;

    private readonly CreadorIncapacidad _creadorIncapacidad;

    public CreadorIncapacidadSalarioOrdinarioTest()
    {
        IResponsablePagoServicio responsablePagoServicio = new ResponsablePagoServicio(Contexto);
        IEmpleadoServicio empleadoServicio = new EmpleadoServicio(Contexto);
        IIncapacidadServicio incapacidadServicio = new IncapacidadServicio(Contexto);
        ISalarioMinimoServicio salarioMinimoServicio = new SalarioMinimoServicio();

        _creadorIncapacidad = new CreadorIncapacidad(responsablePagoServicio, empleadoServicio, incapacidadServicio, salarioMinimoServicio);
    }

    private Incapacidad IncapacidadPersistida() => Contexto.Incapacidades.FirstOrDefault()!;

    [Fact]
    public void Debe_Crear_RepartirEntreEmpresaYEps_Cuando_EsEnfermedadGeneralDe4Dias()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(Richard, EnfermedadGeneral, 2020, 06, 03, 4, "incapacidad del Richard");

        _creadorIncapacidad.Crear(solicitudIncapacidad);

        Incapacidad incapacidad = IncapacidadPersistida();
        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 06, 06));
        (new Dinero(200_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[0].FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.ReconocimientosEconomicos[0].FechaFinal.ShouldBe(new DateTime(2020, 06, 04));
        (new Dinero(133_320m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[1].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[1].FechaInicial.ShouldBe(new DateTime(2020, 06, 05));
        incapacidad.ReconocimientosEconomicos[1].FechaFinal.ShouldBe(new DateTime(2020, 06, 06));
    }

    [Fact]
    public void Debe_Crear_EncadenarEmpresaYEpsSinHuecos_Cuando_EsEnfermedadGeneralDe5Dias()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(Richard, EnfermedadGeneral, 2020, 06, 03, 5, "incapacidad del Richard");

        _creadorIncapacidad.Crear(solicitudIncapacidad);

        Incapacidad incapacidad = IncapacidadPersistida();
        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 06, 07));
        incapacidad.ReconocimientosEconomicos[0].FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.ReconocimientosEconomicos[0].FechaFinal.ShouldBe(new DateTime(2020, 06, 04));
        incapacidad.ReconocimientosEconomicos[1].FechaInicial.ShouldBe(new DateTime(2020, 06, 05));
        incapacidad.ReconocimientosEconomicos[1].FechaFinal.ShouldBe(new DateTime(2020, 06, 07));
    }

    [Fact]
    public void Debe_Crear_DejarTodoACargoDeLaEps_Cuando_EsLicenciaMaternidad()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(Richard, LicenciaMaternidad, 2020, 06, 03, 126, "incapacidad del Richard");

        _creadorIncapacidad.Crear(solicitudIncapacidad);

        Incapacidad incapacidad = IncapacidadPersistida();
        (new Dinero(12_600_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar).ShouldBeTrue();
        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 10, 06));
    }

    [Fact]
    public void Debe_Crear_DejarTodoACargoDeLaEps_Cuando_EsLicenciaPaternidad()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(Richard, LicenciaPaternidad, 2020, 06, 03, 8, "incapacidad del Richard");

        _creadorIncapacidad.Crear(solicitudIncapacidad);

        Incapacidad incapacidad = IncapacidadPersistida();
        (new Dinero(800_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar).ShouldBeTrue();
        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 06, 10));
    }
}
