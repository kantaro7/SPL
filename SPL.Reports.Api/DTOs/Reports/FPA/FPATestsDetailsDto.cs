using SPL.Domain.SPL.Artifact.Nozzles;
using SPL.Domain.SPL.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.FPA
{
    public class FPATestsDetailsDto
    {
        public List<FPAPowerFactorDto> FPAPowerFactor { get; set; }
        public List<FPADielectricStrengthDto> FPADielectricStrength { get; set; }
        public FPAWaterContentDto FPAWaterContent { get; set; }
        public FPAGasContentDto FPAGasContent { get; set; }

       

    }
}
