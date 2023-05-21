namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class PimViewModel
    {
        public PimViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
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
        [DisplayName("Conexión")]
        [Required(ErrorMessage = "Requerido")]
        public string Connections { get; set; }
        [DisplayName("Aplica Baja")]
        [Required(ErrorMessage = "Requerido")]
        public string Low { get; set; }

        [DisplayName("Archivos Evidencia de las Pruebas")]
        [Required(ErrorMessage = "Requerido")]
        public List<ArchivosFrontViewModel> Files { get; set; }
        public List<string> ConnectionsList { get; set; }
        public List<string> ConnectionsSelected { get; set; }
        public string Connection { get; set; }
        public string VoltageLevel { get; set; }
        public List<string> VoltagesLevelAT { get; set; }
        public List<string> VoltagesLevelBT { get; set; }
        public List<string> VoltagesLevelAll { get; set; }

        public List<TreeViewItemDTO> TreeViewItem { get; set; }
        public Workbook Workbook { get; set; }
        public SettingsToDisplayPIMReportsDTO PIMReportsDTO { get; set; }
        public bool IsReportAproved { get; set; }
        public string Base64PDF { get; set; }
        public PIMTestsDTO PimTestDTO { get; set; }
        public string Error { get; set; }
    }
}
