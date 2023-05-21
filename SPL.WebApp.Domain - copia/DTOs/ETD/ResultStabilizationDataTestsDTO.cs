namespace SPL.WebApp.Domain.DTOs.ETD
{
    using System.Collections.Generic;

    public class ResultStabilizationDataTestsDTO
    {
        public List<ErrorColumnsDTO> MessageErrors { get; set; }
        public StabilizationDataDTO Results { get; set; }
    }
}
