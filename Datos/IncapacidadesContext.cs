using System.Collections.Generic;
using IDatos;
using Microsoft.EntityFrameworkCore;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.ValueObjects;
using Modelos.Enumeracion;
using System;

namespace Datos
{
    public partial class IncapacidadesContext : DbContext, IServicioDatos
    {
        public IncapacidadesContext(DbContextOptions<IncapacidadesContext> options) : base(options)
        {

        }

        public List<Empleado> ObtenerEmpleados()
        {
            return new List<Empleado>()
            {
                new Empleado(1, "Alan", "Turing", new Dinero(15_000_000m, Moneda.COP), new Dinero(500_000m, Moneda.COP), TipoSalario.Integral),
                new Empleado(2, "Richard", "Hendricks", new Dinero(3_000_000, Moneda.COP), new Dinero(100_000m, Moneda.COP), TipoSalario.Ley50)
            };
        }
    }
}
