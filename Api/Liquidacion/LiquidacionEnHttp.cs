using Bitakora.Incapacidades;
using Bitakora.Liquidacion;

namespace Api.Liquidacion;

public static class LiquidacionEnHttp
{
    public static void MapearLiquidacion(this IEndpointRouteBuilder rutas)
    {
        rutas.MapGet("/ReconocimientoEconomico/{idEmpleado:int}",
                (int idEmpleado, IIncapacidadServicio incapacidadServicio) =>
                    incapacidadServicio.ObtenerReconocimientosEconomicosDetalle(idEmpleado))
            .WithSummary("Reconocimientos económicos de un empleado, con su responsable de pago")
            .Produces<List<DetalleReconocimientoEconomico>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
