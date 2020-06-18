using System.Collections.Generic;
using Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.ValueObjects;

namespace Test.CapaDatos
{
    [TestClass]
    public class EmpleadoServicioTest: TestBase
    {
        public EmpleadoServicioTest()
        {
            UseSqlite();
        }

        [TestMethod]
        public void Debe_ObtenerEmpleados_RetornarListaEmpleados()
        {
            var context =  GetDbContext();

            var alan = new Empleado("Alan", "Turing", new Dinero(15_000_000m, Moneda.COP), new Dinero(500_000m, "USD"));
            var richard = new Empleado("Richard", "Hendricks", new Dinero(3_000_000, Moneda.COP), new Dinero(100_000m, "USD"));

            var empleadosEsperados = new List<Empleado>
            {
                alan,
                richard
            };

            List<Empleado> empleados = context.ObtenerEmpleados();

            Assert.AreEqual(empleadosEsperados[0].Salario.Cantidad, empleados[0].Salario.Cantidad);
        }
    }
}