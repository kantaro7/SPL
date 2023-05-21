namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultRDDTestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public RDDTestsGeneralDTO RDDTestsGeneral { get; set; }
    }
}
