using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.ValueObjects;
using Modelos.Enumeracion;
using Datos;
namespace Test.DatosTest
{
    [TestClass]
    public class EmpleadoServicioTest : TestBase
    {
        private List<Empleado> _empleadosEsperados;
        private IncapacidadesContext _contexto;
        private EmpleadoServicio _empleadoServicio;

        public EmpleadoServicioTest()
        {
            UseSqlite();
        }

        [TestInitialize]
        public void Inicializar()
        {
            var alan = new Empleado(1, "Alan", "Turing", new Dinero(15_000_000m, Moneda.COP), new TipoSalario(Tipo.Integral));
            var richard = new Empleado(2, "Richard", "Hendricks", new Dinero(3_000_000, Moneda.COP), new TipoSalario(Tipo.Ley50));

            _empleadosEsperados = new List<Empleado>
            {
                alan,
                richard
            };

            _contexto = GetDbContext();

            _empleadoServicio = new EmpleadoServicio(_contexto);
        }

        [TestMethod]
        public void Debe_ObtenerEmpleados_RetornarListaEmpleados()
        {
            List<Empleado> empleados = _empleadoServicio.ObtenerEmpleados();

            Assert.IsTrue(new Dinero(500_000m, Moneda.COP) == empleados[0].SalarioDiario);
            Assert.IsTrue(new Dinero(350_000m, Moneda.COP) == empleados[0].SalarioDiarioPorPorcentajeSalario);
            Assert.IsTrue(new Dinero(150_000m, Moneda.COP) == empleados[0].SalarioDiarioPorPorcentajeCompensacion);

            Assert.IsTrue(new Dinero(100_000m, Moneda.COP) == empleados[1].SalarioDiario);
            Assert.IsTrue(new Dinero(100_000m, Moneda.COP) == empleados[1].SalarioDiarioPorPorcentajeSalario);
            Assert.IsTrue(new Dinero(0m, Moneda.COP) == empleados[1].SalarioDiarioPorPorcentajeCompensacion);
        }

        [TestMethod]
        public void Debe_ObtenerEmpleados_RetornarEmpleado_CuandoFiltraPorIdEmpleado()
        {
            Empleado empleado = _empleadoServicio.ObtenerEmpleado(2);

            Assert.IsTrue(new Dinero(100_000m, Moneda.COP) == empleado.SalarioDiario);
            Assert.IsTrue(new Dinero(100_000m, Moneda.COP) == empleado.SalarioDiarioPorPorcentajeSalario);
            Assert.IsTrue(new Dinero(0m, Moneda.COP) == empleado.SalarioDiarioPorPorcentajeCompensacion);
        }
    }
}