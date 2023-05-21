namespace SPL.Reports.Api.DTOs.Reports.PRD
{
    using System;
    using System.Collections.Generic;

    public class PRDTestsDto
    {
        public string KeyTest { get; set; }
        public decimal NominalVoltage { get; set; }
        public PRDTestsDetailsDto PRDTestsDetails { get; set; }

    }
}
