using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.ValueObjects;
using Modelos.Enumeracion;

namespace Test.CapaDatos
{
    [TestClass]
    public class ResponsablePagoServicioTest : TestBase
    {
        public ResponsablePagoServicioTest()
        {
            UseSqlite();
        }

        [TestMethod]
        public void Debe_ObtenerResponsablesPago_RetornaListaConResponsablePago()
        {
            var context = GetDbContext();

            var responsablesPagosEsperados = new List<ResponsablePago>
            {
                new ResponsablePago(1, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 1,2, 1m),
                new ResponsablePago(2, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 3, 90, 0.6667m),
                new ResponsablePago(3, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 91, 180, 0.5m),
                new ResponsablePago(4, Entidad.EPS, TipoIncapacidad.LicenciaMaternidad, 126, 126, 1m),
                new ResponsablePago(5, Entidad.EPS, TipoIncapacidad.LicenciaPaternidad, 8, 8, 1m),
                new ResponsablePago(6, Entidad.ARL, TipoIncapacidad.EnfermedadLaboral, 1, 180, 1m),
                new ResponsablePago(7, Entidad.ARL, TipoIncapacidad.AccidenteLaboral, 1, 180, 1m)

            };

            List<ResponsablePago> responsablesPago = context.ObtenerResponsablesPago();

            Assert.AreEqual(responsablesPagosEsperados.Count ,responsablesPago.Count);
        }
    }
}