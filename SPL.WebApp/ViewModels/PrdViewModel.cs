namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class PrdViewModel
    {
        public PrdViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();

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

        [DisplayName("Voltage Nominal")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 999.999, ErrorMessage = "La Tensión de Prueba debe ser numérico mayor a cero y menor a 1000")]
        public decimal NominalVoltage { get; set; }

        public List<string> Celdas { get; set; }
        public List<string> Nombres { get; set; }

        public List<TreeViewItemDTO> TreeViewItem { get; set; }

        public Workbook Workbook { get; set; }

        public Workbook OfficialWorkbook { get; set; }

        public SettingsToDisplayPRDReportsDTO PRDReportsDTO { get; set; }

        public bool IsReportAproved { get; set; }

        public string Base64PDF { get; set; }

        public PRDTestsDTO PrdTestDTO { get; set; }

        public string Error { get; set; }
    }
}
