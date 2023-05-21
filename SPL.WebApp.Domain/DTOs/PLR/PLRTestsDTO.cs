namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class PLRTestsDTO
    {
        public string KeyTest { get; set; }
        public decimal PorcDeviationNV { get; set; }
        public decimal Rldnt { get; set; }
        public decimal NominalVoltage { get; set; }
        public List<PLRTestsDetailsDTO> PLRTestsDetails { get; set; }
    }
}
