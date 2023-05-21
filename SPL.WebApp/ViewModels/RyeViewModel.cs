namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class RyeViewModel
    {
        public RyeViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
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
        [DisplayName("Tipo de Enfriamiento")]
        [Required(ErrorMessage = "Requerido")]
        public string CoolingType { get; set; }

        [DisplayName("No. de Conexiones AT")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 99, ErrorMessage = "No. de Conexiones AT debe ser numérico mayor a cero considerando 2 enteros")]
        public int ATConnectionsAmount { get; set; }
        [DisplayName("No. de Conexiones BT")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 99, ErrorMessage = "No. de Conexiones BT debe ser numérico mayor a cero considerando 2 enteros")]
        public int BTConnectionsAmount { get; set; }
        [DisplayName("No. de Conexiones Ter")]
        [Range(0, 99, ErrorMessage = "No. de Conexiones Ter debe ser numérico mayor a cero considerando 2 enteros")]
        public int TerConnectionsAmount { get; set; }
        [DisplayName("Tensión de Prueba AT")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 999, ErrorMessage = "LA Tensión de Prueba AT debe ser numérico mayor a cero considerando 3 enteros")]
        public decimal? ATTestVoltage { get; set; }
        [DisplayName("Tensión de Prueba BT")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 999, ErrorMessage = "LA Tensión de Prueba BT debe ser numérico mayor a cero considerando 3 enteros")]
        public decimal? BTTestVoltage { get; set; }
        [DisplayName("Tensión de Prueba Ter")]
        [Range(0, 999, ErrorMessage = "LA Tensión de Prueba Ter debe ser numérico mayor a cero considerando 3 enteros")]
        public decimal? TerTestVoltage { get; set; }
        public List<string> CoolingTypes { get; set; }
        public List<TreeViewItemDTO> TreeViewItem { get; set; }
        public Workbook Workbook { get; set; }
        public SettingsToDisplayRYEReportsDTO RYEReportsDTO { get; set; }
        public bool IsReportAproved { get; set; }
        public string Base64PDF { get; set; }
        public OutRYETestsDTO RyeTestDTO { get; set; }
        public string Error { get; set; }
    }
}
