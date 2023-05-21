using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class SplInfoSeccionIsz
    {
        public decimal IdRep { get; set; }
        public decimal IdSeccion { get; set; }
        public string DevEnergizado { get; set; }
        public decimal ImpedanciaGar { get; set; }
        public decimal CapBase { get; set; }
        public string UmcapBase { get; set; }
        public decimal Temperatura { get; set; }
        public string UmTemp { get; set; }
        public decimal FactorCorr { get; set; }
        public decimal? PorcZ { get; set; }
        public decimal? PorcR { get; set; }
        public decimal? PorcJx { get; set; }
    }
}
