namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Validations;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class RddViewModel
    {
        public RddViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();

        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }

        [DisplayName("No Prueba")]
        public decimal NoPrueba { get; set; }

        [DisplayName("Idioma")]
        [Required(ErrorMessage = "Requerido")]
        public string ClaveIdioma { get; set; }

        [DisplayName("Prueba")]
        [Required(ErrorMessage = "Requerido")]
        public string ClavePrueba { get; set; }

        [DisplayName("Comentarios")]
        [MaxLength(300, ErrorMessage = "Límite máximo de Caracteres.")]
        public string Comments { get; set; }

        [DisplayName("Config. Devanados")]
        [Required(ErrorMessage = "Requerido")]
        public string WindingConfiguration { get; set; }

        [DisplayName("Conexión")]
        [Required(ErrorMessage = "Requerido")]
        public string Connection { get; set; }

        [DisplayName("Pérdidas de Cobre %Z")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 999, ErrorMessage = "Horas de temperatura debe ser numérico mayor a cero considerando 3 enteros")]
        public decimal ZPorc { get; set; }

        [DisplayName("Pérdidas de Cobre %jX")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 999, ErrorMessage = "Horas de temperatura debe ser numérico mayor a cero considerando 3 enteros")]
        public decimal JXPorc { get; set; }

        public List<TreeViewItemDTO> TreeViewItem { get; set; }

        public int ValorAcep { get; set; }
        public Workbook Workbook { get; set; }

        public SettingsToDisplayRDDReportsDTO RDDReportsDTO { get; set; }

        public bool IsReportAproved { get; set; }

        public string Base64PDF { get; set; }

        public RDDTestsGeneralDTO RDDTestsGeneralDTO { get; set; }
        public string Error { get; set; }
    }
}
