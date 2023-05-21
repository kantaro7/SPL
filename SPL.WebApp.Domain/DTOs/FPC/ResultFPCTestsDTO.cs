namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultFPCTestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public List<FPCTestsDTO> FPCTests { get; set; }
    }
}
