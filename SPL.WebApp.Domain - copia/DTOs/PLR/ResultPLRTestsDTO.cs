namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;
    public class ResultPLRTestsDTO
    {
        public PLRTestsDTO PLRTests { get; set; }
        public List<ErrorColumnsDTO> Results { get; set; }
    }
}
