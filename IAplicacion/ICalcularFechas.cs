using System;

namespace IAplicacion
{
    public interface ICalcularFechas
    {
        DateTime CalcularSiguienteFecha(DateTime fechaInicial, int cantidadDias);
    }

}