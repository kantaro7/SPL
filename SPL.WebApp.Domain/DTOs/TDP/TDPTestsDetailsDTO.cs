namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class TDPTestsDetailsDTO
    {
        public string Time { get; set; }
        public decimal Voltage { get; set; }
        public List<TDPTerminalsDTO> TDPTerminals { get; set; }
    }
}
