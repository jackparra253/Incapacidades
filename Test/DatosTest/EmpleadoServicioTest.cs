using Datos;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.Excepciones;
using Modelos.ValueObjects;
using Shouldly;
using Xunit;

namespace Test.DatosTest;

public class EmpleadoServicioTest : TestBase
{
    private readonly EmpleadoServicio _empleadoServicio;

    public EmpleadoServicioTest()
    {
        _empleadoServicio = new EmpleadoServicio(Contexto);
    }

    [Fact]
    public void Debe_ObtenerEmpleados_DerivarLosSalariosDiariosSegunElTipoDeSalario()
    {
        List<Empleado> empleados = _empleadoServicio.ObtenerEmpleados();

        (new Dinero(500_000m, Moneda.COP) == empleados[0].SalarioDiario).ShouldBeTrue();
        (new Dinero(350_000m, Moneda.COP) == empleados[0].SalarioDiarioPorPorcentajeSalario).ShouldBeTrue();
        (new Dinero(150_000m, Moneda.COP) == empleados[0].SalarioDiarioPorPorcentajeCompensacion).ShouldBeTrue();
        (new Dinero(100_000m, Moneda.COP) == empleados[1].SalarioDiario).ShouldBeTrue();
        (new Dinero(100_000m, Moneda.COP) == empleados[1].SalarioDiarioPorPorcentajeSalario).ShouldBeTrue();
        (new Dinero(0m, Moneda.COP) == empleados[1].SalarioDiarioPorPorcentajeCompensacion).ShouldBeTrue();
    }

    [Fact]
    public void Debe_ObtenerEmpleado_RetornarElEmpleado_Cuando_ElIdExiste()
    {
        const int richard = 2;

        Empleado empleado = _empleadoServicio.ObtenerEmpleado(richard);

        (new Dinero(100_000m, Moneda.COP) == empleado.SalarioDiario).ShouldBeTrue();
        (new Dinero(100_000m, Moneda.COP) == empleado.SalarioDiarioPorPorcentajeSalario).ShouldBeTrue();
        (new Dinero(0m, Moneda.COP) == empleado.SalarioDiarioPorPorcentajeCompensacion).ShouldBeTrue();
    }

    [Fact]
    public void Debe_ObtenerEmpleado_Fallar_Cuando_ElIdNoExiste()
    {
        const int inexistente = 99;

        Action consulta = () => _empleadoServicio.ObtenerEmpleado(inexistente);

        EmpleadoNoEncontrado error = Should.Throw<EmpleadoNoEncontrado>(consulta);

        error.IdEmpleado.ShouldBe(inexistente);
    }
}
