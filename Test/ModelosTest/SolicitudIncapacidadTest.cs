using System.Text.Json;
using Modelos;
using Modelos.Enumeracion;
using Modelos.Excepciones;
using Shouldly;
using Xunit;

namespace Test.ModelosTest;

public class SolicitudIncapacidadTest
{
    [Fact]
    public void Debe_Construir_ExponerLaFechaYElTipoYaConvertidos()
    {
        var solicitud = new SolicitudIncapacidad(2, 1, 2020, 06, 03, 4, "incapacidad del Richard");

        solicitud.FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        solicitud.TipoDeIncapacidad.ShouldBe(TipoIncapacidad.EnfermedadGeneral);
    }

    // Antes, un tipo inválido se casteaba sin error, no matcheaba ningún ResponsablePago, y la
    // incapacidad se persistía sin reconocimientos económicos y sin aviso.
    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(99)]
    [InlineData(-1)]
    [InlineData(300)]
    public void Debe_Construir_Fallar_Cuando_ElTipoDeIncapacidadNoExiste(int tipoIncapacidad)
    {
        var error = Should.Throw<TipoIncapacidadInvalido>(
            () => new SolicitudIncapacidad(2, tipoIncapacidad, 2020, 06, 03, 4, "x"));

        error.Valor.ShouldBe(tipoIncapacidad);
    }

    [Theory]
    [InlineData(2020, 13, 01)]
    [InlineData(2020, 02, 30)]
    [InlineData(2021, 02, 29)]
    [InlineData(2020, 00, 10)]
    public void Debe_Construir_Fallar_Cuando_LaFechaNoExiste(int anio, int mes, int dia)
    {
        Should.Throw<FechaInvalida>(() => new SolicitudIncapacidad(2, 1, anio, mes, dia, 4, "x"));
    }

    [Fact]
    public void Debe_Construir_AceptarUnAnioBisiesto()
    {
        new SolicitudIncapacidad(2, 1, 2020, 02, 29, 4, "x").FechaInicial
            .ShouldBe(new DateTime(2020, 02, 29));
    }

    // Los setters pasaron a private: hay que garantizar que el payload que manda el front
    // (Api/wwwroot/js/site.js) siga deserializando por el constructor.
    [Fact]
    public void Debe_Deserializar_ElPayloadDelFront()
    {
        const string json = """
            {
              "idEmpleado": 2,
              "tipoIncapacidad": 1,
              "anio": 2020,
              "mes": 6,
              "dia": 3,
              "cantidadDias": 4,
              "observaciones": "incapacidad del Richard"
            }
            """;

        SolicitudIncapacidad solicitud = JsonSerializer.Deserialize<SolicitudIncapacidad>(
            json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        solicitud.IdEmpleado.ShouldBe(2);
        solicitud.CantidadDias.ShouldBe(4);
        solicitud.Observaciones.ShouldBe("incapacidad del Richard");
        solicitud.FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        solicitud.TipoDeIncapacidad.ShouldBe(TipoIncapacidad.EnfermedadGeneral);
    }
}
