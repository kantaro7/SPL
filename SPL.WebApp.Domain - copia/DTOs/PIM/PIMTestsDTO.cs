namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class PIMTestsDTO
    {
        public string KeyTest { get; set; }

        public List<PIMTestsDetailsDTO> PIMTestsDetails { get; set; }
    }
}
