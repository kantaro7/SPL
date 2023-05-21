namespace SPL.WebApp.Domain.DTOs.DPR
{
    using System.Collections.Generic;

    public class DPRTestsDetailsDTO
    {
        public string Time { get; set; }
        public decimal Voltage { get; set; }
        public List<DPRTerminalsDTO> DPRTerminals { get; set; }
    }
}
