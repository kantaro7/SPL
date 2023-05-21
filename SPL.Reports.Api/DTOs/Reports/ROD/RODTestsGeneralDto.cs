﻿namespace SPL.Reports.Api.DTOs.Reports.ROD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RODTestsGeneralDto
    {
        public long IdLoad { get; set; }
        public DateTime LoadDate { get; set; }

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

        [DataType(DataType.Text)]
        [MaxLength(3, ErrorMessage = "El campo Tipo de Unidad solo puede tener {0} caracteres")]
        public string UnitType { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Material Devanado solo puede tener {0} caracteres")]
        public string WindingMaterial { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Unidad de Medida solo puede tener {0} caracteres")]
        public string UnitOfMeasurement { get; set; }
        public string TestConnection { get; set; }

        [Range(0, 999.999, ErrorMessage = "El campo Tensión Prueba debe ser numérico mayor o igual a cero considerando hasta 3 enteros y 3 decimales")]
        public decimal TestVoltage { get; set; }


        [DataType(DataType.Text)]
        [MaxLength(300, ErrorMessage = "El campo Comentario solo puede tener {0} caracteres")]
        public string Comment { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
        public string AutorizoCambio { get; set; }
        public string ConexionPrueba { get; set; }
        public List<RODTestsDto> Data { get; set; }
    }
}
