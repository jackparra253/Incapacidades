using System;
using Aplicacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Aplicacion
{
    [TestClass]
    public class ConsultarFechasTest
    {
        [TestMethod]
        public void Debe_CalcularSiguienteFecha_RetornarUnaFecha_Cuando_AgregaCantidadDiasAUnaFechaBase()
        {
            var calcularFechas = new CalcularFechas();

            var fechaEsperada = new DateTime(2020,7, 1);
            int cantidadDias = 2;
            var fechaInicial =  new DateTime(2020, 6, 30);
            
            var fecha = calcularFechas.CalcularSiguienteFecha(fechaInicial, cantidadDias);

            Assert.AreEqual(fechaEsperada, fecha);
        }
    }
}