using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Configuration.Api.DTOs.Configuration
{
    public class InformationOctavesDto
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El campo número de serie es requerido")]
        [MaxLength(55, ErrorMessage = "El campo Número de serie solo puede tener {0} caracteres")]
        public string NoSerie { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El campo Tipo de información es requerido")]
        [MaxLength(200, ErrorMessage = "El campo Tipo de información solo puede tener {0} caracteres")]
        public string TipoInfo { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El campo Altura es requerido")]
        [MaxLength(5, ErrorMessage = "El campo Altura solo puede tener {0} caracteres")]
        public string Altura { get; set; }



        [Required(ErrorMessage = "El campo Fecha datos es requerido")]
        public DateTime FechaDatos { get; set; }

        [Required(ErrorMessage = "El campo Hora es requerido")]
        public string Hora { get; set; }



        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 16 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D16 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 20 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D20 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 25 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D25 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 315 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D315 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 40 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D40 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 50 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D50 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 63 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D63 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 80 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D80 { get; set; }


        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 100 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D100 { get; set; }


        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 125 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D125 { get; set; }


        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 160 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D160 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 200 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D200 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 250 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D250 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 3151 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D3151 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 400 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D400 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 500 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D500 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 630 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D630 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 800 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D800 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 1000 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D1000 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 1250 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D1250 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 1600 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D1600 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 2000 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D2000 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 2500 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D2500 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 3150 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D3150 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 4000 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D4000 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 5000 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D5000 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 6300 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D6300 { get; set; }

        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 8000 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D8000 { get; set; }


        [Range(0.0000000001, 999999.9999999999, ErrorMessage = "El decibel 10000 debe ser numérico mayor a cero considerando hasta 6 enteros y 10 decimales")]
        public decimal D10000 { get; set; }


        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El campo Creado por es requerido")]
        [MaxLength(100, ErrorMessage = "El campo Creado por solo puede tener 100 caracteres")]
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
