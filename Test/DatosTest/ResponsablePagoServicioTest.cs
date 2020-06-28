using System.Collections.Generic;
using Datos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modelos.Entidades;
using Modelos.Enumeracion;

namespace Test.DatosTest
{
    [TestClass]
    public class ResponsablePagoServicioTest : TestBase
    {
        private IncapacidadesContext _contexto;
        private List<ResponsablePago> _responsablesPagosEsperados;
        private ResponsablePagoServicio _responsablePagoServicio;

        public ResponsablePagoServicioTest()
        {
            UseSqlite();
        }

        [TestInitialize]
        public void Inicializar()
        {
            _contexto = GetDbContext();

            _responsablesPagosEsperados = new List<ResponsablePago>
            {
                new ResponsablePago(1, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 1,2, 1m),
                new ResponsablePago(2, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 3, 90, 0.6667m),
                new ResponsablePago(3, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 91, 180, 0.5m),
                new ResponsablePago(4, Entidad.EPS, TipoIncapacidad.LicenciaMaternidad, 1, 126, 1m),
                new ResponsablePago(5, Entidad.EPS, TipoIncapacidad.LicenciaPaternidad, 1, 8, 1m),
                new ResponsablePago(6, Entidad.ARL, TipoIncapacidad.EnfermedadLaboral, 1, 180, 1m),
                new ResponsablePago(7, Entidad.ARL, TipoIncapacidad.AccidenteLaboral, 1, 180, 1m)
            };

            _responsablePagoServicio = new ResponsablePagoServicio(_contexto);
        }

        [TestMethod]
        public void Debe_ObtenerResponsablesPago_RetornaListaConResponsablePago()
        {
            List<ResponsablePago> responsablesPago = _responsablePagoServicio.ObtenerResponsablesPago();

            Assert.AreEqual(_responsablesPagosEsperados.Count, responsablesPago.Count);
        }

        [TestMethod]
        public void Debe_ObtenerResponsablesPago_RetornarListaResponsablesPago_CuandoSeFiltraPorTipoIncapacidadTipoSalarioYCantidadDias()
        {
            var responsablesPagosEsperado = new List<ResponsablePago>
            {
                new ResponsablePago(1, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 1,2, 1m),
                new ResponsablePago(2, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 3, 90, 0.6667m),
                new ResponsablePago(3, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 91, 180, 0.5m)
            };

            var tipoIncapacidad =  TipoIncapacidad.EnfermedadGeneral;
            var cantidadDias = 4;

            List<ResponsablePago> responsablesPagos = _responsablePagoServicio.ObtenerResponsablesPago(tipoIncapacidad, cantidadDias);

            Assert.AreEqual(2, responsablesPagos.Count);
            Assert.AreEqual(responsablesPagosEsperado[0].Id, responsablesPagos[0].Id);
        }
    }
}