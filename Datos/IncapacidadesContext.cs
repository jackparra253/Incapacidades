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
    }
}
