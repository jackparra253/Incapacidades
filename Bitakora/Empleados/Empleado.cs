using Bitakora.Salarios;

namespace Bitakora.Empleados;

public class Empleado
{
    public int Id { get; private set; }
    public string Nombres { get; private set; } = string.Empty;
    public string Apellidos { get; private set; } = string.Empty;
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
        SalarioDiario = salario.Entre(30);
        SalarioDiarioPorPorcentajeSalario = SalarioDiario.Por(tipoSalario.PorcentajeSalario);
        SalarioDiarioPorPorcentajeCompensacion = SalarioDiario.Por(tipoSalario.PorcentajeCompensacion);
        TipoSalario = tipoSalario;
    }
}
