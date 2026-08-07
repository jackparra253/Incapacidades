using Api;
using Bitakora.Empleados;
using Bitakora.Incapacidades;
using Bitakora.Liquidacion;
using Bitakora.Salarios;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Test.Api;

public class ExcepcionesDelDominioComoHttpTest
{
    private readonly ExcepcionesDelDominioComoHttp _manejador;

    public ExcepcionesDelDominioComoHttpTest()
    {
        var problemDetails = Substitute.For<IProblemDetailsService>();
        problemDetails.TryWriteAsync(Arg.Any<ProblemDetailsContext>()).Returns(ValueTask.FromResult(true));

        _manejador = new ExcepcionesDelDominioComoHttp(problemDetails);
    }

    public static TheoryData<Exception> SolicitudesInvalidas() => new()
    {
        new CantidadDiasInvalida(0),
        new FechaInvalida(2020, 02, 30),
        new TipoIncapacidadInvalido(99),
        new DiasSinResponsableDePago(TipoIncapacidad.EnfermedadGeneral, 600, 540)
    };

    [Theory]
    [MemberData(nameof(SolicitudesInvalidas))]
    public async Task Debe_TryHandleAsync_Responder400_Cuando_ElQueLlamaMandoAlgoQueNoSirve(Exception excepcion)
    {
        var contexto = new DefaultHttpContext();

        bool manejada = await _manejador.TryHandleAsync(contexto, excepcion, CancellationToken.None);

        manejada.ShouldBeTrue();
        contexto.Response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Debe_TryHandleAsync_Responder404_Cuando_NoExisteLoQueSePidio()
    {
        var contexto = new DefaultHttpContext();

        bool manejada = await _manejador.TryHandleAsync(contexto, new EmpleadoNoEncontrado(999), CancellationToken.None);

        manejada.ShouldBeTrue();
        contexto.Response.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
    }

    public static TheoryData<Exception> FallasInternas() => new()
    {
        new SalarioMinimoDesconocido(2019),
        new MonedaInvalida("XX"),
        new MonedasIncompatibles("COP", "USD"),
        new IncapacidadSinReconocimientos(1, new DateTime(2020, 06, 03)),
        new InvalidOperationException("cualquier otra")
    };

    [Theory]
    [MemberData(nameof(FallasInternas))]
    public async Task Debe_TryHandleAsync_NoManejar_Cuando_LaFallaNoEsCulpaDelQueLlama(Exception excepcion)
    {
        var contexto = new DefaultHttpContext();

        bool manejada = await _manejador.TryHandleAsync(contexto, excepcion, CancellationToken.None);

        manejada.ShouldBeFalse();
        contexto.Response.StatusCode.ShouldBe(StatusCodes.Status200OK);
    }
}
