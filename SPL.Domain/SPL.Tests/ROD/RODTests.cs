namespace SPL.Domain.SPL.Tests.ROD
{
    using System;
    using System.Collections.Generic;

    using global::SPL.Domain.SPL.Configuration;

    public class RODTests
    {
        public decimal FactorCor20 { get; set; }
        public decimal FactorCorSE { get; set; }
        public string WindingMaterial { get; set; }

        public decimal BoostTemperature { get; set; }
        public decimal TempFP { get; set; }
        public decimal TempTanD { get; set; }

        public decimal Tension { get; set; }
        public string UmTension { get; set; }

        public decimal Temperature { get; set; }
        public string UmTemperature { get; set; }

        public decimal ValorAcepPhases { get; set; }
        public decimal ValorAcMaDesign { get; set; }
        public decimal AcMiDesignValue { get; set; }

        public decimal MaxPorc_Desv { get; set; }
        public decimal MaxPorc_DesvxDesign { get; set; }
        public decimal MinPorc_DesvxDesign { get; set; }

        public DateTime Date { get; set; }
  
        public List<RODTestsDetails> RODTestsDetails { get; set; }
    }
}
