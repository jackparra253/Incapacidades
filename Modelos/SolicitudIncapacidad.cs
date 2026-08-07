using Modelos.Enumeracion;
using Modelos.Excepciones;

namespace Modelos;

public class SolicitudIncapacidad
{
    public int IdEmpleado { get; private set; }
    public int TipoIncapacidad { get; private set; }
    public int Anio { get; private set; }
    public int Mes { get; private set; }
    public int Dia { get; private set; }
    public int CantidadDias { get; private set; }
    public string Observaciones { get; private set; } = string.Empty;

    public DateTime FechaInicial { get; private set; }
    public TipoIncapacidad TipoDeIncapacidad { get; private set; }

    public SolicitudIncapacidad(int idEmpleado, int tipoIncapacidad, int anio, int mes, int dia, int cantidadDias, string observaciones)
    {
        FechaInicial = ConstruirFecha(anio, mes, dia);
        TipoDeIncapacidad = ConvertirTipoIncapacidad(tipoIncapacidad);
        CantidadDias = ValidarCantidadDias(cantidadDias);

        IdEmpleado = idEmpleado;
        TipoIncapacidad = tipoIncapacidad;
        Anio = anio;
        Mes = mes;
        Dia = dia;
        Observaciones = observaciones;
    }

    private SolicitudIncapacidad() { }

    private static DateTime ConstruirFecha(int anio, int mes, int dia)
    {
        try
        {
            return new DateTime(anio, mes, dia);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new FechaInvalida(anio, mes, dia);
        }
    }

    private static TipoIncapacidad ConvertirTipoIncapacidad(int tipoIncapacidad)
    {
        if (tipoIncapacidad is < byte.MinValue or > byte.MaxValue)
            throw new TipoIncapacidadInvalido(tipoIncapacidad);

        if (!Enum.IsDefined(typeof(TipoIncapacidad), (byte)tipoIncapacidad))
            throw new TipoIncapacidadInvalido(tipoIncapacidad);

        return (TipoIncapacidad)tipoIncapacidad;
    }

    private static int ValidarCantidadDias(int cantidadDias)
    {
        if (cantidadDias < 1)
            throw new CantidadDiasInvalida(cantidadDias);

        return cantidadDias;
    }
}
