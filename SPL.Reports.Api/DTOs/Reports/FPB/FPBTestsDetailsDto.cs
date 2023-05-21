using System.ComponentModel.DataAnnotations;
using SPL.Reports.Api.DTOs.Reports.Nozzles;

namespace SPL.Reports.Api.DTOs.Reports.FPB
{
    public class FPBTestsDetailsDto
    {
        public string Id { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "El campo Número de Serie solo puede tener {0} caracteres")]
        public string NroSerie { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(2, ErrorMessage = "El campo T solo puede tener {0} caracteres")]
        public string T { get; set; }

        [Range(0.001, 999999.999, ErrorMessage = "El campo Corriente debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal Current { get; set; }

        [Range(0.001, 999.999, ErrorMessage = "El campo Potencia debe ser numérico mayor a cero considerando hasta 3 enteros y 3 decimales")]
        public decimal Power { get; set; }

        [Range(0.001, 999.999, ErrorMessage = "El campo Porcentaje A debe ser numérico mayor a cero considerando hasta 3 enteros y 3 decimales")]
        public decimal PercentageA { get; set; }

        [Range(0.001, 999.999, ErrorMessage = "El campo Porcentaje B debe ser numérico mayor a cero considerando hasta 3 enteros y 3 decimales")]
        public decimal PercentageB { get; set; }

        [Range(1, 999999, ErrorMessage = "El campo Capacitancia debe ser numérico mayor a cero considerando hasta 6 enteros")]
        public decimal Capacitance { get; set; }
        public RecordNozzleInformationDto NozzlesByDesign { get; set; }
        public CorrectionFactorsXMarksXTypesDto CorrectionFactorSpecificationsTemperature { get; set; }
        public CorrectionFactorsXMarksXTypesDto CorrectionFactorSpecifications20Grados { get; set; }
    }
}
