using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modelos.Constantes;
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
            Dinero reconocimientoEconomicoEsperado = new Dinero(66670m, Moneda.COP);

            TipoSalario tipoSalario = TipoSalario.Ley50;
            var salarioDiaro = new Dinero(100_000, Moneda.COP);
            decimal porcentajeReconocimiento = 0.6667m;

            Dinero reconocimientoEconomico = _calculadora.CalcularReconocimientoEconomico(tipoSalario, salarioDiaro, porcentajeReconocimiento);

            Assert.AreEqual(reconocimientoEconomicoEsperado, reconocimientoEconomico);
        }
    }

}