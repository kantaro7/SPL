namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class TapViewModel
    {
        public TapViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
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
        [DisplayName("Tipo de Unidad")]
        [Required(ErrorMessage = "Requerido")]
        public string UnitType { get; set; }

        [DisplayName("No. de Conexiones AT")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 99, ErrorMessage = "No. de Conexiones AT debe ser numérico mayor a cero considerando 2 enteros")]
        public int ATConnectionsAmount { get; set; }
        [DisplayName("No. de Conexiones BT")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 99, ErrorMessage = "No. de Conexiones BT debe ser numérico mayor a cero considerando 2 enteros")]
        public int BTConnectionsAmount { get; set; }
        [DisplayName("No. de Conexiones Ter")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 99, ErrorMessage = "No. de Conexiones Ter debe ser numérico mayor a cero considerando 2 enteros")]
        public int TerConnectionsAmount { get; set; }
        [DisplayName("Id Capacitancia AT")]
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(20, ErrorMessage = "Límite máximo de Caracteres (20).")]
        public string ATCapacitanceId { get; set; }
        [DisplayName("Id Capacitancia BT")]
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(20, ErrorMessage = "Límite máximo de Caracteres (20).")]
        public string BTCapacitanceId { get; set; }
        [DisplayName("Id Capacitancia Ter")]
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(20, ErrorMessage = "Límite máximo de Caracteres (20).")]
        public string TerCapacitanceId { get; set; }
        public List<TreeViewItemDTO> TreeViewItem { get; set; }
        public Workbook Workbook { get; set; }
        public SettingsToDisplayTAPReportsDTO TAPReportsDTO { get; set; }
        public bool IsReportAproved { get; set; }
        public string Base64PDF { get; set; }
        public TAPTestsDTO TapTestDTO { get; set; }
        public string Error { get; set; }
        public int AcceptanceValue { get; set; }
        public long FPCId { get; set; }

        [DisplayName("Capacitancia Ter")]
        [Range(0, 999999, ErrorMessage = "La Capacitancia Ter debe ser numérico mayor a cero considerando 6 enteros y 0 decimales")]
        [RegularExpression(@"^[1-9][0-9]{0,5}$", ErrorMessage = "La Capacitancia Ter debe ser numérico mayor a cero considerando 6 enteros y 0 decimales")]
        public string CapacitanciaTer { get; set; }
        public string TipoDevanadoNuevo { get; set; }
        public bool EnableCapTer { get; set; }


        public int NuevaFila { get; set; }
        public int ViejaFila { get; set; }
        public string NuevoDevanado { get; set; }
        public string ViejoDevanado { get; set; }
    }
}
