﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.FPB
{
   public class FPBTestsGeneralDto
    {
        public long IdLoad { get; set; }
        public DateTime LoadDate { get; set; }

        [Range(1, 99, ErrorMessage = "El campo Número de Prueba debe ser numérico mayor a cero considerando hasta 2 enteros")]
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
        [MaxLength(64, ErrorMessage = "El campo Nombre Archivo solo puede tener {0} caracteres")]
        public string NameFile { get; set; }
        public byte[] File { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(3, ErrorMessage = "El campo Tipo Reporte solo puede tener {0} caracteres")]
        public string TypeReport { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(3, ErrorMessage = "El campo Clave Prueba solo puede tener {0} caracteres")]
        public string KeyTest { get; set; }

        public string TypeUnit { get; set; }

        [Range(1, 9999, ErrorMessage = "El campo Cantidad de Boquillas debe ser numérico mayor a cero considerando hasta 4 enteros")]
        public int CantBoq { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo Tan Delta solo puede tener {0} caracteres")]
        public string TanDelta { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(300, ErrorMessage = "El campo Comentario solo puede tener {0} caracteres")]
        public string Comment { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }

        public List<FPBTestsDto> Data { get; set; }
    }
}
