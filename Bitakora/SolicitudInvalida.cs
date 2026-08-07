using System;

namespace Bitakora;

public abstract class SolicitudInvalida : Exception
{
    protected SolicitudInvalida(string mensaje) : base(mensaje)
    {
    }
}
