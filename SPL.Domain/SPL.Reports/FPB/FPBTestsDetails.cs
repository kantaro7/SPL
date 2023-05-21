using SPL.Domain.SPL.Artifact.Nozzles;
using SPL.Domain.SPL.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.FPB
{
    public class FPBTestsDetails
    {
        public string Id { get; set; }
        public string NroSerie { get; set; }
        public string T { get; set; }
        public decimal Current { get; set; }
        public decimal Power { get; set; }

        public decimal PercentageA { get; set; }
        public decimal PercentageB { get; set; }

        public decimal Capacitance { get; set; }
        public RecordNozzleInformation NozzlesByDesign { get; set; }

        public CorrectionFactorsXMarksXTypes CorrectionFactorSpecificationsTemperature { get; set; }
        public CorrectionFactorsXMarksXTypes CorrectionFactorSpecifications20Grados { get; set; }

    }
}
