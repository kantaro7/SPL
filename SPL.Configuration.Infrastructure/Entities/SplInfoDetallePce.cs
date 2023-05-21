using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoDetallePce
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public decimal PorcVn { get; set; }
        public decimal NominalKv { get; set; }
        public decimal PerdidasKw { get; set; }
        public decimal CorrienteIrms { get; set; }
        public decimal TensionRms { get; set; }
        public decimal TensionAvg { get; set; }
        public decimal PerdidasOnda { get; set; }
        public decimal PerdidasCorr20 { get; set; }
        public decimal PorcIexc { get; set; }
    }
}
