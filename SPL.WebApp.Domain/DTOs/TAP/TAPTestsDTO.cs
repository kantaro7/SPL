namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class TAPTestsDTO
    {
        public decimal Freacuency { get; set; }
        public int ValueAcep { get; set; }
        public List<TAPTestsDetailsDTO> TAPTestsDetails { get; set; }
    }
}
