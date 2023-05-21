namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class PeeViewModel
    {
        public PeeViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
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
        public List<TreeViewItemDTO> TreeViewItem { get; set; }
        public Workbook Workbook { get; set; }
        public SettingsToDisplayPEEReportsDTO PEEReportsDTO { get; set; }
        public bool IsReportAproved { get; set; }
        public string Base64PDF { get; set; }
        public PEETestsDTO PeeTestDTO { get; set; }
        public string Error { get; set; }
    }
}
