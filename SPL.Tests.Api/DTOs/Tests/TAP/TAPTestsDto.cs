namespace SPL.Tests.Api.DTOs.Tests.TAP
{
    using System.Collections.Generic;

    public class TAPTestsDto
    {
        public decimal Freacuency { get; set; }
        public int ValueAcep { get; set; }
        public List<TAPTestsDetailsDto> TAPTestsDetails { get; set; }
    }
}
