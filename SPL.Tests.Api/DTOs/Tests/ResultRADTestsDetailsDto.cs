namespace SPL.Tests.Api.DTOs.Tests
{
    using System.Collections.Generic;

    public class ResultRADTestsDetailsDto
    {
        public List<ErrorColumnsDto> MessageErrors { get; set; }
        public List<decimal> AbsorptionIndexs { get; set; }
        public List<decimal> PolarizationIndexs { get; set; }
    }
}
