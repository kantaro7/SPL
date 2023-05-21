using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.TIN
{
   public class TINTestsGeneralDto
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

        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Conexión solo puede tener {0} caracteres")]
        public string Connection { get; set; }

        //[Range(0.001, 999999.999, ErrorMessage = "El campo Tensión debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal? Voltage { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Relación de la Tensión solo puede tener {0} caracteres")]
        public string RelVoltage { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo Posición en la Alta Tensión solo puede tener {0} caracteres")]
        public string PosAT { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo Posición en la Baja Tensión solo puede tener {0} caracteres")]
        public string PosBT { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo Posición en Terciario solo puede tener {0} caracteres")]
        public string PosTER { get; set; }

        [Range(0.001, 999999.999, ErrorMessage = "El campo Frecuencia debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal Frecuency { get; set; }

        [Range(0, 999, ErrorMessage = "El campo Tiempo debe ser numérico mayor a cero considerando hasta 3 enteros")]
        public int Time { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(3, ErrorMessage = "El campo Devanado Energizado solo puede tener {0} caracteres")]
        public string EnergizedWinding { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(3, ErrorMessage = "El campo Devanado Inducido solo puede tener {0} caracteres")]
        public string InducedWinding { get; set; }

        [Range(0.001, 999999.999, ErrorMessage = "El campo Tensión Aplicada debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal AppliedVoltage { get; set; }

        [Range(0.001, 999999.999, ErrorMessage = "El campo Tensión Inducida debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal InducedVoltage { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(100, ErrorMessage = "El campo Notas solo puede tener {0} caracteres")]
        public string Grades { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(300, ErrorMessage = "El campo Comentario solo puede tener {0} caracteres")]
        public string Comment { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }

    }
}
