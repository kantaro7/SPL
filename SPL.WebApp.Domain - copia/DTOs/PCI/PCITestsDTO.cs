namespace SPL.WebApp.Domain.DTOs
{
    using System;
    using System.Collections.Generic;

    public class PCITestsDTO
    {
        public string ValueTapPositions { get; set; }
        public PlateTensionDTO PlateTensionsPrim { get; set; }
        public RODTestsGeneralDTO RODTestsGeneralsPrim { get; set; }
        public PCETestsGeneralDTO PCETestsGeneralsPrim { get; set; }
        public DateTime Date { get; set; }
        public List<PCITestsDetailsDTO> PCITestsDetails{ get; set; }
    }
}
