using System;
using Aplicacion;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Moq;

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
    }

}