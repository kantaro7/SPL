namespace SPL.Domain.SPL.Tests.FPC
{
    using System;
    using System.Collections.Generic;

    using global::SPL.Domain.SPL.Configuration;

    public class FPCTests
    {
        public string Specification { get; set; }
        public string UnitType { get; set; }
        public CorrectionFactorSpecification CorrectionFactorSpecifications { get; set; }
        public decimal UpperOilTemperature { get; set; }
        public decimal LowerOilTemperature { get; set; }
        public decimal TempFP { get; set; }
        public decimal TempTanD { get; set; }
        public decimal Frequency { get; set; }
        public decimal Tension { get; set; }
        public string UmTension { get; set; }
        public string UmTempacSup { get; set; }
        public string UmTempacInf { get; set; }
        public DateTime Date { get; set; }
        public decimal AcceptanceValueCap { get; set; }
        public decimal AcceptanceValueFP { get; set; }
        public List<FPCTestsDetails> FPCTestsDetails { get; set; }

        public List<FPCTestsValidationsCap> Capacitance { get; set; }
    }
}
