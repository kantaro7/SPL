namespace SPL.Tests.Api.DTOs.Tests.FPA
{
    using System;
    using System.Collections.Generic;

    using global::SPL.Domain.SPL.Configuration;

    public class FPATestsDto
    {
 
   

        public string SampleConditions { get; set; }
        public string ClavePrueba { get; set; }
        public string KeyLanguage { get; set; }
        public string OilType { get; set; }
        public decimal AmbientTemperature { get; set; }
        public decimal RelativeHumidity { get; set; }
        public FPATestsDetailsDto FPATestsDetails { get; set; }
    }
}
