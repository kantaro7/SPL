namespace SPL.Reports.Api.DTOs.Reports
{
    using System;

    public class HeaderRADTestsDto
    {
        public decimal IdLoad { get; set; }
        public decimal Section { get; set; }
        public decimal? Tension { get; set; }
        public string UnitOfMeasureOfTension { get; set; }
        public string UnitOfMeasureOfTemperature { get; set; }
        public decimal? Temperature { get; set; }
        public DateTime? TestDate { get; set; }
    }
}
