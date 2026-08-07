using Bitakora.Incapacidades;
using Bitakora.Liquidacion;
using Bitakora.Salarios;
using Shouldly;
using Xunit;

namespace Test.Incapacidades;

public class IncapacidadTest
{
    private const int Alan = 1;

    private static ReconocimientoEconomico Reconocimiento(decimal salarioDiario, int cantidadDias, Entidad responsable)
    {
        return new ReconocimientoEconomico(Alan, new DateTime(2020, 06, 03), cantidadDias,
            new Dinero(salarioDiario, Moneda.COP), 1, responsable, new Dinero(1m, Moneda.COP));
    }

    [Fact]
    public void Debe_TotalAPagar_SumarLosReconocimientosDeTodosLosResponsables()
    {
        var incapacidad = new Incapacidad(Alan, TipoIncapacidad.EnfermedadGeneral, new DateTime(2020, 06, 03), 4, "x",
            new List<ReconocimientoEconomico>
            {
                Reconocimiento(100_000m, 2, Entidad.EMPRESA),
                Reconocimiento(66_660m, 2, Entidad.EPS)
            });

        Dinero totalAPagar = incapacidad.TotalAPagar();

        totalAPagar.ShouldBe(new Dinero(333_320m, Moneda.COP));
    }

    [Fact]
    public void Debe_TotalAPagar_DevolverElUnicoReconocimiento_Cuando_HayUnSoloResponsable()
    {
        var incapacidad = new Incapacidad(Alan, TipoIncapacidad.LicenciaPaternidad, new DateTime(2020, 06, 03), 8, "x",
            new List<ReconocimientoEconomico> { Reconocimiento(100_000m, 8, Entidad.EPS) });

        Dinero totalAPagar = incapacidad.TotalAPagar();

        totalAPagar.ShouldBe(new Dinero(800_000m, Moneda.COP));
    }

    [Fact]
    public void Debe_Construir_Fallar_Cuando_NoHayNingunReconocimientoEconomico()
    {
        Action construccion = () => new Incapacidad(Alan, TipoIncapacidad.EnfermedadGeneral,
            new DateTime(2020, 06, 03), 4, "x", new List<ReconocimientoEconomico>());

        IncapacidadSinReconocimientos error = Should.Throw<IncapacidadSinReconocimientos>(construccion);

        error.IdEmpleado.ShouldBe(Alan);
    }
}
