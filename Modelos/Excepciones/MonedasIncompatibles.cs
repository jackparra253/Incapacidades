using System;

namespace Modelos.Excepciones
{
    public class MonedasIncompatibles : Exception
    {
        public string Una { get; }
        public string Otra { get; }

        public MonedasIncompatibles(string una, string otra)
            : base($"No se puede operar entre monedas distintas: {una} y {otra}.")
        {
            Una = una;
            Otra = otra;
        }
    }
}
