namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultPCETestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public List<PCETestsDTO> PCETests { get; set; }
    }
}
