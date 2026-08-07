using System;

namespace Modelos.Excepciones;

public class IncapacidadSinReconocimientos : Exception
{
    public int IdEmpleado { get; }
    public DateTime FechaInicial { get; }

    public IncapacidadSinReconocimientos(int idEmpleado, DateTime fechaInicial)
        : base($"La incapacidad del empleado {idEmpleado} que empieza el {fechaInicial:d} no tiene " +
               "ningún reconocimiento económico, así que no hay nada que liquidar.")
    {
        IdEmpleado = idEmpleado;
        FechaInicial = fechaInicial;
    }
}
