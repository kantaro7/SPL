namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultPCITestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public PCIOutTestsDTO PCIOutTests { get; set; }
    }
}
