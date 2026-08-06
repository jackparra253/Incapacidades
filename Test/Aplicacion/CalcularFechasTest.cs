using Aplicacion;
using Shouldly;
using Xunit;

namespace Test.Aplicacion;

public class CalcularFechasTest
{
    [Fact]
    public void Debe_CalcularSiguienteFecha_RetornarUnaFecha_Cuando_AgregaCantidadDiasAUnaFechaBase()
    {
        var calcularFechas = new CalcularFechas();
        var fechaInicial = new DateTime(2020, 6, 30);
        int cantidadDias = 2;

        DateTime fecha = calcularFechas.CalcularSiguienteFecha(fechaInicial, cantidadDias);

        fecha.ShouldBe(new DateTime(2020, 7, 1));
    }
}
