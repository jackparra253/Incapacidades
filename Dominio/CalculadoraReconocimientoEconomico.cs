using IDominio;
using Modelos.Constantes;
using Modelos.Enumeracion;
using Modelos.ValueObjects;

namespace Dominio
{
    public class CalculadoraReconocimientoEconomico : ICalculadoraReconocimientoEconomico
    {
        public Dinero CalcularReconocimientoEconomico(TipoSalario tipoSalario, Dinero salarioDiaro, decimal porcentajeReconocimiento)
        {
            decimal reconocimientoEconomico = salarioDiaro.Cantidad * porcentajeReconocimiento;

            return new Dinero(reconocimientoEconomico, Moneda.COP);
        }
    }
}
