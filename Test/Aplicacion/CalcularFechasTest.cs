using Aplicacion;
using Shouldly;
using Xunit;

namespace Test.Aplicacion;

public class ConsultarFechasTest
{
    [Fact]
    public void Debe_CalcularSiguienteFecha_RetornarUnaFecha_Cuando_AgregaCantidadDiasAUnaFechaBase()
    {
        var calcularFechas = new CalcularFechas();

        var fechaEsperada = new DateTime(2020, 7, 1);
        int cantidadDias = 2;
        var fechaInicial = new DateTime(2020, 6, 30);

        var fecha = calcularFechas.CalcularSiguienteFecha(fechaInicial, cantidadDias);

        fecha.ShouldBe(fechaEsperada);
    }
}
