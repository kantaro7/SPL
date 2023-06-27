namespace SPL.WebApp.Domain.DTOs.ETD
{
    using System.Collections.Generic;

    public class ETDUploadResultDTO
    {
        public List<ETDReportDTO> ETDReports { get; set; }
        public List<HeaderCuttingDataDTO> HeaderCuttingDatas { get; set; }

        public List<ErrorColumnsDTO> Errors { get; set; }
    }
}
