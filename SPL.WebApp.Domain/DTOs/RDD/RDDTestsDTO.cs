namespace SPL.WebApp.Domain.DTOs
{
    using System;
    using System.Collections.Generic;

    public class RDDTestsDTO
    {
        public string Specification { get; set; }
        public CorrectionFactorDTO CorrectionFactorSpecifications { get; set; }
        public string UmTempacSup { get; set; }
        public decimal UpperOilTemperature { get; set; }
        public string UmTempacInf { get; set; }
        public decimal LowerOilTemperature { get; set; }
        public decimal Frequency { get; set; }
        public string UmTension { get; set; }
        public decimal Tension { get; set; }
        public decimal TempFP { get; set; }
        public decimal TempTanD { get; set; }
        public DateTime Date { get; set; }
        public decimal AcceptanceValueCap { get; set; }
        public decimal AcceptanceValueFP { get; set; }
        public List<RDDTestsDetailsDTO> RDDTestsDetails { get; set; }

        public List<decimal> Capacitance { get; set; }
    }
}
