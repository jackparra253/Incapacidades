using IDominio;
using Modelos.Constantes;
using Modelos.Entidades;
using Modelos.Enumeracion;
using Modelos.ValueObjects;

namespace Dominio
{
    public class CalculadoraReconocimientoEconomico : ICalculadoraReconocimientoEconomico
    {
        public Dinero CalcularReconocimientoEconomico(Empleado empleado, ResponsablePago responsablePago)
        {
            decimal salarioDiarioBase = empleado.SalarioDiario.Cantidad * responsablePago.PorcentajeSalario;

            decimal reconocimientoEconomico = salarioDiarioBase * responsablePago.ReconocimientoPorcentaje;

            return new Dinero(reconocimientoEconomico, empleado.SalarioDiario.Moneda);
        }
    }
}
