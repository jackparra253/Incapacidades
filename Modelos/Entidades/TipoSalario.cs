using Modelos.Enumeracion;

namespace Modelos.Entidades;

/// <summary>
/// Define qué fracción del salario mensual constituye el Ingreso Base de Cotización (IBC), que es
/// sobre lo que se liquidan las incapacidades. El resto es factor prestacional, que se congela
/// durante la incapacidad y no genera reconocimiento.
/// </summary>
public abstract class TipoSalario
{
    public abstract Tipo Tipo { get; }
    public abstract decimal PorcentajeSalario { get; }
    public abstract decimal PorcentajeCompensacion { get; }
}

/// <summary>Salario ordinario: el IBC es el 100% del salario.</summary>
public class SalarioLey50 : TipoSalario
{
    public override Tipo Tipo => Tipo.Ley50;
    public override decimal PorcentajeSalario => 1m;
    public override decimal PorcentajeCompensacion => 0m;
}

/// <summary>Salario integral (art. 132 CST): el IBC es el 70%; el 30% es factor prestacional.</summary>
public class SalarioIntegral : TipoSalario
{
    public override Tipo Tipo => Tipo.Integral;
    public override decimal PorcentajeSalario => 0.7m;
    public override decimal PorcentajeCompensacion => 0.3m;
}
