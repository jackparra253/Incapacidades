using Bitakora.Incapacidades;

namespace Api.Incapacidades;

public static class IncapacidadesEnHttp
{
    public static void MapearIncapacidades(this IEndpointRouteBuilder rutas)
    {
        rutas.MapPost("/Incapacidad",
            (SolicitudIncapacidad solicitudIncapacidad, ICreadorIncapacidad creadorIncapacidad) =>
                creadorIncapacidad.Crear(solicitudIncapacidad));

        rutas.MapGet("/IncapacidadConsulta/{idEmpleado:int}",
            (int idEmpleado, IIncapacidadServicio incapacidadServicio) =>
                incapacidadServicio.ObtenerIncapacidadesDetalle(idEmpleado));

        rutas.MapGet("/CalcularFechas",
            (int anio, int mes, int dia, int cantidadDias, ICalcularFechas calcularFechas) =>
                calcularFechas.CalcularSiguienteFecha(new DateTime(anio, mes, dia), cantidadDias));
    }
}
