using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace SPL.Reports.Api.DTOs.Reports.ISZ
{
    public class ISZTestsDetailsDto
    {
        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo Posición 1 solo puede tener {0} caracteres")]
        public string Position1 { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo Posición 2 solo puede tener {0} caracteres")]
        public string Position2 { get; set; }

        [Range(0.001, 99999999.999, ErrorMessage = "El campo Tensión 1 debe ser numérico mayor a cero considerando hasta 8 enteros y 3 decimales")]
        public decimal Voltage1 { get; set; }

        [Range(0.001, 99999999.999, ErrorMessage = "El campo Tensión 2 debe ser numérico mayor a cero considerando hasta 8 enteros y 3 decimales")]
        public decimal Voltage2 { get; set; }

        [Range(0.1, 999999.9, ErrorMessage = "El campo Tensión VRMS debe ser numérico mayor a cero considerando hasta 6 enteros y 1 decimal")]
        public decimal VoltsVRMS { get; set; }

        [Range(0.001, 999999.999, ErrorMessage = "El campo Corriente IRMS debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal CurrentsIRMS { get; set; }

        [Range(0.001, 999.999, ErrorMessage = "El campo Potencia KW debe ser numérico mayor a cero considerando hasta 3 enteros y 3 decimales")]
        public decimal PowerKW { get; set; }

        [Range(0.001, 999.999, ErrorMessage = "El campo Potencia KW Corregida debe ser numérico mayor a cero considerando hasta 3 enteros y 3 decimales")]
        public decimal PowerKWVoltage1 { get; set; }

        [Range(0.001, 999.999, ErrorMessage = "El campo Porcentaje de Ro debe ser numérico mayor a cero considerando hasta 3 entero y 3 decimales")]
        public decimal PercentageRo { get; set; }

        [Range(0.001, 999.999, ErrorMessage = "El campo Porcentaje de jXo debe ser numérico mayor a cero considerando hasta 3 entero y 3 decimales")]
        public decimal PercentagejXo { get; set; }

        [Range(0.001, 999.999, ErrorMessage = "El campo Porcentaje de Zo debe ser numérico mayor a cero considerando hasta 3 entero y 3 decimales")]
        public decimal PercentageZo { get; set; }

        [Range(0.000001, 999.999999, ErrorMessage = "El campo Factor de Corr. debe ser numérico mayor a cero considerando hasta 3 enteros y 6 decimales")]
        public decimal FactorCorr { get; set; }

        [Range(0.001, 99999.999, ErrorMessage = "El campo Z Base debe ser numérico mayor a cero considerando hasta 5 enteros y 3 decimales")]
        public decimal ZBase { get; set; }
        public decimal ZOhms { get; set; }
        public Complex PercentageComplex { get; set; }


    }
}
