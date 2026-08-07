using Modelos.Excepciones;

namespace Modelos.ValueObjects;

public class Dinero : ValueObject
{
    public decimal Cantidad { get; private set; }
    public string Moneda { get; private set; } = string.Empty;

    public Dinero(decimal cantidad, string moneda)
    {
        if (!EsMonedaValida(moneda))
            throw new MonedaInvalida(moneda);

        Cantidad = cantidad;
        Moneda = moneda.ToUpperInvariant();
    }

    private Dinero() { }

    public Dinero Por(decimal factor)
    {
        return new Dinero(Cantidad * factor, Moneda);
    }

    public Dinero Entre(decimal divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException("No se puede dividir dinero entre cero.");

        return new Dinero(Cantidad / divisor, Moneda);
    }

    public Dinero Mas(Dinero otro)
    {
        ExigirMismaMoneda(otro);

        return new Dinero(Cantidad + otro.Cantidad, Moneda);
    }

    public bool EsMenorQue(Dinero otro)
    {
        ExigirMismaMoneda(otro);

        return Cantidad < otro.Cantidad;
    }

    private void ExigirMismaMoneda(Dinero otro)
    {
        if (!string.Equals(Moneda, otro.Moneda, StringComparison.OrdinalIgnoreCase))
            throw new MonedasIncompatibles(Moneda, otro.Moneda);
    }

    private static bool EsMonedaValida(string moneda)
    {
        if (string.IsNullOrWhiteSpace(moneda) || moneda.Length != 3)
            return false;

        foreach (char caracter in moneda)
            if (!char.IsLetter(caracter))
                return false;

        return true;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Math.Round(Cantidad, 2);
        yield return Moneda.ToUpperInvariant();
    }
}
