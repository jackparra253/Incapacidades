using Aplicacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDatos;
using Datos;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System;
using Modelos.ValueObjects;
using Modelos;
using System.Linq;
using Modelos.Constantes;

namespace Test.Aplicacion
{
    [TestClass]
    public class CreadorIncapacidadEnfermedadGeneralSalarioIntegralTest : TestBase
    {
        private IncapacidadesContext _contexto;

        private CreadorIncapacidadEnfermedadGeneralSalarioIntegral _creadorIncapacidad;
        public CreadorIncapacidadEnfermedadGeneralSalarioIntegralTest()
        {
            UseSqlite();
        }

        [TestInitialize]
        public void Inicializar()
        {
            _contexto = GetDbContext();

            var builder = new DbContextOptionsBuilder<IncapacidadesContext>();

            IResponsablePagoServicio responsablePagoServicio = new ResponsablePagoServicio(_contexto);

            IEmpleadoServicio empleadoServicio = new EmpleadoServicio(_contexto);

            IIncapacidadServicio incapacidadServicio = new IncapacidadServicio(_contexto);

            _creadorIncapacidad = new CreadorIncapacidadEnfermedadGeneralSalarioIntegral(responsablePagoServicio,empleadoServicio, incapacidadServicio);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsEnfermedadGeneralPorDosDiasSalarioIntegral_5Dias()
        {
            var solicitudIncapacidad = new SolicitudIncapacidad(1, 1, 2020, 06, 03, 5, "incapacidad del señor Alan");

            _creadorIncapacidad.Crear(solicitudIncapacidad);
            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.AreEqual(new DateTime(2020, 06, 03), incapacidad.FechaIncial);
            Assert.AreEqual(new DateTime(2020, 06, 07), incapacidad.FechaFinal);

            Assert.IsTrue(new Dinero(1_000_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 03), incapacidad.ReconocimientosEconomicos[0].FechaInicial);
            Assert.AreEqual(new DateTime(2020, 06, 04), incapacidad.ReconocimientosEconomicos[0].FechaFinal);

            Assert.IsTrue(new Dinero(700_035m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[1].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 05), incapacidad.ReconocimientosEconomicos[1].FechaInicial);
            Assert.AreEqual(new DateTime(2020, 06, 07), incapacidad.ReconocimientosEconomicos[1].FechaFinal);

            Assert.IsTrue(new Dinero(450_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[2].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 05), incapacidad.ReconocimientosEconomicos[2].FechaInicial);
            Assert.AreEqual(new DateTime(2020, 06, 07), incapacidad.ReconocimientosEconomicos[2].FechaFinal);
        }
    }
}