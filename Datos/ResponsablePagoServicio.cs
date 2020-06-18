using System;
using System.Collections.Generic;
using System.Linq;
using IDatos;
using Modelos.Entidades;
using Modelos.Enumeracion;

namespace Datos
{
    public partial class IncapacidadesContext : IServicioDatos
    {
        public List<ResponsablePago> ObtenerResponsablesPago()
        {
            return new List<ResponsablePago>
            {
                new ResponsablePago(1, Entidad.EMPRESA, TipoIncapacidad.EnfermedadGeneral, 1,2, 1m),
                new ResponsablePago(2, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 3, 90, 0.6667m),
                new ResponsablePago(3, Entidad.EPS, TipoIncapacidad.EnfermedadGeneral, 91, 180, 0.5m),
                new ResponsablePago(4, Entidad.EPS, TipoIncapacidad.LicenciaMaternidad, 126, 126, 1m),
                new ResponsablePago(5, Entidad.EPS, TipoIncapacidad.LicenciaPaternidad, 8, 8, 1m),
                new ResponsablePago(6, Entidad.ARL, TipoIncapacidad.EnfermedadLaboral, 1, 180, 1m),
                new ResponsablePago(7, Entidad.ARL, TipoIncapacidad.AccidenteLaboral, 1, 180, 1m)

            };
        }

        public ResponsablePago ObtenerResponsablePago(int id)
        {
            return ObtenerResponsablesPago().Where(responsable => responsable.Id == id).FirstOrDefault();
        }
    }    
}
