using System;

namespace Modelos.Excepciones
{
    public class MonedaInvalida : Exception
    {
        public string Moneda { get; }

        public MonedaInvalida(string moneda)
            : base($"'{moneda}' no es una moneda válida: se espera un código ISO de tres letras, como COP.")
        {
            Moneda = moneda;
        }
    }
}
