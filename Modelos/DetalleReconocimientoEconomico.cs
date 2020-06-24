using Modelos.ValueObjects;

namespace Modelos
{
    public class DetalleReconocimientoEconomico
    {
        public int IdIncapacidad { get; private set; }
        public string FechaInicial { get; private set; }
        public string FechaFinal { get; private set; }
        public Dinero ValorAPagar { get; private set; }
        public string ResponsablePago { get; private set; }
        


        public DetalleReconocimientoEconomico(int idIncapacidad, string fechaInicial, string fechaFinal, Dinero valorAPagar, string responsablePago)
        {
            IdIncapacidad = idIncapacidad;
            FechaInicial = fechaInicial;
            FechaFinal = fechaFinal;
            ValorAPagar = valorAPagar;
            ResponsablePago = responsablePago;
            
        }
    }
}