namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Validations;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class FpcViewModel
    {
        public FpcViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();

        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }

        [DisplayName("No Prueba")]
        public decimal NoPrueba { get; set; }
        public decimal CantMediciones { get; set; }

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

        [DisplayName("Especificación")]
        [Required(ErrorMessage = "Requerido")]
        public string Specification { get; set; }

        [DisplayName("Frecuencia")]
        [Required(ErrorMessage = "Requerido")]
        public decimal Frequency { get; set; }

        public SelectList VoltageLevels { get; set; }

        public List<TreeViewItemDTO> TreeViewItem { get; set; }

        public Workbook Workbook { get; set; }

        public Workbook OfficialWorkbook { get; set; }

        public decimal AcceptanceValueFP { get; set; }

        public decimal AcceptanceValueCap { get; set; }

        public SettingsToDisplayFPCReportsDTO FPCReportsDTO { get; set; }

        public bool IsReportAproved { get; set; }

        public string Base64PDF { get; set; }

        public List<FPCTestsDTO> FpcTestDTOs { get; set; }
        public string Error { get; set; }
        public List<ValidateCapacitanceDTO> ValidateCapacitances1 { get; set; }
        public List<ValidateCapacitanceDTO> ValidateCapacitances2 { get; set; }
    }
}
