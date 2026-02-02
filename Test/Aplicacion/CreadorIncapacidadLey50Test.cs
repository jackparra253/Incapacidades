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

public class CreadorIncapacidadLey50Test : TestBase
{
    private readonly CreadorIncapacidadLey50 _creadorIncapacidad;

    public CreadorIncapacidadLey50Test()
    {
        IResponsablePagoServicio responsablePagoServicio = new ResponsablePagoServicio(Contexto);
        IEmpleadoServicio empleadoServicio = new EmpleadoServicio(Contexto);
        IIncapacidadServicio incapacidadServicio = new IncapacidadServicio(Contexto);

        _creadorIncapacidad = new CreadorIncapacidadLey50(responsablePagoServicio, empleadoServicio, incapacidadServicio);
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
        (new Dinero(133_340m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[1].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[1].FechaInicial.ShouldBe(new DateTime(2020, 06, 05));
        incapacidad.ReconocimientosEconomicos[1].FechaFinal.ShouldBe(new DateTime(2020, 06, 06));
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
