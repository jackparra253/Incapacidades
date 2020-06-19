namespace Modelos
{
    public class SolicitudIncapacidad
    {
        public int IdEmpleado { get; set; }
        public int TipoIncapacidad { get; private set; }
        public int Anio { get; private set; }
        public int Mes { get; private set; }
        public int Dia { get; set; }
        public int CantidadDias { get; private set; }
        public string Observaciones { get; private set; }

        public SolicitudIncapacidad(int idEmpleado, int tipoIncapacidad, int anio, int mes, int dia, int cantidadDias, string observaciones)
        {
            IdEmpleado = idEmpleado;
            TipoIncapacidad = tipoIncapacidad;
            Anio = anio;
            Mes = mes;
            Dia = dia;
            CantidadDias = cantidadDias;
            Observaciones = observaciones;
        }
    }
}