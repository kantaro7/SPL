using System;
using System.ComponentModel.DataAnnotations;

namespace SPL.Reports.Api.DTOs.Reports.TDP
{
    public class TDPTestsGeneralDto
    {
        public long IdLoad { get; set; }
        public DateTime LoadDate { get; set; }
        public DateTime Date { get; set; }

        //[Range(0, 99, ErrorMessage = "El campo Número de Prueba debe ser numérico mayor a cero considerando hasta 2 enteros")]
        public int TestNumber { get; set; }

        //[DataType(DataType.Text)]
        //[MaxLength(2, ErrorMessage = "El campo Clave Idioma solo puede tener {0} caracteres")]
        public string LanguageKey { get; set; }

        //[DataType(DataType.Text)]
        //[MaxLength(512, ErrorMessage = "El campo Cliente solo puede tener {0} caracteres")]
        public string Customer { get; set; }

        //[DataType(DataType.Text)]
        //[MaxLength(512, ErrorMessage = "El campo Capacidad solo puede tener {0} caracteres")]
        public string Capacity { get; set; }

        //[DataType(DataType.Text)]
        //[MaxLength(55, ErrorMessage = "El campo Número de Serie solo puede tener {0} caracteres")]
        public string SerialNumber { get; set; }
        public bool Result { get; set; }

        //[DataType(DataType.Text)]
        //[MaxLength(512, ErrorMessage = "El campo Capacidad solo puede tener {0} caracteres")]
        public string NameFile { get; set; }
        public byte[] File { get; set; }

        //[DataType(DataType.Text)]
        //[MaxLength(3, ErrorMessage = "El campo Tipo Reporte solo puede tener {0} caracteres")]
        public string TypeReport { get; set; }

        //[DataType(DataType.Text)]
        //[MaxLength(3, ErrorMessage = "El campo Clave Prueba solo puede tener {0} caracteres")]
        public string KeyTest { get; set; }

        //[DataType(DataType.Text)]
        //[MaxLength(3, ErrorMessage = "El campo Tipo de Unidad solo puede tener {0} caracteres")]
        public string Comment { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
   
     
        
        
       
        public int Tolerance { get; set; }


        public int NoCycles { get; set; }
        public int TotalTime { get; set; }
        public int Interval { get; set; }
        public decimal TimeLevel { get; set; }
        public decimal OutputLevel { get; set; }
        public int DescMayPc { get; set; }
        public int DescMayMv { get; set; }
        public int IncMaxPc { get; set; }
        public string VoltageLevels { get; set; }
        public string TerminalsTest { get; set; }
        public string MeasurementType { get; set; }

  
        public int Frequency { get; set; }
        public string Pos_At { get; set; }
        public string Pos_Bt { get; set; }
        public string Pos_Ter { get; set; }
        


        public TDPTestsDto TDPTest { get; set; }

    }
}
