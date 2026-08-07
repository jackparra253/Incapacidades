using Bitakora.Incapacidades;

namespace Api.Liquidacion;

public static class LiquidacionEnHttp
{
    public static void MapearLiquidacion(this IEndpointRouteBuilder rutas)
    {
        rutas.MapGet("/ReconocimientoEconomico/{idEmpleado:int}",
            (int idEmpleado, IIncapacidadServicio incapacidadServicio) =>
                incapacidadServicio.ObtenerReconocimientosEconomicosDetalle(idEmpleado));
    }
}
