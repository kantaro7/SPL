namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class BaseTemplateConsolidatedReportViewModel
    {


        [DisplayName("Idioma")]
        [Required(ErrorMessage = "Requerido")]
        public string Language { get; set; }

        [DisplayName("Plantilla Base")]
        [Required(ErrorMessage = "Requerido")]
        public IFormFile File { get; set; }

    }
}
