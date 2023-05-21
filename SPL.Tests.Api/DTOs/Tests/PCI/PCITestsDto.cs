namespace SPL.Tests.Api.DTOs.Tests.PCI
{
    using System;
    using System.Collections.Generic;

    using global::SPL.Domain.SPL.Artifact.PlateTension;
    using global::SPL.Domain.SPL.Configuration;
    using global::SPL.Domain.SPL.Reports.PCE;
    using global::SPL.Domain.SPL.Reports.ROD;

    using SPL.Tests.Api.DTOs.Tests.PCE;
    using SPL.Tests.Api.DTOs.Tests.ROD;

    public class PCITestsDto
    {
        public string ValueTapPositions { get; set; }
        public PlateTensionDto PlateTensionsPrim { get; set; }
        public RODTestsGeneralDto RODTestsGeneralsPrim { get; set; }
        public PCETestsGeneralDto PCETestsGeneralsPrim { get; set; }
 
        public DateTime Date { get; set; }
   
        public List<PCITestsDetailsDto> PCITestsDetails { get; set; }
    }
}
