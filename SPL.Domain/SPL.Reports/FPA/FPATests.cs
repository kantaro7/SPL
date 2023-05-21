namespace SPL.Domain.SPL.Reports.FPA
{
    using System;
    using System.Collections.Generic;

    using global::SPL.Domain.SPL.Configuration;

    public class FPATests
    {
 
   

        public string SampleConditions { get; set; }
        public string KeyLanguage { get; set; }
        public string OilType { get; set; }
        public decimal AmbientTemperature { get; set; }
        public decimal RelativeHumidity { get; set; }
        public FPATestsDetails FPATestsDetails { get; set; }
    }
}
