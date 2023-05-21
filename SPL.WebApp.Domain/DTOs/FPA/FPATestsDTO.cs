namespace SPL.WebApp.Domain.DTOs
{
    public class FPATestsDTO
    {
        public string ClavePrueba { get; set; }
        public string SampleConditions { get; set; }
        public string KeyLanguage { get; set; }
        public string OilType { get; set; }
        public decimal AmbientTemperature { get; set; }
        public decimal RelativeHumidity { get; set; }
        public FPATestsDetailsDTO FPATestsDetails { get; set; }
    }
}
