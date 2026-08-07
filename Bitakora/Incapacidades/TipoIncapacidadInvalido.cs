using System;

namespace Bitakora.Incapacidades;

public class TipoIncapacidadInvalido : SolicitudInvalida
{
    public int Valor { get; }

    public TipoIncapacidadInvalido(int valor)
        : base($"{valor} no corresponde a ningún tipo de incapacidad conocido.")
    {
        Valor = valor;
    }
}
