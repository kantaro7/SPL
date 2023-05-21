namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class BaseTemplateViewModel
    {

        [Required(ErrorMessage = "Requerido")]
        [DisplayName("Tipo de Reporte")]
        public string ReportId { get; set; }

        [DisplayName("Idioma")]
        [Required(ErrorMessage = "Requerido")]
        public string Language { get; set; }

        [DisplayName("Prueba")]
        [Required(ErrorMessage = "Requerido")]
        public string TestId { get; set; }

        [DisplayName("Plantilla Base")]
        [Required(ErrorMessage = "Requerido")]
        public IFormFile File { get; set; }

        [DisplayName("No. Columnas")]
        [Required(ErrorMessage = "Requerido")]
        [Range(1,99, ErrorMessage ="El valor debe ser mayor a 0 y menor a 100")]
        public int NoColumn { get; set; }

        public IList<IFormFile> File2 { get; set; }

        public IFormFile[] File3 { get; set; }

        public decimal[][] data { get; set; }
        public decimal MaxX { get; set; }
        public decimal MinX { get; set; }    
        public decimal MaxY { get; set; }
        public decimal MinY { get; set; }

    }
}
