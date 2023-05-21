using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPL.Reports.Api.DTOs.Reports.CEM
{
   public class CEMTestsGeneralDto
    {
        public long IdLoad { get; set; }
        public DateTime LoadDate { get; set; }
        public DateTime Date { get; set; }

        [Range(0, 99, ErrorMessage = "El campo Número de Prueba debe ser numérico mayor a cero considerando hasta 2 enteros")]
        public int TestNumber { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(2, ErrorMessage = "El campo Clave Idioma solo puede tener {0} caracteres")]
        public string LanguageKey { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(512, ErrorMessage = "El campo Cliente solo puede tener {0} caracteres")]
        public string Customer { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(512, ErrorMessage = "El campo Capacidad solo puede tener {0} caracteres")]
        public string Capacity { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(55, ErrorMessage = "El campo Número de Serie solo puede tener {0} caracteres")]
        public string SerialNumber { get; set; }
        public bool Result { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(64, ErrorMessage = "El campo Nombre del Archivo solo puede tener {0} caracteres")]
        public string NameFile { get; set; }
        public byte[] File { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(3, ErrorMessage = "El campo Tipo Reporte solo puede tener {0} caracteres")]
        public string TypeReport { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(3, ErrorMessage = "El campo Clave Prueba solo puede tener {0} caracteres")]
        public string KeyTest { get; set; }



        public bool StatusAllPosSec { get; set; }

        //[DataType(DataType.Text)]
        //[MaxLength(3, ErrorMessage = "El campo Id de Posición Primaria solo puede tener {0} caracteres")]
        public string IdPosPrimary { get; set; }

        //[DataType(DataType.Text)]
        //[MaxLength(5, ErrorMessage = "El campo Posición Primaria solo puede tener {0} caracteres")]
        public string PosPrimary { get; set; }

        //[DataType(DataType.Text)]
        //[MaxLength(3, ErrorMessage = "El campo Id de Posición Secundaria solo puede tener {0} caracteres")]
        public string IdPosSecundary { get; set; }

        //[DataType(DataType.Text)]
        //[MaxLength(5, ErrorMessage = "El campo Posición Secundaria solo puede tener {0} caracteres")]
        public string PosSecundary { get; set; }

        [Range(0.01, 99999.99, ErrorMessage = "El campo Voltaje de Prueba debe ser numérico mayor a cero considerando hasta 5 enteros y 2 decimales")]
        public decimal TestsVoltage { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Título de la primera terminal solo puede tener 15 caracteres")]
        public string TitTerm1 { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Título de la segunda terminal solo puede tener 15 caracteres")]
        public string TitTerm2 { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Título de la tercera terminal solo puede tener 15 caracteres")]
        public string TitTerm3 { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(300, ErrorMessage = "El campo Comentario solo puede tener {0} caracteres")]
        public string Comment { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }


        public List<CEMTestsDetailsDto> CEMTestsDetails { get; set; }

    }
}
