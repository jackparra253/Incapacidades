using System.Collections.Generic;
using IDatos;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.Enumeracion;
using Modelos.Excepciones;
using Modelos.ValueObjects;
using System.Linq;

namespace Datos
{
    public class EmpleadoServicio : IEmpleadoServicio
    {
        private readonly IncapacidadesContext _contexto;

        public EmpleadoServicio(IncapacidadesContext contexto)
        {
            _contexto = contexto;
        }

        public List<Empleado> ObtenerEmpleados()
        {
            return new List<Empleado>()
            {
                new Empleado(1, "Alan", "Turing",new Dinero(15_000_000m, Moneda.COP), new TipoSalario(Tipo.Integral)),
                new Empleado(2, "Richard", "Hendricks", new Dinero(3_000_000, Moneda.COP), new TipoSalario(Tipo.Ley50))
            };
        }

        public Empleado ObtenerEmpleado(int id)
        {
            return ObtenerEmpleados().FirstOrDefault(empleado => empleado.Id == id)
                ?? throw new EmpleadoNoEncontrado(id);
        }
    }
}
