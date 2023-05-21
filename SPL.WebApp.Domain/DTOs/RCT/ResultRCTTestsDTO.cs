namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultRCTTestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public List<RCTTestsDTO> RCTTests { get; set; }
    }
}
