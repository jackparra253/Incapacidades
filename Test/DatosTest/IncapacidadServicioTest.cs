using Datos;
using Modelos;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.Enumeracion;
using Modelos.ValueObjects;
using Shouldly;
using Xunit;

namespace Test.DatosTest;

public class IncapacidadServicioTest : TestBase
{
    private readonly IncapacidadServicio _incapacidadServicio;

    public IncapacidadServicioTest()
    {
        var reconocimientoEconomicos = new ReconocimientoEconomico(1, new DateTime(2020, 06, 03), 2, new Dinero(500_000m, Moneda.COP), 1, Entidad.EMPRESA);

        var incapacidad = new Incapacidad(1, TipoIncapacidad.LicenciaMaternidad, new DateTime(2020, 06, 03), 2, "Test", new List<ReconocimientoEconomico> { reconocimientoEconomicos });

        Contexto.Add(incapacidad);
        Contexto.SaveChanges();

        _incapacidadServicio = new IncapacidadServicio(Contexto);
    }

    [Fact]
    public void Debe_ObtenerIncapacidadesDetalle_RetornarListaDetalleIncapacidad_Cuando_RecibeIdEmpleado()
    {
        var incapacidadesDetalleEsperadas = new List<DetalleIncapacidad>
        {
            new DetalleIncapacidad(1, "Licencia Maternidad", new DateTime(2020, 06, 03).ToShortDateString(), new DateTime(2020, 06, 04).ToShortDateString(), 2)
        };

        int idEmpleado = 1;

        List<DetalleIncapacidad> incapacidadesDetalle = _incapacidadServicio.ObtenerIncapacidadesDetalle(idEmpleado);

        incapacidadesDetalle[0].Tipo.ShouldBe(incapacidadesDetalleEsperadas[0].Tipo);
    }

    [Fact]
    public void Debe_ObtenerReconocimientosEconomicosDetalle_RetornarlistaReconocimientosEconomicos_Cuando_RecibeIdEmpleado()
    {
        var reconocimientoEconomicosDetalleEsperados = new List<DetalleReconocimientoEconomico>
        {
            new DetalleReconocimientoEconomico(1, new DateTime(2020, 06, 03).ToShortDateString(), new DateTime(2020, 06, 04).ToShortDateString(), new Dinero(1_000_000m, Moneda.COP), "EMPRESA")
        };

        int idEmpleado = 1;
        List<DetalleReconocimientoEconomico> reconocimientosEconomicosDetalle = _incapacidadServicio.ObtenerReconocimientosEconomicosDetalle(idEmpleado);

        reconocimientosEconomicosDetalle[0].ResponsablePago.ShouldBe(reconocimientoEconomicosDetalleEsperados[0].ResponsablePago);
        reconocimientosEconomicosDetalle[0].ValorAPagar.ShouldBe(reconocimientoEconomicosDetalleEsperados[0].ValorAPagar);
    }
}
