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
        public Dinero SalarioDiarioPorPorcentajeSalario { get; private set; }
        public Dinero SalarioDiarioPorPorcentajeCompensacion { get; private set; }
        public TipoSalario TipoSalario { get; private set; }
        public Empleado(int id, string nombres, string apellidos, Dinero salario, TipoSalario tipoSalario)
        {
            Id = id;
            Nombres = nombres;
            Apellidos = apellidos;
            Salario = salario;
            SalarioDiario = new Dinero(salario.Cantidad / 30, salario.Moneda);
            SalarioDiarioPorPorcentajeSalario = new Dinero(SalarioDiario.Cantidad * tipoSalario.PorcentajeSalario, salario.Moneda);
            SalarioDiarioPorPorcentajeCompensacion = new Dinero(SalarioDiario.Cantidad * tipoSalario.PorcentajeCompensacion, salario.Moneda);
            TipoSalario = tipoSalario;
        }
    }
}
