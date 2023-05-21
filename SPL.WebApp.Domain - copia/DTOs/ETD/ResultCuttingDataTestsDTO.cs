namespace SPL.WebApp.Domain.DTOs.ETD
{
    using System.Collections.Generic;

    public class ResultCuttingDataTestsDTO
    {
        public List<ErrorColumnsDTO> MessageErrors { get; set; }
        public HeaderCuttingDataDTO Data { get; set; }
    }
}
