namespace SPL.Reports.Api.DTOs.Reports.NRA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class NRATestsDto
    {
      

        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo PosAT solo puede tener {0} caracteres")]
        public string HV { get; set; }


        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo PosBT solo puede tener {0} caracteres")]
        public string LV { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo PosTER solo puede tener {0} caracteres")]
        public string TV { get; set; }


        public string Losses { get; set; }


        [Range(0.1, 999.9, ErrorMessage = "El campo Altura debe ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal Height { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Unidad de medida de Altura solo puede tener {0} caracteres")]
        public string UmHeight { get; set; }

        [Range(0.1, 999.9, ErrorMessage = "El campo Perimetro debe ser numérico mayor a cero considerando hasta 3 enteros y 1 decimal")]
        public decimal Length { get; set; }


        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Unidad de medida de Perimetro solo puede tener {0} caracteres")]
        public string UmLength { get; set; }


        [Range(0.01, 99999.99, ErrorMessage = "El campo Área debe ser numérico mayor a cero considerando hasta 5 enteros y 2 decimales")]
        public decimal Surface { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Unidad de medida de Superficie solo puede tener {0} caracteres")]
        public string UmSurface { get; set; }


        [Range(0.01, 99.99, ErrorMessage = "El campo K Factor debe ser numérico mayor a cero considerando hasta 2 enteros y 2 decimales")]
        public decimal KFactor { get; set; }


        //********Inicio**************Tomados de SPL_INFO_LABORATORIO[HttpGet("getInformationLaboratories")] SPL.Configuration.Api

        [Range(0.0001, 999999.9999, ErrorMessage = "El campo SV debe ser numérico mayor a cero considerando hasta 6 enteros y 4 decimales")]
        public decimal SV { get; set; }

        [Range(0.01, 999.99, ErrorMessage = "El campo Alfa debe ser numérico mayor a cero considerando hasta 3 enteros y 2 decimales")]
        public decimal Alfa { get; set; }
        //*********Fin*************Tomados de SPL_INFO_LABORATORIO[HttpGet("getInformationLaboratories")] SPL.Configuration.Api



        //********Inicio**************viene del obtener configuracion
    
        public decimal Diferencia { get; set; }
        public decimal FactorCoreccion { get; set; }
        //********Fin**************viene del obtener configuracion


        
        public decimal Guaranteed { get; set; }

        [Range(0, 99999, ErrorMessage = "El campo Nivel de presión de sonido promedio debe ser numérico mayor a cero considerando hasta 5 enteros sin decimales")]
        public decimal AvgSoundPressureLevel { get; set; }

        [Range(0, 99999, ErrorMessage = "El campo Nivel de potencia de sonido debe ser numérico mayor a cero considerando hasta 5 enteros sin decimales")]
        public decimal SoundPowerLevel { get; set; }



        public NRATestsDetailsOctDto NRATestsDetailsOcts { get; set; }
        public NRATestsDetailsRuiDto NRATestsDetailsRuis { get; set; }


    }
}
