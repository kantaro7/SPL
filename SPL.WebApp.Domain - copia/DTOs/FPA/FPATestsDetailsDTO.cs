namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class FPATestsDetailsDTO
    {
        public List<FPAPowerFactorDTO> FPAPowerFactor { get; set; }
        public List<FPADielectricStrengthDTO> FPADielectricStrength { get; set; }
        public FPAWaterContentDTO FPAWaterContent { get; set; }
        public FPAGasContentDTO FPAGasContent { get; set; }
    }
}
