using System;
using Aplicacion;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.ValueObjects;
using Modelos;
using IDatos;
using Datos;
using IDominio;
using Dominio;

namespace Test.Aplicacion
{
    [TestClass]
    public class CreadorIncapacidadLey50Test : TestBase
    {
        private IncapacidadesContext _contexto;
        private CreadorIncapacidadLey50 _creadorIncapacidad;
        public CreadorIncapacidadLey50Test()
        {
            UseSqlite();
        }

        [TestInitialize]
        public void Inicializar()
        {
            _contexto = GetDbContext();

            IResponsablePagoServicio responsablePagoServicio = new ResponsablePagoServicio(_contexto);
            IEmpleadoServicio empleadoServicio = new EmpleadoServicio(_contexto);
            IIncapacidadServicio incapacidadServicio = new IncapacidadServicio(_contexto);
            ICalculadoraReconocimientoEconomicoSalarioLey50 calculadoraReconocomientoEconomico = new CalculadoraReconocimientoEconomicoSalarioLey50();

            _creadorIncapacidad = new CreadorIncapacidadLey50(responsablePagoServicio, empleadoServicio, incapacidadServicio, calculadoraReconocomientoEconomico);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsEnfermedadGeneralPorDosDiasSalarioLey50_4Dias()
        {
            var solicitudIncapacidad = new SolicitudIncapacidad(2, 1, 2020, 06, 03, 4, "incapacidad del Richard");

            _creadorIncapacidad.Crear(solicitudIncapacidad);
            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.AreEqual(new DateTime(2020, 06, 03), incapacidad.FechaIncial);
            Assert.AreEqual(new DateTime(2020, 06, 06), incapacidad.FechaFinal);
            Assert.IsTrue(new Dinero(200_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 03), incapacidad.ReconocimientosEconomicos[0].FechaInicial);
            Assert.AreEqual(new DateTime(2020, 06, 04), incapacidad.ReconocimientosEconomicos[0].FechaFinal);
            Assert.IsTrue(new Dinero(133_340m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[1].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 05), incapacidad.ReconocimientosEconomicos[1].FechaInicial);
            Assert.AreEqual(new DateTime(2020, 06, 06), incapacidad.ReconocimientosEconomicos[1].FechaFinal);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsLicenciaMaternidadSalarioLey50()
        {
            var solicitudIncapacidad = new SolicitudIncapacidad(2, 2, 2020, 06, 03, 126, "incapacidad del Richard");

            _creadorIncapacidad.Crear(solicitudIncapacidad);
            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.IsTrue(new Dinero(12_600_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 03), incapacidad.FechaIncial);
            Assert.AreEqual(new DateTime(2020, 10, 06), incapacidad.FechaFinal);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsLicenciaPaternidadSalarioLey50()
        {
            var solicitudIncapacidad = new SolicitudIncapacidad(2, 3, 2020, 06, 03, 8, "incapacidad del Richard");

            _creadorIncapacidad.Crear(solicitudIncapacidad);
            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.IsTrue(new Dinero(800_000m, Moneda.COP) == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
            Assert.AreEqual(new DateTime(2020, 06, 03), incapacidad.FechaIncial);
            Assert.AreEqual(new DateTime(2020, 06, 10), incapacidad.FechaFinal);
        }
    }
}