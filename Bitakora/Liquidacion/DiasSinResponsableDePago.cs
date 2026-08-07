using Bitakora.Incapacidades;
using System;

namespace Bitakora.Liquidacion;

public class DiasSinResponsableDePago : Exception
{
    public TipoIncapacidad TipoIncapacidad { get; }
    public int CantidadDias { get; }
    public int UltimoDiaCubierto { get; }

    public DiasSinResponsableDePago(TipoIncapacidad tipoIncapacidad, int cantidadDias, int ultimoDiaCubierto)
        : base($"Una incapacidad por {tipoIncapacidad} tiene responsable de pago hasta el día " +
               $"{ultimoDiaCubierto}, así que los {cantidadDias - ultimoDiaCubierto} días que van " +
               $"hasta el {cantidadDias} quedarían sin liquidar.")
    {
        TipoIncapacidad = tipoIncapacidad;
        CantidadDias = cantidadDias;
        UltimoDiaCubierto = ultimoDiaCubierto;
    }
}
