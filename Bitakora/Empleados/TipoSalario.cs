namespace Bitakora.Empleados;

public abstract class TipoSalario
{
    public abstract Tipo Tipo { get; }
    public abstract decimal PorcentajeSalario { get; }
    public abstract decimal PorcentajeCompensacion { get; }
}

public class SalarioLey50 : TipoSalario
{
    public override Tipo Tipo => Tipo.Ley50;
    public override decimal PorcentajeSalario => 1m;
    public override decimal PorcentajeCompensacion => 0m;
}

public class SalarioIntegral : TipoSalario
{
    public override Tipo Tipo => Tipo.Integral;
    public override decimal PorcentajeSalario => 0.7m;
    public override decimal PorcentajeCompensacion => 0.3m;
}
