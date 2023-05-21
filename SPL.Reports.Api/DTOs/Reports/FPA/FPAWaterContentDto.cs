namespace SPL.Reports.Api.DTOs.Reports.FPA
{
   public class FPAWaterContentDto
    {

        public string Test { get; set; }

        public string ASTM { get; set; }
        public decimal OneSt { get; set; }
        public decimal TwoNd { get; set; }
        public decimal ThreeRd { get; set; }

        public decimal Average { get; set; }
        public decimal MaxLimit1 { get; set; }
        public string MaxLimit2 { get; set; }
    }
}
