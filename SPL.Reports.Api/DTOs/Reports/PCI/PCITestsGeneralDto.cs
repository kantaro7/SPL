namespace SPL.Reports.Api.DTOs.Reports.PCI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SPL.Domain.SPL.Reports.PCI;

    public class PCITestsGeneralDto
    {
        public long IdLoad { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(3, ErrorMessage = "El campo Tipo Reporte solo puede tener {0} caracteres")]
        public string TypeReport { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(3, ErrorMessage = "El campo Clave Prueba solo puede tener {0} caracteres")]
        public string KeyTest { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(2, ErrorMessage = "El campo Clave Idioma solo puede tener {0} caracteres")]
        public string LanguageKey { get; set; }

        public DateTime LoadDate { get; set; }
        
        [Range(0, 99, ErrorMessage = "El campo Número de Prueba debe ser numérico mayor a cero considerando hasta 2 enteros")]
        public int TestNumber { get; set; }
        
        public DateTime Date { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(512, ErrorMessage = "El campo Cliente solo puede tener {0} caracteres")]
        public string Customer { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(512, ErrorMessage = "El campo Capacidad solo puede tener {0} caracteres")]
        public string Capacity { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(55, ErrorMessage = "El campo Número de Serie solo puede tener {0} caracteres")]
        public string SerialNumber { get; set; }

        public bool ReducedCapacityAtLowVoltage { get; set; }

        public bool Autotransformer { get; set; }

        public bool Monofasico { get; set; }

        public string WindingMaterial { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(300, ErrorMessage = "El campo Comentario solo puede tener {0} caracteres")]
        public string Comment { get; set; }

        public decimal Kwcu { get; set; }

        public decimal KwcuMva { get; set; }

        public decimal Kwtot100 { get; set; }

        public decimal Kwtot100M { get; set; }

        public bool Result { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(64, ErrorMessage = "El campo Nombre del Archivo solo puede tener {0} caracteres")]
        public string NameFile { get; set; }
        
        public byte[] File { get; set; }

        public string Creadopor { get; set; }

        public DateTime Fechacreacion { get; set; }

        public string Modificadopor { get; set; }

        public DateTime? Fechamodificacion { get; set; }

        public List<PCITestRating> Ratings { get; set; } = new List<PCITestRating>();
    }
}