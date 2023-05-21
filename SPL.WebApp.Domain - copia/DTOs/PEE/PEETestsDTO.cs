namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class PEETestsDTO
    {
        public string KeyTest { get; set; }

        public List<PEETestsDetailsDTO> PEETestsDetails { get; set; }
    }
}
