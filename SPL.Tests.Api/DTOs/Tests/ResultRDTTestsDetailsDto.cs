namespace SPL.Tests.Api.DTOs.Tests
{
    using System.Collections.Generic;

    public class ResultRDTTestsDetailsDto
    {
        public List<ErrorColumnsDto> MessageErrors { get; set; }
        public List<decimal> NominalValue { get; set; }
        public List<decimal> DeviationPhasesA { get; set; }
        public List<decimal> DeviationPhasesB { get; set; }
        public List<decimal> DeviationPhasesC { get; set; }
        public List<decimal> HVVolts { get; set; }
        public List<decimal> LVVolts { get; set; }
    }
}
