using Modelos.Entidades;
using Modelos.ValueObjects;

namespace IDominio
{
    public interface ICalculadoraReconocimientoEconomico
    {
        Dinero CalcularReconocimientoEconomico(Empleado empleado, ResponsablePago responsablePago, int cantidadDiasRestantes);
    }
}