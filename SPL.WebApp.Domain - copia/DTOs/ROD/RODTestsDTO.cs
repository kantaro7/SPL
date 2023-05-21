﻿namespace SPL.WebApp.Domain.DTOs
{
    using System;
    using System.Collections.Generic;

    public class RODTestsDTO
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

        public decimal TemperatureSE { get; set; }
        public string UmTemperatureSE { get; set; }

        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Title3 { get; set; }

        public List<RODTestsDetailsDTO> RODTestsDetails { get; set; }
    }
}
