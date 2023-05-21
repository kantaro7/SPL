using SPL.Domain.SPL.Artifact.Nozzles;
using SPL.Domain.SPL.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.PCE
{
    public class PCETestsDetails
    {
        public decimal PorcentajeVN { get; set; }
        public decimal NominalKV { get; set; }
        public decimal PerdidasKW { get; set; }
        public decimal CorrienteIRMS { get; set; }
        public decimal TensionKVRMS { get; set; }
        public decimal TensionKVAVG { get; set; }

        public decimal PerdidasOndaKW { get; set; }
        public decimal Corregidas20KW { get; set; }
        public decimal PorcentajeIexc { get; set; }

    }
}
