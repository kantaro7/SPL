namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;
    public class ResultCGDTestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public List<CGDTestsDTO> CGDTests { get; set; }
    }
}
