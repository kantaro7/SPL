namespace SPL.Reports.Api.DTOs.Reports.PEE
{
    using System;
    using System.Collections.Generic;

    public class PEETestsDto
    {
        public string KeyTest { get; set; }
     
        public List<PEETestsDetailsDto> PEETestsDetails { get; set; }

    }
}
