namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultPRDTestsDTO
    {
        public PRDTestsDTO PRDTests { get; set; }
        public List<ErrorColumnsDTO> Results { get; set; }
    }
}
