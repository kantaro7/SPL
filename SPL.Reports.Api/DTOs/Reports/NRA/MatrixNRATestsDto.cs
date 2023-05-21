namespace SPL.Reports.Api.DTOs.Reports.NRA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class MatrixNRATestsDto
    {
        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo Posición solo puede tener 5 caracteres")]
        public string Position { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(200, ErrorMessage = "El campo Tipo de información solo puede tener 200 caracteres")]
        public string TypeInformation { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo Altura solo puede tener 5 caracteres")]
        public string Height { get; set; }

        [Range(0, 9, ErrorMessage = "El campo Sección debe ser numérico mayor a cero considerando hasta 1 entero sin decimales")]
        public int Section { get; set; }

        [Range(0.000001, 99999.999999, ErrorMessage = "El campo DBA debe ser numérico mayor a cero considerando hasta 5 enteros y 6 decimales")]
        public decimal Dba { get; set; }
        public decimal? DbaCor { get; set; }  /*--Solo aplica para la prueba nivel de ruido */


        [Range(0, 99999.999999, ErrorMessage = "El campo Decibel 315 debe ser numérico mayor a cero considerando hasta 5 enteros y 6 decimales")]
        public decimal Decibels315 { get; set; }
        [Range(0, 99999.999999, ErrorMessage = "El campo Decibel 63 debe ser numérico mayor a cero considerando hasta 5 enteros y 6 decimales")]
        public decimal Decibels63 { get; set; }
        [Range(0, 99999.999999, ErrorMessage = "El campo Decibel 125 debe ser numérico mayor a cero considerando hasta 5 enteros y 6 decimales")]
        public decimal Decibels125 { get; set; }
        [Range(0, 99999.999999, ErrorMessage = "El campo Decibel 250 debe ser numérico mayor a cero considerando hasta 5 enteros y 6 decimales")]
        public decimal Decibels250 { get; set; }
        [Range(0, 99999.999999, ErrorMessage = "El campo Decibel 500 debe ser numérico mayor a cero considerando hasta 5 enteros y 6 decimales")]
        public decimal Decibels500 { get; set; }
        [Range(0, 99999.999999, ErrorMessage = "El campo Decibel 1000 debe ser numérico mayor a cero considerando hasta 5 enteros y 6 decimales")]
        public decimal Decibels1000 { get; set; }
        [Range(0, 99999.999999, ErrorMessage = "El campo Decibel 2000 debe ser numérico mayor a cero considerando hasta 5 enteros y 6 decimales")]
        public decimal Decibels2000 { get; set; }
        [Range(0, 99999.999999, ErrorMessage = "El campo Decibel 4000 debe ser numérico mayor a cero considerando hasta 5 enteros y 6 decimales")]
        public decimal Decibels4000 { get; set; }
        [Range(0, 99999.999999, ErrorMessage = "El campo Decibel 8000 debe ser numérico mayor a cero considerando hasta 5 enteros y 6 decimales")]
        public decimal Decibels8000 { get; set; }
        [Range(0, 99999.999999, ErrorMessage = "El campo Decibel 10000 debe ser numérico mayor a cero considerando hasta 5 enteros y 6 decimales")]
        public decimal Decibels10000 { get; set; }

    }
}
