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

        private ICalculadoraReconocimientoEconomico _calculadoraReconocimientoEconomico;

        private readonly IIncapacidadServicio _incapacidadServicio;


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

            var calculadoraFechasMock = new Mock<ICalcularFechas>();
            calculadoraFechasMock.Setup(p => p.CalcularSiguienteFecha(new DateTime(2020, 06, 03), 3)).Returns(new DateTime(2020, 06, 05));

            DbContextOptionsBuilder<IncapacidadesContext> builder = new DbContextOptionsBuilder<IncapacidadesContext>();

            IServicioDatos _servicioDatos = new IncapacidadesContext(builder.Options);

            ICalculadoraReconocimientoEconomico _calculadoraReconocimientoEconomico = new CalculadoraReconocimientoEconomico();

            IIncapacidadServicio _incapacidadServicio = new IncapacidadServicio(_contexto);

            _creadorIncapacidad = new CreadorIncapacidad(_servicioDatos, calculadoraFechasMock.Object, _calculadoraReconocimientoEconomico, _incapacidadServicio);
        }

        [TestMethod]
        public void Debe_Crear_PersistirIncipacidad_Cuando_EsEnfermedadGeneralPorDosDiasSalarioIntegral()
        {
            var reconocimientoEconomicoEsperado = new ReconocimientoEconomico(1, new DateTime(2020,06,03), new DateTime(2020,06,04), new Dinero(1_000_000m, Moneda.COP), Entidad.EMPRESA);
            
            var solicitudIncapacidad = new SolicitudIncapacidad(1, 1, 2020, 06, 03, 2, "incapacidad del señor Alan");

            _creadorIncapacidad.Crear(solicitudIncapacidad);

            Incapacidad incapacidad = _contexto.Incapacidades.FirstOrDefault();

            Assert.IsTrue(reconocimientoEconomicoEsperado.ValorAPagar == incapacidad.ReconocimientosEconomicos[0].ValorAPagar);
        }
    }

}