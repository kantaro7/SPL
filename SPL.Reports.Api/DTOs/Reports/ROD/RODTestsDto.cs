namespace SPL.Reports.Api.DTOs.Reports.ROD
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RODTestsDto
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

        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Title3 { get; set; }

        public decimal TemperatureSE { get; set; }
        public string UmTemperatureSE { get; set; }

        public decimal ValorAcepPhases { get; set; }
        public decimal ValorAcMaDesign { get; set; }
        public decimal AcMiDesignValue { get; set; }

        public string AutorizoCambio { get; set; }

        [Range(0, 999.99, ErrorMessage = "El valor de Porcentaje de desviacion debe ser numérico mayor a cero considerando 3 enteros con 2 decimal")]
        public decimal MaxPorc_Desv { get; set; }

        [Range(0, 999.99, ErrorMessage = "El valor de Máximo Porcentaje de desviacion debe ser numérico mayor a cero considerando 3 enteros con 2 decimal")]

        public decimal MaxPorc_DesvxDesign { get; set; }

        [Range(0, 999.99, ErrorMessage = "El valor de Mínimo Porcentaje de desviacion debe ser numérico mayor a cero considerando 3 enteros con 2 decimal")]

        public decimal MinPorc_DesvxDesign { get; set; }

        public DateTime Date { get; set; }

        public List<RODTestsDetailsDto> RODTestsDetails { get; set; }
    }
}
