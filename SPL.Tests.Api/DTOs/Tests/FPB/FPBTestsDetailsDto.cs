using SPL.Tests.Api.DTOs.Tests;
using SPL.Tests.Api.DTOs.Tests.Nozzles;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Tests.Api.DTOs.Tests.FPB
{
    public class FPBTestsDetailsDto
    {
        public string Id { get; set; }
        public string NroSerie { get; set; }
        public string T { get; set; }
        public decimal Current { get; set; }
        public decimal Power { get; set; }

        public decimal PercentageA { get; set; }
        public decimal PercentageB { get; set; }
    
        public decimal Capacitance { get; set; }
        public CorrectionFactorsXMarksXTypesDto CorrectionFactorSpecificationsTemperature { get; set; }
        public CorrectionFactorsXMarksXTypesDto CorrectionFactorSpecifications20Grados { get; set; }
        public RecordNozzleInformationDto NozzlesByDesign { get; set; }

    }
}
