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
using Microsoft.EntityFrameworkCore;

namespace Test.Aplicacion
{
    [TestClass]
    public class CreadorIncapacidadTest : TestBase
    {
        private CreadorIncapacidadEnfermedadGeneralSalarioLey50 _creadorIncapacidad;
        private IncapacidadesContext _contexto;
        public CreadorIncapacidadTest()
        {
            UseSqlite();
        }

        [TestInitialize]
        public void Inicializar()
        {
            _contexto = GetDbContext();

            var builder = new DbContextOptionsBuilder<IncapacidadesContext>();

            IResponsablePagoServicio _responsablePagoServicio = new ResponsablePagoServicio(_contexto);

            IEmpleadoServicio empleadoServicio = new EmpleadoServicio(_contexto);

            IIncapacidadServicio _incapacidadServicio = new IncapacidadServicio(_contexto);

            _creadorIncapacidad = new CreadorIncapacidadEnfermedadGeneralSalarioLey50(_responsablePagoServicio, empleadoServicio, _incapacidadServicio);
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
    }
}