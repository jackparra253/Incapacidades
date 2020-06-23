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

        public EmpleadoServicioTest()
        {
            UseSqlite();
        }

        [TestInitialize]
        public void Inicializar()
        {
            var alan = new Empleado(1, "Alan", "Turing", new Dinero(15_000_000m, Moneda.COP), new Dinero(500_000m, Moneda.COP), TipoSalario.Integral);
            var richard = new Empleado(2, "Richard", "Hendricks", new Dinero(3_000_000, Moneda.COP), new Dinero(100_000m, Moneda.COP), TipoSalario.Ley50);

            _empleadosEsperados = new List<Empleado>
            {
                alan,
                richard
            };

             _contexto = GetDbContext();
        }

        [TestMethod]
        public void Debe_ObtenerEmpleados_RetornarListaEmpleados()
        {
            List<Empleado> empleados = _contexto.ObtenerEmpleados();

            Assert.IsTrue(_empleadosEsperados[0].Salario == empleados[0].Salario);
            Assert.IsTrue(_empleadosEsperados[1].Salario == empleados[1].Salario);
        }

        [TestMethod]
        public void Debe_ObtenerEmpleados_RetornarEmpleado_CuandoFiltraPorIdEmpleado()
        {
            int id = 2;
            Empleado empleado = _contexto.ObtenerEmpleado(id);

            Assert.IsTrue(_empleadosEsperados[1].Salario == empleado.Salario);
        }

    }
}