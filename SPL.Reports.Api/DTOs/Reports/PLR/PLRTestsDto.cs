namespace SPL.Reports.Api.DTOs.Reports.FPC
{
    using System;
    using System.Collections.Generic;

    public class PLRTestsDto
    {
        public string KeyTest { get; set; }
        public decimal PorcDeviationNV { get; set; }
        public decimal Rldnt { get; set; }
        public decimal NominalVoltage { get; set; }
        public List<PLRTestsDetailsDto> PLRTestsDetails { get; set; }

    }
}
