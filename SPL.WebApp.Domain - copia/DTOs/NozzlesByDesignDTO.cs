namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class NozzlesByDesignDTO
    {
        public int TotalQuantity { get; set; }
        public List<RecordNozzleInformationDTO> NozzleInformation { get; set; }
        public bool OperationType { get; set; }
    }
}
