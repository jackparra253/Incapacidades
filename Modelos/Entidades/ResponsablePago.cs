using Modelos.Enumeracion;

namespace Modelos.Entidades
{
    public class ResponsablePago
    {
        public int Id { get; private set; }
        public Entidad Responsable { get; private set; }
        public TipoIncapacidad TipoIncapacidad { get; private set; }
        public int DiasIncapacidadInicial { get; private set; }
        public int DiasIncapacidadFinal { get; private set; }
        public decimal ReconocimientoPorcentaje { get; private set; }

        public ResponsablePago(int id, Entidad responsable, TipoIncapacidad tipoIncapacidad, int diasIncapacidadInicial, int diasIncapacidadFinal, decimal reconocimientoPorcentaje)
        {
            Id = id;
            Responsable = responsable;
            TipoIncapacidad = tipoIncapacidad;
            DiasIncapacidadInicial = diasIncapacidadInicial;
            DiasIncapacidadFinal = diasIncapacidadFinal;
            ReconocimientoPorcentaje = reconocimientoPorcentaje;
        }
    }
}