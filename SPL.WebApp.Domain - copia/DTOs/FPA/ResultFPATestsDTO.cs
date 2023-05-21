namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultFPATestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public FPATestsDTO FPATests { get; set; }
    }
}
