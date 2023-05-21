namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class RCTTestsDTO
    {
        public decimal AcceptanceValue { get; set; }

        public List<RCTTestsDetailsDTO> RCTTestsDetails { get; set; }
    }
}
