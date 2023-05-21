namespace SPL.Tests.Api.DTOs.Tests.FPA
{
   public class FPAPowerFactorDto
    {
        public string Temperature { get; set; }
        public string ASMT { get; set; }
        public decimal Measurement { get; set; }
        public decimal Scale { get; set; }
        public decimal CorrectionFactor { get; set; }
        public decimal PowerFactor { get; set; }
        public decimal MaxLimitFP1 { get; set; }
        public string MaxLimitFP2{ get; set; }
    }
}
