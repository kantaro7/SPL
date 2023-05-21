namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class BaseTemplateConsolidatedReportDTO
    {
        public string ClaveIdioma { get; set; }
        public string Plantilla { get; set; }
        public string NombreArchivo { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
