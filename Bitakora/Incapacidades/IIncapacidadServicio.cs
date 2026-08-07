using Bitakora.Liquidacion;

namespace Bitakora.Incapacidades;

public interface IIncapacidadServicio
{
    void Guardar(Incapacidad incapacidad);
    List<DetalleIncapacidad> ObtenerIncapacidadesDetalle(int idEmpleado);

    List<DetalleReconocimientoEconomico> ObtenerReconocimientosEconomicosDetalle(int idEmpleado);
}
