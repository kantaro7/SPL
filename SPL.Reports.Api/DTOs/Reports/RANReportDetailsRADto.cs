using System;
using System.ComponentModel.DataAnnotations;

namespace SPL.Reports.Api.DTOs.Reports
{
    public class RANReportDetailsRADto
    {
        public int Section { get; set; }

        public DateTime DateTest { get; set; }
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "El campo Descripción solo puede tener {0} caracteres")]
        public string Description { get; set; }

        [Range(0, 99999999, ErrorMessage = "El campo Medición debe ser numérico mayor a cero considerando hasta 8 enteros")]
        public decimal Measurement { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Unidad de medición solo puede tener {0} caracteres")]
        public string UMMeasurement { get; set; }

        [Range(0, 99999999, ErrorMessage = "El campo VCD debe ser numérico mayor a cero considerando hasta 8 enteros")]
        public decimal VCD { get; set; }

        [Range(0, 99999999, ErrorMessage = "El campo Limite debe ser numérico mayor a cero considerando hasta 8 enteros")]
        public decimal Limit { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(10, ErrorMessage = "El campo Duración solo puede tener {0} caracteres")]
        public string Duration { get; set; }

        [Range(0, 99999999, ErrorMessage = "El campo Tiempo debe ser numérico mayor a cero considerando hasta 8 enteros")]
        public decimal Time { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(10, ErrorMessage = "El campo Unidad de Tiempo solo puede tener {0} caracteres")]
        public string UMTime { get; set; }

    } 
}