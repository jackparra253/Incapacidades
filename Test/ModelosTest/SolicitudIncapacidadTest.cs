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
        const int enfermedadGeneral = 1;

        var solicitud = new SolicitudIncapacidad(2, enfermedadGeneral, 2020, 06, 03, 4, "incapacidad del Richard");

        solicitud.FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        solicitud.TipoDeIncapacidad.ShouldBe(TipoIncapacidad.EnfermedadGeneral);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(99)]
    [InlineData(-1)]
    [InlineData(300)]
    public void Debe_Construir_Fallar_Cuando_ElTipoDeIncapacidadNoExiste(int tipoIncapacidad)
    {
        Action construccion = () => new SolicitudIncapacidad(2, tipoIncapacidad, 2020, 06, 03, 4, "x");

        TipoIncapacidadInvalido error = Should.Throw<TipoIncapacidadInvalido>(construccion);

        error.Valor.ShouldBe(tipoIncapacidad);
    }

    [Theory]
    [InlineData(2020, 13, 01)]
    [InlineData(2020, 02, 30)]
    [InlineData(2021, 02, 29)]
    [InlineData(2020, 00, 10)]
    public void Debe_Construir_Fallar_Cuando_LaFechaNoExiste(int anio, int mes, int dia)
    {
        Action construccion = () => new SolicitudIncapacidad(2, 1, anio, mes, dia, 4, "x");

        Should.Throw<FechaInvalida>(construccion);
    }

    [Fact]
    public void Debe_Construir_AceptarUnAnioBisiesto()
    {
        var solicitud = new SolicitudIncapacidad(2, 1, 2020, 02, 29, 4, "x");

        DateTime fechaInicial = solicitud.FechaInicial;

        fechaInicial.ShouldBe(new DateTime(2020, 02, 29));
    }

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
        var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        SolicitudIncapacidad solicitud = JsonSerializer.Deserialize<SolicitudIncapacidad>(json, opciones)!;

        solicitud.IdEmpleado.ShouldBe(2);
        solicitud.CantidadDias.ShouldBe(4);
        solicitud.Observaciones.ShouldBe("incapacidad del Richard");
        solicitud.FechaInicial.ShouldBe(new DateTime(2020, 06, 03));
        solicitud.TipoDeIncapacidad.ShouldBe(TipoIncapacidad.EnfermedadGeneral);
    }
}
