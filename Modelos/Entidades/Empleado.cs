using Modelos.ValueObjects;
using Modelos.Enumeracion;

namespace Modelos.Entidades
{
    public class Empleado
    {
        public int Id { get; private set; }
        public string Nombres { get; private set; }
        public string Apellidos { get; private set; }
        public Dinero Salario { get; private set; }
        public Dinero SalarioDiario { get; private set; }
        public TipoSalario TipoSalario { get; private set; }
        public Empleado(int id, string nombres, string apellidos, Dinero salario, Dinero salarioDiario, TipoSalario tipoSalario)
        {
            Id = id;
            Nombres = nombres;
            Apellidos = apellidos;
            Salario = salario;
            SalarioDiario = salarioDiario;
            TipoSalario = tipoSalario;
        }
    }
}
