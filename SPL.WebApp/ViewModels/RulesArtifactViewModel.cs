namespace SPL.WebApp.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RulesArtifactViewModel
    {
        [Required(ErrorMessage = "OrderCodeRulesArtifact")]
        public string OrderCode { get; set; }
        public string Secuencia { get; set; }
        public string Descripcion { get; set; }
        public string ClaveNorma { get; set; }
        public string ClaveIdioma { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
