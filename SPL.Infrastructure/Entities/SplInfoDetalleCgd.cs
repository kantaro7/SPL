using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Artifact.Infrastructure.Entities
{
    public partial class SplInfoDetalleCgd
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public string Descripcion { get; set; }
        public decimal? ResultadoPpm { get; set; }
        public decimal? AceptacionPpm { get; set; }
        public decimal? AntesPpm { get; set; }
        public decimal? DespuesPpm { get; set; }
        public decimal? IncrementoPpm { get; set; }
        public decimal? LimiteMax { get; set; }
        public string Validacion { get; set; }
    }
}
