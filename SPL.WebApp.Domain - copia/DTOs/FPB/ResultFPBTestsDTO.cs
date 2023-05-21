namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultFPBTestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public List<FPBTestsDTO> FPBTests { get; set; }
    }
}
