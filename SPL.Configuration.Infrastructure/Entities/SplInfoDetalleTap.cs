using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoDetalleTap
    {
        public decimal IdRep { get; set; }
        public decimal Renglon { get; set; }
        public string DevEnergizado { get; set; }
        public string DevAterrizado { get; set; }
        public decimal NivelTension { get; set; }
        public decimal TensionAplicada { get; set; }
        public decimal Corriente { get; set; }
        public decimal Tiempo { get; set; }
        public decimal Capacitancia { get; set; }
        public decimal AmpCal { get; set; }
        public decimal PorcCorriente { get; set; }
    }
}
