using Microsoft.VisualStudio.TestTools.UnitTesting;
using Datos;
using IDatos;
using Modelos.Entidades;
using System;
using Modelos.ValueObjects;
using Modelos.Constantes;
using Modelos.Enumeracion;
using System.Collections.Generic;
using Modelos;

namespace Test.DatosTest
{
    [TestClass]
    public class IncapacidadServicioTest : TestBase
    {
        private IncapacidadesContext _contexto;

        private IncapacidadServicio _incapacidadServicio;
        public IncapacidadServicioTest()
        {
            UseSqlite();
        }

        [TestInitialize]
        public void Inicializar()
        {
            _contexto = GetDbContext();

            var reconocimientoEconomicos = new ReconocimientoEconomico(1, new DateTime(2020, 06, 03), new DateTime(2020, 06, 04), new Dinero(1_000_000m, Moneda.COP), Entidad.EMPRESA);

            var incapacidad = new Incapacidad(1, TipoIncapacidad.LicenciaMaternidad, new DateTime(2020, 06, 03), new DateTime(2020, 06, 04), 2, "Test", new List<ReconocimientoEconomico> { reconocimientoEconomicos });

            _contexto.Add(incapacidad);
            _contexto.SaveChanges();

            _incapacidadServicio = new IncapacidadServicio(_contexto);
        }


        [TestMethod]
        public void Debe_ObtenerIncapacidadesDetalle_RetornarListaDetalleIncapacidad_Cuando_RecibeIdEmpleado()
        {
            var incapacidadesDetalleEsperadas = new List<DetalleIncapacidad>
            {
                new DetalleIncapacidad(1, "Licencia Maternidad", new DateTime(2020,06,03).ToShortDateString(), new DateTime(2020,06,04).ToShortDateString(),2)
            };

            int idEmpleado = 1;

            List<DetalleIncapacidad> incapacidadesDetalle = _incapacidadServicio.ObtenerIncapacidadesDetalle(idEmpleado);

            Assert.AreEqual(incapacidadesDetalleEsperadas[0].Tipo, incapacidadesDetalle[0].Tipo);
        }

        [TestMethod]
        public void Debe_ObtenerReconocimientosEconomicosDetalle_RetornarlistaReconocimientosEconomicos_Cuando_RecibeIdEmpleado()
        {
            var reconocimientoEconomicosDetalleEsperados = new List<DetalleReconocimientoEconomico>
            {
                new DetalleReconocimientoEconomico(1, new DateTime(2020,06,03).ToShortDateString(), new DateTime(2020,06,04).ToShortDateString(), new Dinero(1_000_000m, Moneda.COP), "EMPRESA")
            };

            int idEmpleado = 1;
            List<DetalleReconocimientoEconomico> reconocimientosEconomicosDetalle = _incapacidadServicio.ObtenerReconocimientosEconomicosDetalle(idEmpleado);
            
            Assert.AreEqual(reconocimientoEconomicosDetalleEsperados[0].ResponsablePago, reconocimientosEconomicosDetalle[0].ResponsablePago);
            Assert.AreEqual(reconocimientoEconomicosDetalleEsperados[0].ValorAPagar, reconocimientosEconomicosDetalle[0].ValorAPagar);
        }
    }
}