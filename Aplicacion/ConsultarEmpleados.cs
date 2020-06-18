using System.Collections.Generic;
using IAplicacion;
using IDatos;
using Modelos.Entidades;

namespace Aplicacion
{
    public class ConsultarEmpleados: IConsultarEmpleados
    {
        private readonly IServicioDatos _servicioDatos;

        public ConsultarEmpleados(IServicioDatos servicioDatos)
        {
            _servicioDatos = servicioDatos;
        }

        public List<Empleado> ObtenerEmpleados()
        {
            return _servicioDatos.ObtenerEmpleados();
        }
    }
}
