namespace SPL.WebApp.Domain.DTOs.ETD
{
    using System.Collections.Generic;

    public class ResultETDTestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public ETDTestsGeneralDTO ETDTestsGeneral { get; set; }
    }
}
