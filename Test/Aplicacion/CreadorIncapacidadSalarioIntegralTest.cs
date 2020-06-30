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
using Modelos.Enumeracion;

namespace Test.Aplicacion
{
    [TestClass]
    public class CreadorIncapacidadSalarioIntegralTest : TestBase
    {
        private IncapacidadesContext _contexto;

        private CreadorIncapacidadSalarioIntegral _creadorIncapacidad;
        public CreadorIncapacidadSalarioIntegralTest()
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

            _creadorIncapacidad = new CreadorIncapacidadSalarioIntegral(responsablePagoServicio, empleadoServicio, incapacidadServicio);
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
            Assert.AreEqual(Entidad.EMPRESA, incapacidad.ReconocimientosEconomicos[0].ResponsablePago);

            Assert.IsTrue(new Dinero(700_035m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[1].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 05), incapacidad.ReconocimientosEconomicos[1].FechaInicial);
            Assert.AreEqual(new DateTime(2020, 06, 07), incapacidad.ReconocimientosEconomicos[1].FechaFinal);
            Assert.AreEqual(Entidad.EPS, incapacidad.ReconocimientosEconomicos[1].ResponsablePago);

            Assert.IsTrue(new Dinero(450_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[2].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 05), incapacidad.ReconocimientosEconomicos[2].FechaInicial);
            Assert.AreEqual(new DateTime(2020, 06, 07), incapacidad.ReconocimientosEconomicos[2].FechaFinal);
            Assert.AreEqual(Entidad.EMPRESA, incapacidad.ReconocimientosEconomicos[2].ResponsablePago);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsLicenciaMaternidadSalarioIntegral()
        {
            var solicitudIncapacidad = new SolicitudIncapacidad(1, 2, 2020, 06, 03, 126, "incapacidad del señor Alan");

            _creadorIncapacidad.Crear(solicitudIncapacidad);

            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.AreEqual(new DateTime(2020, 06, 03), incapacidad.FechaIncial);
            Assert.AreEqual(new DateTime(2020, 10, 06), incapacidad.FechaFinal);

            Assert.IsTrue(new Dinero(44_100_000m , Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 03), incapacidad.ReconocimientosEconomicos[0].FechaInicial);
            Assert.AreEqual(new DateTime(2020, 10, 06), incapacidad.ReconocimientosEconomicos[0].FechaFinal);
            Assert.AreEqual(Entidad.EPS, incapacidad.ReconocimientosEconomicos[0].ResponsablePago);

            Assert.IsTrue(new Dinero(18_900_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[1].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 03), incapacidad.ReconocimientosEconomicos[1].FechaInicial);
            Assert.AreEqual(new DateTime(2020, 10, 06), incapacidad.ReconocimientosEconomicos[1].FechaFinal);
            Assert.AreEqual(Entidad.EMPRESA, incapacidad.ReconocimientosEconomicos[1].ResponsablePago);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsLicenciaPaternidadSalarioIntegral()
        {
            var solicitudIncapacidad = new SolicitudIncapacidad(1, 3, 2020, 06, 03, 8, "incapacidad del señor Alan");

            _creadorIncapacidad.Crear(solicitudIncapacidad);

            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.AreEqual(new DateTime(2020, 06, 03), incapacidad.FechaIncial);
            Assert.AreEqual(new DateTime(2020, 06, 10), incapacidad.FechaFinal);

            Assert.IsTrue(new Dinero(2_800_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 03), incapacidad.ReconocimientosEconomicos[0].FechaInicial);
            Assert.AreEqual(new DateTime(2020, 06, 10), incapacidad.ReconocimientosEconomicos[0].FechaFinal);
            Assert.AreEqual(Entidad.EPS, incapacidad.ReconocimientosEconomicos[0].ResponsablePago);

            Assert.IsTrue(new Dinero(1_200_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[1].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 03), incapacidad.ReconocimientosEconomicos[1].FechaInicial);
            Assert.AreEqual(new DateTime(2020, 06, 10), incapacidad.ReconocimientosEconomicos[1].FechaFinal);
            Assert.AreEqual(Entidad.EMPRESA, incapacidad.ReconocimientosEconomicos[1].ResponsablePago);
        }
    }
}