using System;

namespace Modelos.Excepciones;

public class TipoIncapacidadInvalido : Exception
{
    public int Valor { get; }

    public TipoIncapacidadInvalido(int valor)
        : base($"{valor} no corresponde a ningún tipo de incapacidad conocido.")
    {
        Valor = valor;
    }
}
