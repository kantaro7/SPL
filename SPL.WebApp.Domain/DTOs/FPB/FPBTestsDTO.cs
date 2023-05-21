﻿namespace SPL.WebApp.Domain.DTOs
{
    using System;
    using System.Collections.Generic;

    public class FPBTestsDTO
    {

        public string TanDelta { get; set; }
        public string KeyTest { get; set; }
        public decimal TempFP { get; set; }
        public decimal TempTanD { get; set; }
        public decimal ToleranciaFP { get; set; }
        public decimal ToleranciaCap { get; set; }
        public decimal Tension { get; set; }
        public string UmTension { get; set; }
        public decimal Temperature { get; set; }
        public string UmTemperature { get; set; }

        public DateTime Date { get; set; }
        public decimal AcceptanceValueCap { get; set; }
        public decimal AcceptanceValueFP { get; set; }
        public List<FPBTestsDetailsDTO> FPBTestsDetails { get; set; }
        public List<decimal> CalValorAceptFP { get; set; }
        public List<decimal> CalValorAceptCAP { get; set; }
    }
}
