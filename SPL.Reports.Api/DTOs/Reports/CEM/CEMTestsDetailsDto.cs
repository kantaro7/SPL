using System;
using System.ComponentModel.DataAnnotations;

namespace SPL.Reports.Api.DTOs.Reports.CEM
{
    public class CEMTestsDetailsDto
    {
        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo Posición Secundaria solo puede tener {0} caracteres")]
        public string PosSec { get; set; }

        [Range(0.001, 999999.999, ErrorMessage = "El campo Corriente Term 1 debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal CurrentTerm1 { get; set; }

        [Range(0.001, 999999.999, ErrorMessage = "El campo Corriente Term 2 debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal CurrentTerm2 { get; set; }

        [Range(0.001, 999999.999, ErrorMessage = "El campo Corriente Term 3 debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal CurrentTerm3 { get; set; }
     
    }
}