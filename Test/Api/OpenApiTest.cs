using System.Net;
using System.Text.Json;
using Shouldly;
using Xunit;

namespace Test.Api;

public class OpenApiTest : IClassFixture<ApiDePrueba>
{
    private readonly HttpClient _cliente;

    public OpenApiTest(ApiDePrueba api)
    {
        _cliente = api.CreateClient();
    }

    private async Task<JsonElement> Documento()
    {
        HttpResponseMessage respuesta = await _cliente.GetAsync("/openapi/v1.json");

        respuesta.StatusCode.ShouldBe(HttpStatusCode.OK);

        return JsonDocument.Parse(await respuesta.Content.ReadAsStringAsync()).RootElement;
    }

    [Fact]
    public async Task Debe_Servir_UnDocumentoOpenApi31()
    {
        JsonElement documento = await Documento();

        documento.GetProperty("openapi").GetString()!.ShouldStartWith("3.1");
        documento.GetProperty("info").GetProperty("title").GetString().ShouldBe("Bitákora — Incapacidades");
    }

    [Theory]
    [InlineData("/Empleado", "get")]
    [InlineData("/Incapacidad", "post")]
    [InlineData("/IncapacidadConsulta/{idEmpleado}", "get")]
    [InlineData("/CalcularFechas", "get")]
    [InlineData("/ReconocimientoEconomico/{idEmpleado}", "get")]
    public async Task Debe_Documentar_CadaRutaQueLaApiExpone(string ruta, string verbo)
    {
        JsonElement paths = (await Documento()).GetProperty("paths");

        paths.TryGetProperty(ruta, out JsonElement path).ShouldBeTrue($"falta la ruta {ruta}");
        path.TryGetProperty(verbo, out _).ShouldBeTrue($"falta el verbo {verbo} en {ruta}");
    }

    [Fact]
    public async Task Debe_Documentar_LosTresErroresDeCrearIncapacidad()
    {
        JsonElement respuestas = (await Documento())
            .GetProperty("paths").GetProperty("/Incapacidad").GetProperty("post").GetProperty("responses");

        respuestas.TryGetProperty("400", out _).ShouldBeTrue();
        respuestas.TryGetProperty("404", out _).ShouldBeTrue();
        respuestas.TryGetProperty("500", out _).ShouldBeTrue();
    }

    [Theory]
    [InlineData("/Empleado", "get")]
    [InlineData("/IncapacidadConsulta/{idEmpleado}", "get")]
    [InlineData("/CalcularFechas", "get")]
    [InlineData("/ReconocimientoEconomico/{idEmpleado}", "get")]
    public async Task Debe_NoDocumentar404_Cuando_ElEndpointNoResuelveUnEmpleado(string ruta, string verbo)
    {
        JsonElement respuestas = (await Documento())
            .GetProperty("paths").GetProperty(ruta).GetProperty(verbo).GetProperty("responses");

        respuestas.TryGetProperty("404", out _).ShouldBeFalse($"{ruta} declara un 404 que nunca devuelve");
    }

    [Fact]
    public async Task Debe_Describir_ElEmpleadoComoLoConsumeElFront()
    {
        JsonElement empleado = (await Documento())
            .GetProperty("components").GetProperty("schemas").GetProperty("Empleado").GetProperty("properties");

        empleado.TryGetProperty("nombres", out _).ShouldBeTrue();
        empleado.TryGetProperty("salario", out _).ShouldBeTrue();
        empleado.TryGetProperty("salarioDiario", out _).ShouldBeTrue();
        empleado.GetProperty("tipoSalario").GetProperty("$ref").GetString().ShouldEndWith("/TipoSalario");
    }

    [Fact]
    public async Task Debe_Describir_ElDineroConCantidadYMoneda()
    {
        JsonElement dinero = (await Documento())
            .GetProperty("components").GetProperty("schemas").GetProperty("Dinero").GetProperty("properties");

        dinero.TryGetProperty("cantidad", out _).ShouldBeTrue();
        dinero.TryGetProperty("moneda", out _).ShouldBeTrue();
    }
}
