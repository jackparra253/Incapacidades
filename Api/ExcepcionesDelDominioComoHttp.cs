using Bitakora;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api;

public class ExcepcionesDelDominioComoHttp : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public ExcepcionesDelDominioComoHttp(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext contexto, Exception excepcion, CancellationToken cancelacion)
    {
        int? estado = EstadoPara(excepcion);

        if (estado is null)
            return false;

        contexto.Response.StatusCode = estado.Value;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = contexto,
            Exception = excepcion,
            ProblemDetails = new ProblemDetails
            {
                Status = estado,
                Title = TituloPara(excepcion),
                Detail = excepcion.Message
            }
        });
    }

    private static int? EstadoPara(Exception excepcion)
    {
        return excepcion switch
        {
            SolicitudInvalida => StatusCodes.Status400BadRequest,
            NoEncontrado => StatusCodes.Status404NotFound,
            _ => null
        };
    }

    private static string TituloPara(Exception excepcion)
    {
        return excepcion switch
        {
            SolicitudInvalida => "La solicitud no es válida",
            NoEncontrado => "No se encontró lo que se pidió",
            _ => "Error"
        };
    }
}
