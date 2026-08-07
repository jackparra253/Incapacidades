using System;

namespace Modelos.Excepciones;

public class SalarioMinimoDesconocido : Exception
{
    public int Anio { get; }

    public SalarioMinimoDesconocido(int anio)
        : base($"No se conoce el salario mínimo legal vigente para {anio}, así que no se puede " +
               "verificar el piso de la incapacidad.")
    {
        Anio = anio;
    }
}
