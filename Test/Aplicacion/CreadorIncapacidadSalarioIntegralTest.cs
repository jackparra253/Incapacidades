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
    private readonly CreadorIncapacidad _creadorIncapacidad;

    public CreadorIncapacidadSalarioIntegralTest()
    {
        IResponsablePagoServicio responsablePagoServicio = new ResponsablePagoServicio(Contexto);
        IEmpleadoServicio empleadoServicio = new EmpleadoServicio(Contexto);
        IIncapacidadServicio incapacidadServicio = new IncapacidadServicio(Contexto);

        _creadorIncapacidad = new CreadorIncapacidad(responsablePagoServicio, empleadoServicio, incapacidadServicio);
    }

    [Fact]
    public void Debe_Crear_PersistirIncapacidad_Cuando_EsEnfermedadGeneralPorDosDiasSalarioIntegral_5Dias()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(1, 1, 2020, 06, 03, 5, "incapacidad del señor Alan");

        _creadorIncapacidad.Crear(solicitudIncapacidad);
        Incapacidad incapacidad = Contexto.Incapacidades.FirstOrDefault()!;

        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 06, 07));

        // Salario integral 15.000.000 -> IBC diario = (15.000.000 / 30) * 70% = 350.000.
        // El 30% prestacional se congela durante la incapacidad: no genera reconocimiento.
        incapacidad.ReconocimientosEconomicos.Count.ShouldBe(2);

        // Días 1-2: el empleador paga el 100% del IBC diario.
        (new Dinero(700_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[0].FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.ReconocimientosEconomicos[0].FechaFinal.ShouldBe(new DateTime(2020, 06, 04));
        incapacidad.ReconocimientosEconomicos[0].ResponsablePago.ShouldBe(Entidad.EMPRESA);

        // Días 3-5: la EPS paga el 66,66% del IBC diario -> 350.000 * 0,6666 * 3.
        (new Dinero(699_930m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[1].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[1].FechaInicial.ShouldBe(new DateTime(2020, 06, 05));
        incapacidad.ReconocimientosEconomicos[1].FechaFinal.ShouldBe(new DateTime(2020, 06, 07));
        incapacidad.ReconocimientosEconomicos[1].ResponsablePago.ShouldBe(Entidad.EPS);
    }

    // Regresion: con 4 dias la formula vieja arrancaba la EPS el 04/06, solapando un dia que la
    // empresa ya habia pagado, y dejaba el 06/06 sin cubrir.
    [Fact]
    public void Debe_Crear_EncadenarEmpresaYEpsSinTraslape_Cuando_EsEnfermedadGeneralSalarioIntegral_4Dias()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(1, 1, 2020, 06, 03, 4, "incapacidad del señor Alan");

        _creadorIncapacidad.Crear(solicitudIncapacidad);
        Incapacidad incapacidad = Contexto.Incapacidades.FirstOrDefault()!;

        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 06, 06));

        incapacidad.ReconocimientosEconomicos[0].FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.ReconocimientosEconomicos[0].FechaFinal.ShouldBe(new DateTime(2020, 06, 04));
        incapacidad.ReconocimientosEconomicos[0].ResponsablePago.ShouldBe(Entidad.EMPRESA);

        incapacidad.ReconocimientosEconomicos[1].FechaInicial.ShouldBe(new DateTime(2020, 06, 05));
        incapacidad.ReconocimientosEconomicos[1].FechaFinal.ShouldBe(new DateTime(2020, 06, 06));
        incapacidad.ReconocimientosEconomicos[1].ResponsablePago.ShouldBe(Entidad.EPS);

        incapacidad.ReconocimientosEconomicos.Count.ShouldBe(2);
    }

    [Fact]
    public void Debe_Crear_PersistirIncapacidad_Cuando_EsLicenciaMaternidadSalarioIntegral()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(1, 2, 2020, 06, 03, 126, "incapacidad del señor Alan");

        _creadorIncapacidad.Crear(solicitudIncapacidad);

        Incapacidad incapacidad = Contexto.Incapacidades.FirstOrDefault()!;

        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 10, 06));

        // La EPS cubre los 126 días al 100% del IBC diario. Ya no hay línea del 30% prestacional.
        incapacidad.ReconocimientosEconomicos.Count.ShouldBe(1);

        (new Dinero(44_100_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[0].FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.ReconocimientosEconomicos[0].FechaFinal.ShouldBe(new DateTime(2020, 10, 06));
        incapacidad.ReconocimientosEconomicos[0].ResponsablePago.ShouldBe(Entidad.EPS);
    }

    [Fact]
    public void Debe_Crear_PersistirIncapacidad_Cuando_EsLicenciaPaternidadSalarioIntegral()
    {
        var solicitudIncapacidad = new SolicitudIncapacidad(1, 3, 2020, 06, 03, 8, "incapacidad del señor Alan");

        _creadorIncapacidad.Crear(solicitudIncapacidad);

        Incapacidad incapacidad = Contexto.Incapacidades.FirstOrDefault()!;

        incapacidad.FechaIncial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.FechaFinal.ShouldBe(new DateTime(2020, 06, 10));

        incapacidad.ReconocimientosEconomicos.Count.ShouldBe(1);

        (new Dinero(2_800_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar).ShouldBeTrue();
        incapacidad.ReconocimientosEconomicos[0].FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        incapacidad.ReconocimientosEconomicos[0].FechaFinal.ShouldBe(new DateTime(2020, 06, 10));
        incapacidad.ReconocimientosEconomicos[0].ResponsablePago.ShouldBe(Entidad.EPS);
    }
}
