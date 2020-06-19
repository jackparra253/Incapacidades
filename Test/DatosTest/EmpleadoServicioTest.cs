using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.ValueObjects;
using Modelos.Enumeracion;

namespace Test.DatosTest
{
    [TestClass]
    public class EmpleadoServicioTest : TestBase
    {
        public EmpleadoServicioTest()
        {
            UseSqlite();
        }

        [TestMethod]
        public void Debe_ObtenerEmpleados_RetornarListaEmpleados()
        {
            var context = GetDbContext();

            var alan = new Empleado(1, "Alan", "Turing", new Dinero(15_000_000m, Moneda.COP), new Dinero(500_000m, Moneda.COP), TipoSalario.Integral);
            var richard = new Empleado(2, "Richard", "Hendricks", new Dinero(3_000_000, Moneda.COP), new Dinero(100_000m, Moneda.COP), TipoSalario.Ley50);

            var empleadosEsperados = new List<Empleado>
            {
                alan,
                richard
            };

            List<Empleado> empleados = context.ObtenerEmpleados();

            Assert.IsTrue(empleadosEsperados[0].Salario == empleados[0].Salario);
            Assert.IsTrue(empleadosEsperados[1].Salario == empleados[1].Salario);
        }
    }
}