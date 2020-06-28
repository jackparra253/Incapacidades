using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.Enumeracion;
using Modelos.ValueObjects;

namespace Test.ModelosTest
{
    [TestClass]
    public class ReconocimientoEconomicoTest
    {
        [TestMethod]
        public void Debe_ReconocimientoEconomico_CalcularValorAPagar()
        {
            var reconocimientoEconomico = new ReconocimientoEconomico(1, new DateTime(2020, 06, 27), 2, new Dinero(100_000, Moneda.COP), 0.6667m, Entidad.EPS);

            Assert.AreEqual(new Dinero(133_340m, Moneda.COP), reconocimientoEconomico.ValorAPagar);
        }

        [TestMethod]
        public void Debe_ReconocimientoEconomico_AsignarFechaFinal()
        {
            var reconocimientoEconomico = new ReconocimientoEconomico(1, new DateTime(2020, 06, 27), 2, new Dinero(100_000, Moneda.COP), 0.6667m, Entidad.EPS);

            Assert.AreEqual(new DateTime(2020, 06, 28), reconocimientoEconomico.FechaFinal);
        }
    }
}