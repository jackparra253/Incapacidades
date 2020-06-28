using System.Collections.Generic;
using Modelos;
using Modelos.Entidades;

namespace IDatos
{
    public interface IIncapacidadServicio
    {
        void Guardar(Incapacidad incapacidad);
        List<DetalleIncapacidad> ObtenerIncapacidadesDetalle(int idEmpleado);

        List<DetalleReconocimientoEconomico> ObtenerReconocimientosEconomicosDetalle(int idEmpleado);
    }     
}
