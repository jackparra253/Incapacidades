using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Shouldly;
using Xunit;

namespace Test.Api;

public class EndpointsTest : IClassFixture<ApiDePrueba>
{
    private const int Richard = 2;

    private readonly HttpClient _cliente;

    public EndpointsTest(ApiDePrueba api)
    {
        _cliente = api.CreateClient();
    }

    private static object Solicitud(int idEmpleado = Richard, int tipoIncapacidad = 1, int anio = 2020,
        int mes = 6, int dia = 3, int cantidadDias = 4)
    {
        return new { idEmpleado, tipoIncapacidad, anio, mes, dia, cantidadDias, observaciones = "x" };
    }

    private async Task<JsonElement> Leer(HttpResponseMessage respuesta)
    {
        string cuerpo = await respuesta.Content.ReadAsStringAsync();

        return JsonDocument.Parse(cuerpo).RootElement;
    }

    [Fact]
    public async Task Debe_Empleado_ListarLosEmpleados()
    {
        HttpResponseMessage respuesta = await _cliente.GetAsync("/Empleado");

        respuesta.StatusCode.ShouldBe(HttpStatusCode.OK);
        JsonElement empleados = await Leer(respuesta);
        empleados.GetArrayLength().ShouldBe(2);
        empleados[0].GetProperty("nombres").GetString().ShouldBe("Alan");
        empleados[1].GetProperty("nombres").GetString().ShouldBe("Richard");
    }

    [Fact]
    public async Task Debe_CalcularFechas_DevolverLaFechaComoStringJson()
    {
        HttpResponseMessage respuesta = await _cliente.GetAsync("/CalcularFechas/?anio=2020&mes=6&dia=3&cantidadDias=4");

        respuesta.StatusCode.ShouldBe(HttpStatusCode.OK);
        JsonElement fechaFinal = await Leer(respuesta);
        fechaFinal.ValueKind.ShouldBe(JsonValueKind.String);
        fechaFinal.GetString()!.Substring(0, 10).ShouldBe("2020-06-06");
    }

    [Fact]
    public async Task Debe_Incapacidad_Crear_Cuando_LaSolicitudEsValida()
    {
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/Incapacidad", Solicitud());

        respuesta.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Debe_Incapacidad_Responder400ConElMensajeDelDominio_Cuando_LaCantidadDeDiasEsCero()
    {
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/Incapacidad", Solicitud(cantidadDias: 0));

        respuesta.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        JsonElement problema = await Leer(respuesta);
        problema.GetProperty("detail").GetString().ShouldBe("Una incapacidad dura al menos un día, y se pidieron 0.");
    }

    [Fact]
    public async Task Debe_Incapacidad_Responder400_Cuando_LaDuracionPasaElUltimoTramo()
    {
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/Incapacidad", Solicitud(cantidadDias: 600));

        respuesta.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        JsonElement problema = await Leer(respuesta);
        problema.GetProperty("detail").GetString()!.ShouldContain("540");
    }

    [Fact]
    public async Task Debe_Incapacidad_Responder404_Cuando_ElEmpleadoNoExiste()
    {
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/Incapacidad", Solicitud(idEmpleado: 999));

        respuesta.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        JsonElement problema = await Leer(respuesta);
        problema.GetProperty("detail").GetString().ShouldBe("No existe un empleado con el id 999.");
    }

    [Fact]
    public async Task Debe_Incapacidad_Responder500SinFiltrarElMensaje_Cuando_FallaAlgoInterno()
    {
        HttpResponseMessage respuesta = await _cliente.PostAsJsonAsync("/Incapacidad", Solicitud(anio: 2019));

        respuesta.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        (await respuesta.Content.ReadAsStringAsync()).ShouldNotContain("salario mínimo");
    }

    [Fact]
    public async Task Debe_IncapacidadConsulta_LlevarElTotalAPagar()
    {
        await _cliente.PostAsJsonAsync("/Incapacidad", Solicitud());

        HttpResponseMessage respuesta = await _cliente.GetAsync($"/IncapacidadConsulta/{Richard}");

        respuesta.StatusCode.ShouldBe(HttpStatusCode.OK);
        JsonElement incapacidades = await Leer(respuesta);
        incapacidades.GetArrayLength().ShouldBeGreaterThan(0);
        incapacidades[0].GetProperty("totalAPagar").GetProperty("moneda").GetString().ShouldBe("COP");
    }

    [Fact]
    public async Task Debe_ReconocimientoEconomico_ListarLosDeUnEmpleado()
    {
        await _cliente.PostAsJsonAsync("/Incapacidad", Solicitud());

        HttpResponseMessage respuesta = await _cliente.GetAsync($"/ReconocimientoEconomico/{Richard}");

        respuesta.StatusCode.ShouldBe(HttpStatusCode.OK);
        JsonElement reconocimientos = await Leer(respuesta);
        reconocimientos.GetArrayLength().ShouldBeGreaterThan(0);
        reconocimientos[0].GetProperty("responsablePago").GetString().ShouldBe("EMPRESA");
    }
}
