using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Security.Infrastructure.Entities
{
    public partial class SplInfoDetallePlr
    {
        public decimal IdRep { get; set; }
        public decimal Renglon { get; set; }
        public decimal Tension { get; set; }
        public decimal Corriente { get; set; }
        public decimal? Reactancia { get; set; }
        public decimal? PorcDesv { get; set; }
        public decimal? Tiempo { get; set; }
    }
}
