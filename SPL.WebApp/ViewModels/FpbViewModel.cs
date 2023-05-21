namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Validations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class FpbViewModel
    {
        public FpbViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();

        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }

        [DisplayName("No Prueba")]
        public decimal NoPrueba { get; set; }
        public decimal CantMediciones { get; set; }

        [DisplayName("Idioma")]
        [Required(ErrorMessage = "Requerido")]
        public string ClaveIdioma { get; set; }

        [DisplayName("TanDelta")]
        [Required(ErrorMessage = "Requerido")]
        public string TanDelta { get; set; }

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

        [DisplayName("Nivel de Tensión")]
        [Required(ErrorMessage = "Requerido")]
        public string VoltageLevel { get; set; }

       
        public decimal Tension { get; set; }
        public decimal Temperature { get; set; }
        public string UmTension { get; set; }
        public string UmTemperature { get; set; }

        public DateTime Date { get; set; }
        public List<TreeViewItemDTO> TreeViewItem { get; set; }

        public decimal AcceptanceValueFP { get; set; }

        public decimal AcceptanceValueCap { get; set; }
        public decimal TempFP { get; set; }
        public decimal TempTandD { get; set; }

        public Workbook Workbook { get; set; }
        public Workbook OfficialWorkbook { get; set; }

        public List<FPBTestsDTO> FpbTestDTOs { get; set; }
        public bool IsReportAproved { get; set; }
        public string Base64PDF { get; set; }
        public SettingsToDisplayFPBReportsDTO FPBReportsDTO { get; set; }

        public string FechaSec1 { get; set; }
        public string FechaSec2 { get; set; }
        public string Error { get; set; }
        public List<ValidateCapacitanceDTO> ValidateCapacitancesFP { get; set; }
        public List<ValidateCapacitanceDTO> ValidateCapacitancesCAP { get; set; }

    }
}
