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
                new ResponsablePago(1, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 1,2, 1m, 1m, TipoSalario.Ley50),
                new ResponsablePago(2, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 1,2, 1m, 1m, TipoSalario.Integral),
                new ResponsablePago(3, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 3, 90, 0.6667m, 1m, TipoSalario.Ley50),
                new ResponsablePago(4, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 3, 90, 0.6667m, 0.7m, TipoSalario.Integral),
                new ResponsablePago(5, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 3, 90, 1m, 0.3m, TipoSalario.Integral),
                new ResponsablePago(6, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 91, 180, 0.5m, 1m, TipoSalario.Ley50),
                new ResponsablePago(7, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 91, 180, 0.5m, 0.7m, TipoSalario.Integral),
                new ResponsablePago(8, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 91, 180, 1m, 0.3m, TipoSalario.Integral),
                new ResponsablePago(9, Entidad.EPS, TipoIncapacidad.LicenciaMaternidad, 126, 126, 1m, 1m, TipoSalario.Ley50),
                new ResponsablePago(10, Entidad.EPS, TipoIncapacidad.LicenciaMaternidad, 126, 126, 1m, 0.7m, TipoSalario.Integral),
                new ResponsablePago(11, Entidad.EMPRESA, TipoIncapacidad.LicenciaMaternidad, 126, 126, 1m, 03m, TipoSalario.Integral),
                new ResponsablePago(12, Entidad.EPS, TipoIncapacidad.LicenciaPaternidad, 8, 8, 1m, 1m, TipoSalario.Ley50),
                new ResponsablePago(13, Entidad.EPS, TipoIncapacidad.LicenciaPaternidad, 8, 8, 1m, 0.7m, TipoSalario.Integral),
                new ResponsablePago(14, Entidad.EMPRESA, TipoIncapacidad.LicenciaPaternidad, 8, 8, 1m, 0.3m, TipoSalario.Integral),
                new ResponsablePago(15, Entidad.ARL, TipoIncapacidad.EnfermedadLaboral, 1, 180, 1m, 1m, TipoSalario.Ley50),
                new ResponsablePago(16, Entidad.ARL, TipoIncapacidad.EnfermedadLaboral, 1, 180, 1m, 0.7m, TipoSalario.Integral),
                new ResponsablePago(17, Entidad.EMPRESA, TipoIncapacidad.EnfermedadLaboral, 1, 180, 1m, 0.3m, TipoSalario.Integral),
                new ResponsablePago(18, Entidad.ARL, TipoIncapacidad.AccidenteLaboral, 1, 180, 1m, 1m, TipoSalario.Ley50),
                new ResponsablePago(19, Entidad.ARL, TipoIncapacidad.AccidenteLaboral, 1, 180, 1m, 0.7m, TipoSalario.Integral),
                new ResponsablePago(20, Entidad.EMPRESA, TipoIncapacidad.AccidenteLaboral, 1, 180, 1m, 0.3m, TipoSalario.Integral)
            };
        }

        [TestMethod]
        public void Debe_ObtenerResponsablesPago_RetornaListaConResponsablePago()
        {
            List<ResponsablePago> responsablesPago = _contexto.ObtenerResponsablesPago();

            Assert.AreEqual(_responsablesPagosEsperados.Count, responsablesPago.Count);
        }

        [TestMethod]
        public void Debe_ObtenerResponsablePago_RetornarUnResponsable_CuandoIngresaIdResponsablePago()
        {
            var responsablePagoEsperado = new ResponsablePago(18, Entidad.ARL, TipoIncapacidad.AccidenteLaboral, 1, 180, 1m, 1m, TipoSalario.Ley50);

            ResponsablePago responsablePago = _contexto.ObtenerResponsablePago(responsablePagoEsperado.Id);

            Assert.AreEqual(responsablePagoEsperado.Id, responsablePago.Id);
            Assert.AreEqual(responsablePagoEsperado.ReconocimientoPorcentaje, responsablePago.ReconocimientoPorcentaje);
            Assert.AreEqual(responsablePagoEsperado.Responsable, responsablePago.Responsable);
            Assert.AreEqual(responsablePagoEsperado.TipoIncapacidad, responsablePago.TipoIncapacidad);
            Assert.AreEqual(responsablePagoEsperado.DiasIncapacidadFinal, responsablePago.DiasIncapacidadFinal);
            Assert.AreEqual(responsablePagoEsperado.DiasIncapacidadInicial, responsablePago.DiasIncapacidadInicial);
        }

        [TestMethod]
        public void Debe_ObtenerResponsablesPago_RetornarListaResponsablesPago_CuandoSeFiltraPorTipoIncapacidadTipoSalarioYCantidadDias()
        {
            var responsablesPagosEsperado = new List<ResponsablePago>
            {
                new ResponsablePago(2, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 1,2, 1m, 1m, TipoSalario.Integral),
                new ResponsablePago(4, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 3, 90, 0.6667m, 0.7m, TipoSalario.Integral),
                new ResponsablePago(5, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 3, 90, 1m, 0.3m, TipoSalario.Integral)
            };

            var tipoIncapacidad =  TipoIncapacidad.EnfermedadGeneral;
            var tipoSalario = TipoSalario.Integral;
            var cantidadDias = 4;

            List<ResponsablePago> responsablePagos = _contexto.ObtenerResponsablesPago(tipoIncapacidad, tipoSalario, cantidadDias);

            Assert.AreEqual(responsablesPagosEsperado.Count, responsablePagos.Count);
        }
    }
}