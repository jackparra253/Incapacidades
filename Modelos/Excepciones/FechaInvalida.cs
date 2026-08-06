using System;

namespace Modelos.Excepciones;

public class FechaInvalida : Exception
{
    public int Anio { get; }
    public int Mes { get; }
    public int Dia { get; }

    public FechaInvalida(int anio, int mes, int dia)
        : base($"{anio}-{mes}-{dia} no es una fecha válida.")
    {
        Anio = anio;
        Mes = mes;
        Dia = dia;
    }
}
