using Bitakora.Incapacidades;

namespace Api.Incapacidades;

public static class IncapacidadesEnHttp
{
    public static void MapearIncapacidades(this IEndpointRouteBuilder rutas)
    {
        rutas.MapPost("/Incapacidad",
                (SolicitudIncapacidad solicitudIncapacidad, ICreadorIncapacidad creadorIncapacidad) =>
                    creadorIncapacidad.Crear(solicitudIncapacidad))
            .WithSummary("Liquida una incapacidad y la guarda con sus reconocimientos económicos")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        rutas.MapGet("/IncapacidadConsulta/{idEmpleado:int}",
                (int idEmpleado, IIncapacidadServicio incapacidadServicio) =>
                    incapacidadServicio.ObtenerIncapacidadesDetalle(idEmpleado))
            .WithSummary("Incapacidades de un empleado, con el total a pagar de cada una")
            .Produces<List<DetalleIncapacidad>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        rutas.MapGet("/CalcularFechas",
                (int anio, int mes, int dia, int cantidadDias, ICalcularFechas calcularFechas) =>
                    calcularFechas.CalcularSiguienteFecha(new DateTime(anio, mes, dia), cantidadDias))
            .WithSummary("Fecha en que termina un período que empieza en la fecha dada")
            .Produces<DateTime>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
