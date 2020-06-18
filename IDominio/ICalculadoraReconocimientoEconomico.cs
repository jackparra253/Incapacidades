using Modelos.Enumeracion;
using Modelos.ValueObjects;

namespace IDominio
{
    public interface ICalculadoraReconocimientoEconomico
    {
        Dinero CalcularReconocimientoEconomico(TipoSalario tipoSalario, Dinero salarioDiaro, decimal porcentajeReconocimiento);
    }
}