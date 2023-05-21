namespace SPL.Tests.Api.DTOs.Tests.FPC
{

    using System;
    using System.Collections.Generic;

    public class FPCTestsDto
    {
        public string Specification { get; set; }
        public string UnitType { get; set; }
        public CorrectionFactorSpecificationDto CorrectionFactorSpecifications { get; set; }
        public decimal UpperOilTemperature { get; set; }
        public decimal LowerOilTemperature { get; set; }
        public decimal TempFP { get; set; }
        public decimal TempTanD { get; set; }
        public decimal Frequency { get; set; }
        public DateTime Date { get; set; }
        public decimal Tension { get; set; }
        public string UmTension { get; set; }
        public string UmTempacSup { get; set; }
        public string UmTempacInf { get; set; }
        public decimal AcceptanceValueCap { get; set; }
        public decimal AcceptanceValueFP { get; set; }
        public List<FPCTestsDetailsDto> FPCTestsDetails { get; set; }

        public List<FPCTestsValidationsCapDto> Capacitance { get; set; }
    }
}
