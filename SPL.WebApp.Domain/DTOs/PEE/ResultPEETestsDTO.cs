namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultPEETestsDTO
    {
        public PEETestsDTO PEETests { get; set; }
        public List<ErrorColumnsDTO> Results { get; set; }
    }
}
