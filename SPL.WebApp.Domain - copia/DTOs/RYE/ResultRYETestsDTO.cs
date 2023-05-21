namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;
    public class ResultRYETestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public OutRYETestsDTO RYETests { get; set; }
    }
}
