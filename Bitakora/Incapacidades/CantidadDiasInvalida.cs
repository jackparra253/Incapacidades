using System;

namespace Bitakora.Incapacidades;

public class CantidadDiasInvalida : Exception
{
    public int Valor { get; }

    public CantidadDiasInvalida(int valor)
        : base($"Una incapacidad dura al menos un día, y se pidieron {valor}.")
    {
        Valor = valor;
    }
}
