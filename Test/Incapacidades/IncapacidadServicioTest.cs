using Bitakora.Incapacidades;
using Bitakora.Liquidacion;
using Bitakora.Salarios;
using Shouldly;
using Xunit;

namespace Test.Incapacidades;

public class IncapacidadServicioTest : TestBase
{
    private const int Alan = 1;

    private readonly IncapacidadServicio _incapacidadServicio;

    public IncapacidadServicioTest()
    {
        var reconocimientoEconomico = new ReconocimientoEconomico(Alan, new DateTime(2020, 06, 03), 2,
            new Dinero(500_000m, Moneda.COP), 1, Entidad.EMPRESA, new Dinero(29_260m, Moneda.COP));

        var incapacidad = new Incapacidad(Alan, TipoIncapacidad.LicenciaMaternidad, new DateTime(2020, 06, 03), 2, "Test",
            new List<ReconocimientoEconomico> { reconocimientoEconomico });

        Contexto.Add(incapacidad);
        Contexto.SaveChanges();

        _incapacidadServicio = new IncapacidadServicio(Contexto);
    }

    [Fact]
    public void Debe_ObtenerIncapacidadesDetalle_TraducirElTipoATexto()
    {
        List<DetalleIncapacidad> incapacidadesDetalle = _incapacidadServicio.ObtenerIncapacidadesDetalle(Alan);

        incapacidadesDetalle[0].Tipo.ShouldBe("Licencia Maternidad");
    }

    [Fact]
    public void Debe_ObtenerIncapacidadesDetalle_LlevarElTotalAPagar_Cuando_SeLeeDesdeLaBase()
    {
        Contexto.ChangeTracker.Clear();

        List<DetalleIncapacidad> incapacidadesDetalle = _incapacidadServicio.ObtenerIncapacidadesDetalle(Alan);

        incapacidadesDetalle[0].TotalAPagar.ShouldBe(new Dinero(1_000_000m, Moneda.COP));
    }

    [Fact]
    public void Debe_ObtenerReconocimientosEconomicosDetalle_TraducirElResponsableYConservarElValor()
    {
        List<DetalleReconocimientoEconomico> reconocimientosEconomicosDetalle = _incapacidadServicio.ObtenerReconocimientosEconomicosDetalle(Alan);

        reconocimientosEconomicosDetalle[0].ResponsablePago.ShouldBe("EMPRESA");
        reconocimientosEconomicosDetalle[0].ValorAPagar.ShouldBe(new Dinero(1_000_000m, Moneda.COP));
    }
}
