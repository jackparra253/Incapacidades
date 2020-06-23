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
using Microsoft.EntityFrameworkCore;
using IAplicacion;

namespace Test.Aplicacion
{
    [TestClass]
    public class CreadorIncapacidadTest : TestBase
    {
        private IServicioDatos _servicioDatos;
        private CreadorIncapacidad _creadorIncapacidad;

        public CreadorIncapacidadTest()
        {
            UseSqlite();
        }

        [TestInitialize]
        public void Inicializar()
        {
            var calculadoraFechasMock = new Mock<ICalcularFechas>();
            calculadoraFechasMock.Setup(p => p.CalcularSiguienteFecha(new DateTime(2020, 06, 03), 3)).Returns(new DateTime(2020, 06, 05));

            DbContextOptionsBuilder<IncapacidadesContext> builder = new DbContextOptionsBuilder<IncapacidadesContext>();

            IServicioDatos _servicioDatos = new IncapacidadesContext(builder.Options);

            _creadorIncapacidad = new CreadorIncapacidad(_servicioDatos, calculadoraFechasMock.Object);
        }

        [TestMethod]
        public void Debe_Crear_Persistir_IncpacidadYReconocimientosEconomicos()
        {
            var context = GetDbContext();

            // var reconocimientosEconomicos = new List<ReconocimientoEconomico>
            // {
            //     new ReconocimientoEconomico(1,1,new DateTime(2020, 06, 03),new Dinero(333_350m, Moneda.COP), Entidad.EMPRESA),
            //     new ReconocimientoEconomico(1,1,new DateTime(2020, 06, 04),new Dinero(333_350m, Moneda.COP), Entidad.EMPRESA),
            //     new ReconocimientoEconomico(1,1,new DateTime(2020, 06, 05),new Dinero(233_345m, Moneda.COP), Entidad.EPS),
            //     new ReconocimientoEconomico(1,1,new DateTime(2020, 06, 05),new Dinero(150_000m, Moneda.COP), Entidad.EMPRESA)
            // };

            // var incapacidadEsperada = new Incapacidad(1, TipoIncapacidad.EnfermedadGeneral, new DateTime(2020, 06, 03), new DateTime(2020, 06, 05), 3, "incapacidad del señor Alan", reconocimientosEconomicos);


            // var alan = new Empleado(1, "Alan", "Turing", new Dinero(15_000_000m, Moneda.COP), new Dinero(500_000m, Moneda.COP), TipoSalario.Integral);

            var solicitudIncapacidad = new SolicitudIncapacidad(1, 1, 2020, 06, 03, 3, "incapacidad del señor Alan");

            _creadorIncapacidad.Crear(solicitudIncapacidad);

            // Incapacidad incapacidad = context.Incapacidades.FirstOrDefault();

            // Assert.AreEqual(incapacidadEsperada.ReconocimientosEconomicos.Count, incapacidad.ReconocimientosEconomicos.Count);
            // Assert.AreEqual(incapacidadEsperada.IdEmpleado, incapacidad.IdEmpleado);
            // AsertarReconocimientosEconomicos(incapacidadEsperada, incapacidad);
        }

        public void AsertarReconocimientosEconomicos(Incapacidad incapacidadEsperada, Incapacidad incapacidad)
        {
            for (int i = 0; i < incapacidadEsperada.ReconocimientosEconomicos.Count; i++)
            {
                Assert.AreEqual(incapacidadEsperada.ReconocimientosEconomicos[i].ValorAPagar, incapacidad.ReconocimientosEconomicos[i].ValorAPagar);
                Assert.AreEqual(incapacidadEsperada.ReconocimientosEconomicos[i].ResponsablePago, incapacidad.ReconocimientosEconomicos[i].ResponsablePago);
                Assert.AreEqual(incapacidadEsperada.ReconocimientosEconomicos[i].Fecha, incapacidad.ReconocimientosEconomicos[i].Fecha);
            }
        }



    }

}