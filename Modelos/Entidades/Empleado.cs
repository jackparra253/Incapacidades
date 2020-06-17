using Modelos.ValueObjects;

namespace Modelos.Entidades
{
    public class Empleado
    {
        public int Id { get; private set; }
        public string Nombres { get; private set; }
        public string Apellidos { get; private set; }
        public Dinero Salario { get; private set; }
        public Dinero SalarioDiario { get; private set; }

        public Empleado(string nombres, string apellidos, Dinero salario, Dinero salarioDiario)
        {
            Nombres = nombres;
            Apellidos = apellidos;
            Salario = salario;
            SalarioDiario = salarioDiario;
        }
    }
}
