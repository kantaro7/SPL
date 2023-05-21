namespace SPL.WebApp.Domain.DTOs
{
    public class PEETestsDetailsDTO
    {
        public string CoolingType { get; set; }
        public decimal VoltageRMS { get; set; }
        public decimal CurrentRMS { get; set; }
        public decimal PowerKW { get; set; }
        public decimal Kwaux_gar { get; set; }
        public decimal Mvaaux_gar { get; set; }
    }
}
