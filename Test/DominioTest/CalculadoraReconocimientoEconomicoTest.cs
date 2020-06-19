using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.Enumeracion;
using Modelos.ValueObjects;

namespace Test.DominioTest
{
    [TestClass]
    public class CalculadoraReconocimientoEconomicoTest
    {
        private readonly CalculadoraReconocimientoEconomico _calculadora;

        public CalculadoraReconocimientoEconomicoTest()
        {
            _calculadora = new CalculadoraReconocimientoEconomico();
        }

        [TestMethod]
        public void Debe_CalcularReconocimientoEconomico_RetornarDinero_CuandoTipoSalarioLey50()
        {
            Dinero reconocimientoEconomicoEsperado = new Dinero(66_670.0000m, Moneda.COP);

            var richard = new Empleado(2, "Richard", "Hendricks", new Dinero(3_000_000, Moneda.COP), new Dinero(100_000m, Moneda.COP), TipoSalario.Ley50);

            var eps = new ResponsablePago(3, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 3, 90, 0.6667m, 1m, TipoSalario.Ley50);

            Dinero reconocimientoEconomico = _calculadora.CalcularReconocimientoEconomico(richard, eps);

            Assert.IsTrue(reconocimientoEconomicoEsperado == reconocimientoEconomico);
        }

        [TestMethod]
        public void Debe_CalcularReconocimientoEconomico_RetornarDinero_CuandoTipoSalarioIntegral()
        {
            Dinero reconocimientoEconomicoEsperado = new Dinero(233_345.0000m, Moneda.COP);

            var alan = new Empleado(1, "Alan", "Turing", new Dinero(15_000_000m, Moneda.COP), new Dinero(500_000m,  Moneda.COP), TipoSalario.Integral);

            var eps = new ResponsablePago(4, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 3, 90, 0.6667m, 0.7m, TipoSalario.Integral);

            Dinero reconocimientoEconomico = _calculadora.CalcularReconocimientoEconomico(alan, eps);

            Assert.IsTrue(reconocimientoEconomicoEsperado == reconocimientoEconomico);
        }
    }
}