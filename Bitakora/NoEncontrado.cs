using System;

namespace Bitakora;

public abstract class NoEncontrado : Exception
{
    protected NoEncontrado(string mensaje) : base(mensaje)
    {
    }
}
