using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Security.Infrastructure.Entities
{
    public partial class SplInfoDetalleDpr
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public string Tiempo { get; set; }
        public decimal Tension { get; set; }
        public decimal? Terminal1Pc { get; set; }
        public decimal? Terminal1Mv { get; set; }
        public decimal? Terminal2Pc { get; set; }
        public decimal? Terminal2Mv { get; set; }
        public decimal? Terminal3Pc { get; set; }
        public decimal? Terminal3Mv { get; set; }
    }
}
