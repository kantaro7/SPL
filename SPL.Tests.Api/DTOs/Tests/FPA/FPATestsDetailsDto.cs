namespace SPL.Tests.Api.DTOs.Tests.FPA
{
    using System.Collections.Generic;

    public class FPATestsDetailsDto
    {
        public List<FPAPowerFactorDto> FPAPowerFactor { get; set; }
        public List<FPADielectricStrengthDto> FPADielectricStrength { get; set; }
        public FPAWaterContentDto FPAWaterContent { get; set; }
        public FPAGasContentDto FPAGasContent { get; set; }
    }
}
