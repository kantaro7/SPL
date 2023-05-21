namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultRODTestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public List<RODTestsDTO> RODTests { get; set; }
    }
}
