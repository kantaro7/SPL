namespace SPL.WebApp.Domain.DTOs
{
    public class FPBTestsDetailsDTO
    {
        public string Id { get; set; }
        public string NroSerie { get; set; }
        public string T { get; set; }
        public decimal Current { get; set; }
        public decimal Power { get; set; }

        public decimal PercentageA { get; set; }
        public decimal PercentageB { get; set; }

        public decimal Capacitance { get; set; }
        public CorrectionFactorsXMarksXTypesDTO CorrectionFactorSpecificationsTemperature { get; set; }
        public CorrectionFactorsXMarksXTypesDTO CorrectionFactorSpecifications20Grados { get; set; }
        public RecordNozzleInformationDTO NozzlesByDesign { get; set; }
    }
}
