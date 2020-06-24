using System.Linq;
using IDatos;
using Modelos.Entidades;
using System.Collections.Generic;
using Modelos;
using Modelos.Enumeracion;

namespace Datos
{
    public partial class IncapacidadServicio : IIncapacidadServicio
    {

        private readonly IncapacidadesContext _contexto;

        public IncapacidadServicio(IncapacidadesContext contexto)
        {
            _contexto = contexto;
        }

        public void Guardar(Incapacidad incapacidad)
        {
            _contexto.Incapacidades.Add(incapacidad);
            _contexto.SaveChanges();
        }

        public List<DetalleIncapacidad> ObtenerIncapacidadesDetalle(int idEmpleado)
        {
            List<DetalleIncapacidad> incapacidadesDetalle = _contexto.Incapacidades
                .Where(i => i.IdEmpleado == idEmpleado)
                .Select(i => new DetalleIncapacidad(i.IncapacidadId,
                     (TransformarATextoTipoIncapacida(i.TipoIncapacidad)),
                    i.FechaIncial.ToShortDateString(),
                    i.FechaFinal.ToShortDateString(),
                    i.CantidadDias))
                .ToList();

            return incapacidadesDetalle;
        }

        private static string TransformarATextoTipoIncapacida(TipoIncapacidad tipoIncapacidad)
        {
            switch (tipoIncapacidad)
            {
                case TipoIncapacidad.EnfermedadGeneral:
                    return "Enfermedad General";
                    break;
                case TipoIncapacidad.LicenciaMaternidad:
                    return "Licencia Maternidad";
                    break;
                case TipoIncapacidad.LicenciaPaternidad:
                    return "Licencia Paternidad";
                    break;
                case TipoIncapacidad.EnfermedadLaboral:
                    return "Enfermedad Laboral";
                    break;
                case TipoIncapacidad.AccidenteLaboral:
                    return "AccidenteLaboral";
                    break;
                default:
                    return "";
                    break;
            }

        }

        public List<DetalleReconocimientoEconomico> ObtenerReconocimientosEconomicosDetalle(int idEmpleado)
        {
            var reconocimientosEconomicos = _contexto.ReconocimientosEconomicos.ToList();

            List<DetalleReconocimientoEconomico> reconocimientosEconomicosDetalle = reconocimientosEconomicos
                .Where(re => re.IdEmpleado == idEmpleado)
                .Select(re => new DetalleReconocimientoEconomico(
                    re.IncapacidadId,
                    re.FechaInicial.ToShortDateString(),
                    re.FechaFinal.ToShortDateString(),
                    re.ValorAPagar,
                    TransformarATextoResponsable(re.ResponsablePago)
                )).ToList();

            return reconocimientosEconomicosDetalle;
        }
        private static string TransformarATextoResponsable(Entidad responsable)
        {
            switch (responsable)
            {
                case Entidad.EPS:
                    return "EPS";
                    break;
                case Entidad.ARL:
                    return "ARL";
                    break;
                case Entidad.EMPRESA:
                    return "EMPRESA";
                    break;

                default:
                    return "";
                    break;
            }
        }
    }
}