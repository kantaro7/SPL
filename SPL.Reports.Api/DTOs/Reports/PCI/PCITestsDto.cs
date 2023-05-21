namespace SPL.Reports.Api.DTOs.Reports.PCI
{
    using System;
    using System.Collections.Generic;

    using global::SPL.Domain.SPL.Artifact.PlateTension;
    using global::SPL.Domain.SPL.Configuration;
    using global::SPL.Domain.SPL.Reports.PCE;
    using global::SPL.Domain.SPL.Reports.ROD;

    using SPL.Reports.Api.DTOs.Reports.PCE;
    using SPL.Reports.Api.DTOs.Reports.ROD;

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
