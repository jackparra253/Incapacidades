
using Modelos.Enumeracion;

namespace Modelos.Entidades
{
    public class TipoSalario
    {
        public Tipo Tipo { get; private set; }

        public decimal PorcentajeSalario { get; private set; }
        public decimal PorcentajeCompensacion { get; private set; }

        public TipoSalario(Tipo tipo)
        {
            Tipo = tipo;

            if (tipo == Tipo.Ley50)
                AsignarPorcentajeSalarioYPorcentajeCompesacion(1, 0);

            if (tipo == Tipo.Integral)
                AsignarPorcentajeSalarioYPorcentajeCompesacion(0.7m, 0.3m);
        }

        private void AsignarPorcentajeSalarioYPorcentajeCompesacion(decimal porcentajeSalario, decimal porcentajeCompensacion)
        {
            PorcentajeSalario = porcentajeSalario;
            PorcentajeCompensacion = porcentajeCompensacion;
        }
    }
}