using Modelos.Enumeracion;
using Modelos.ValueObjects;

namespace Modelos.Entidades
{
    public class ReconocimientoEconomico
    {
        public int Id { get; private set; }
        public int IdIncapacidad { get; private set; }
        public int IdEmpleado { get; private set; }
        public Dinero ValorAPagar { get; private set; }
        public Entidad ResponsablePago { get; private set; }
        public Incapacidad Incapacidad { get; private set; }

        public ReconocimientoEconomico(int idIncapacidad, int idEmpleado, Dinero valorAPagar, Entidad responsablePago)
        {
            IdIncapacidad = idIncapacidad;
            IdEmpleado = idEmpleado;
            ValorAPagar = valorAPagar;
            ResponsablePago = responsablePago;
        }
    }
}