using System;
using Aplicacion;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.Enumeracion;
using Modelos.ValueObjects;
using Modelos;
using IDatos;
using Datos;
using Dominio;
using IDominio;
using Microsoft.EntityFrameworkCore;
using IAplicacion;

namespace Test.Aplicacion
{
    [TestClass]
    public class CreadorIncapacidadTest : TestBase
    {
        private IServicioDatos _servicioDatos;
        private ICalcularFechas _calculadoraFechas;
        private ICalculadoraReconocimientoEconomico _calculadoraReconocimientoEconomico;
        private IIncapacidadServicio _incapacidadServicio;
        private CreadorIncapacidad _creadorIncapacidad;
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

            IServicioDatos _servicioDatos = new IncapacidadesContext(builder.Options);

            ICalcularFechas _calculadoraFechas = new CalcularFechas();

            ICalculadoraReconocimientoEconomico _calculadoraReconocimientoEconomico = new CalculadoraReconocimientoEconomico();

            IIncapacidadServicio _incapacidadServicio = new IncapacidadServicio(_contexto);

            _creadorIncapacidad = new CreadorIncapacidad(_servicioDatos, _calculadoraFechas, _calculadoraReconocimientoEconomico, _incapacidadServicio);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsEnfermedadGeneralPorDosDiasSalarioIntegral()
        {
            var reconocimientoEconomicoEsperado = new ReconocimientoEconomico(1, new DateTime(2020, 06, 03), new DateTime(2020, 06, 04), new Dinero(1_000_000m, Moneda.COP), Entidad.EMPRESA);

            var solicitudIncapacidad = new SolicitudIncapacidad(1, 1, 2020, 06, 03, 2, "incapacidad del señor Alan");

            _creadorIncapacidad.Crear(solicitudIncapacidad);

            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.IsTrue(reconocimientoEconomicoEsperado.ValorAPagar == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
        }


        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsEnfermedadGeneralPorDosDiasSalarioIntegral_3Dias()
        {
            var reconocimientoEconomicoEmpresa = new ReconocimientoEconomico(1, new DateTime(2020, 06, 03), new DateTime(2020, 06, 04), new Dinero(1_000_000m, Moneda.COP), Entidad.EMPRESA);
            var reconocimientoEconomicoEmpresa2 = new ReconocimientoEconomico(1, new DateTime(2020, 06, 05), new DateTime(2020, 06, 05), new Dinero(150_000m, Moneda.COP), Entidad.EMPRESA);
            var reconocimientoEconomicoEps = new ReconocimientoEconomico(1, new DateTime(2020, 06, 05), new DateTime(2020, 06, 05), new Dinero(233_345m, Moneda.COP), Entidad.EPS);

            var solicitudIncapacidad = new SolicitudIncapacidad(1, 1, 2020, 06, 03, 3, "incapacidad del señor Alan");

            _creadorIncapacidad.Crear(solicitudIncapacidad);

            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.AreEqual(new DateTime(2020, 06, 03),incapacidad.FechaIncial);
            Assert.AreEqual(new DateTime(2020, 06, 05),incapacidad.FechaFinal);

            Assert.IsTrue(reconocimientoEconomicoEmpresa.ValorAPagar == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);             
            Assert.AreEqual(reconocimientoEconomicoEmpresa.FechaInicial, incapacidad.ReconocimientosEconomicos[0].FechaInicial);             
            Assert.AreEqual(reconocimientoEconomicoEmpresa.FechaFinal, incapacidad.ReconocimientosEconomicos[0].FechaFinal);             


            Assert.IsTrue(reconocimientoEconomicoEps.ValorAPagar == incapacidad.ReconocimientosEconomicos[1].ValorAPagar);
            Assert.AreEqual(reconocimientoEconomicoEps.FechaInicial, incapacidad.ReconocimientosEconomicos[1].FechaInicial);
            Assert.AreEqual(reconocimientoEconomicoEps.FechaFinal, incapacidad.ReconocimientosEconomicos[1].FechaFinal);

            Assert.IsTrue(reconocimientoEconomicoEmpresa2.ValorAPagar == incapacidad.ReconocimientosEconomicos[2].ValorAPagar);
            Assert.AreEqual(reconocimientoEconomicoEmpresa2.FechaInicial , incapacidad.ReconocimientosEconomicos[2].FechaInicial);
            Assert.AreEqual(reconocimientoEconomicoEmpresa2.FechaFinal , incapacidad.ReconocimientosEconomicos[2].FechaFinal);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsEnfermedadGeneralPorDosDiasSalarioLey50()
        {
            var reconocimientoEconomicoEsperado = new ReconocimientoEconomico(2, new DateTime(2020, 06, 03), new DateTime(2020, 06, 04), new Dinero(200_000m, Moneda.COP), Entidad.EMPRESA);

            var solicitudIncapacidad = new SolicitudIncapacidad(2, 1, 2020, 06, 03, 2, "incapacidad del Richard");

            _creadorIncapacidad.Crear(solicitudIncapacidad);

            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.IsTrue(reconocimientoEconomicoEsperado.ValorAPagar == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsLicenciaMaternidadSalarioIntegral()
        {
            var reconocimientoEconomicoEmpresa = new ReconocimientoEconomico(1, new DateTime(2020, 06, 03), new DateTime(2020, 10, 06), new Dinero(18_900_000m, Moneda.COP), Entidad.EMPRESA);
            var reconocimientoEconomicoEps = new ReconocimientoEconomico(1, new DateTime(2020, 06, 03), new DateTime(2020, 10, 06), new Dinero(44_100_000m, Moneda.COP), Entidad.EPS);

            var solicitudIncapacidad = new SolicitudIncapacidad(1, 2, 2020, 06, 03, 126, "incapacidad del señor Alan");

            _creadorIncapacidad.Crear(solicitudIncapacidad);

            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.IsTrue(reconocimientoEconomicoEmpresa.ValorAPagar == incapacidad.ReconocimientosEconomicos[1].ValorAPagar);
            Assert.IsTrue(reconocimientoEconomicoEps.ValorAPagar == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsLicenciaMaternidadSalarioLey50()
        {
            var reconocimientoEconomicoEsperado = new ReconocimientoEconomico(2, new DateTime(2020, 06, 03), new DateTime(2020, 10, 06), new Dinero(12_600_000m, Moneda.COP), Entidad.EPS);

            var solicitudIncapacidad = new SolicitudIncapacidad(2, 2, 2020, 06, 03, 126, "incapacidad del Richard");

            _creadorIncapacidad.Crear(solicitudIncapacidad);

            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.IsTrue(reconocimientoEconomicoEsperado.ValorAPagar == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsLicenciaPaternidadSalarioIntegral()
        {
            var reconocimientoEconomicoEmpresa = new ReconocimientoEconomico(1, new DateTime(2020, 06, 03), new DateTime(2020, 06, 11), new Dinero(1_200_000m, Moneda.COP), Entidad.EMPRESA);
            var reconocimientoEconomicoEps = new ReconocimientoEconomico(1, new DateTime(2020, 06, 03), new DateTime(2020, 06, 11), new Dinero(2_800_000m, Moneda.COP), Entidad.EPS);

            var solicitudIncapacidad = new SolicitudIncapacidad(1, 3, 2020, 06, 03, 8, "incapacidad del señor Alan");

            _creadorIncapacidad.Crear(solicitudIncapacidad);

            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.IsTrue(reconocimientoEconomicoEmpresa.ValorAPagar == incapacidad.ReconocimientosEconomicos[1].ValorAPagar);
            Assert.IsTrue(reconocimientoEconomicoEps.ValorAPagar == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncapacidad_Cuando_EsLicenciaPaternidadSalarioLey50()
        {
            var reconocimientoEconomicoEsperado = new ReconocimientoEconomico(2, new DateTime(2020, 06, 03), new DateTime(2020, 06, 11), new Dinero(800_000m, Moneda.COP), Entidad.EPS);

            var solicitudIncapacidad = new SolicitudIncapacidad(2, 3, 2020, 06, 03, 8, "incapacidad del Richard");

            _creadorIncapacidad.Crear(solicitudIncapacidad);

            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.IsTrue(reconocimientoEconomicoEsperado.ValorAPagar == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
        }
    }
}