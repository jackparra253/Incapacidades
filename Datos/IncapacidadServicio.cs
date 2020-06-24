using System.Linq;
using IDatos;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;

namespace Datos
{
    public partial class IncapacidadServicio : IIncapacidadServicio
    {

        private readonly IncapacidadesContext _contexto;

        public IncapacidadServicio(IncapacidadesContext contexto)
        {
            _contexto =contexto;
        }

        public void Guardar(Incapacidad incapacidad)
        {
            _contexto.Incapacidades.Add(incapacidad);
            _contexto.SaveChanges();
        }
    }
}