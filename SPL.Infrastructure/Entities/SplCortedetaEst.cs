using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Artifact.Infrastructure.Entities
{
    public partial class SplCortedetaEst
    {
        public decimal IdCorte { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public decimal Tiempo { get; set; }
        public decimal Resistencia { get; set; }
        public decimal TempR { get; set; }
    }
}
