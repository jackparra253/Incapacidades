using Aplicacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDatos;
using Datos;
using Dominio;
using IDominio;
using Microsoft.EntityFrameworkCore;
using IAplicacion;
using Modelos.Entidades;
using System;
using Modelos.ValueObjects;
using Modelos;
using Modelos.Enumeracion;
using System.Linq;
using Modelos.Constantes;

namespace Test.Aplicacion
{
    [TestClass]
    public class CreadorIncapacidadEnfermedadGeneralSalarioIntegralTest : TestBase
    {
        private IncapacidadesContext _contexto;
        public CreadorIncapacidadEnfermedadGeneralSalarioIntegralTest()
        {
            UseSqlite();
        }

        [TestInitialize]
        public void Inicializar()
        {
            _contexto = GetDbContext();

            var builder = new DbContextOptionsBuilder<IncapacidadesContext>();

            IResponsablePagoServicio _responsablePagoServicio = new ResponsablePagoServicio(_contexto);

            ICalcularFechas _calculadoraFechas = new CalcularFechas();

            ICalculadoraReconocimientoEconomico _calculadoraReconocimientoEconomico = new CalculadoraReconocimientoEconomico();

            IIncapacidadServicio _incapacidadServicio = new IncapacidadServicio(_contexto);
        }
    }
}