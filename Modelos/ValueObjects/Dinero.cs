using System;
using System.Collections.Generic;

namespace Modelos.ValueObjects
{
    public class Dinero : ValueObject
    {
        public decimal Cantidad { get; private set; }
        public string Moneda { get; set; }

        public Dinero(decimal cantidad, string moneda)
        {
            Cantidad = cantidad;
            Moneda = moneda;
        }

        private Dinero() { }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Math.Round(Cantidad, 2);
            yield return Moneda.ToUpper();
        }
    }
}
