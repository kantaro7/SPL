using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPL.Reports.Api.DTOs.Reports.TDP
{
    public class TDPTestsDetailsDto
    {
        //[DataType(DataType.Text)]
        //[MaxLength(15, ErrorMessage = "El campo Tiempo solo puede tener {0} caracteres")]
        public string Time { get; set; }

        //[Range(0.01, 9999999.99, ErrorMessage = "El campo Tensión debe ser numérico mayor a cero considerando hasta 7 enteros y 2 decimales")]
        public decimal Voltage { get; set; }
        public List<TDPTerminalsDto> TDPTerminals { get; set; }
    }
}
