namespace Modelos
{
    public class DetalleIncapacidad
    {
        public int Id { get; private set; }
        public string Tipo { get; private set; }
        public string FechaInicial { get; private set; }
        public string FechaFinal { get; private set; }
        public int CantidadDias { get; private set; }

        public DetalleIncapacidad(int id,string tipo,string fechaInicial, string fechaFinal, int cantidadDias)
        {
            Id = id;
            Tipo = tipo;
            FechaInicial = fechaInicial;
            FechaFinal = fechaFinal;
            CantidadDias = cantidadDias;
        }

    }
}