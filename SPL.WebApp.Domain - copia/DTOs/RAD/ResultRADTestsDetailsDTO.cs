namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultRADTestsDetailsDTO
    {
        public List<ErrorColumnsDTO> MessageErrors { get; set; }
        public List<decimal> AbsorptionIndexs { get; set; }
        public List<decimal> PolarizationIndexs { get; set; }
    }
}
