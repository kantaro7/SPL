namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Validations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class IndViewModel
    {
        public IndViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
        public PositionsDTO Positions { get; set; }
        public List<CharacteristicsArtifactDTO> CharacteristicsArtifact { get; set; }
        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }
        [DisplayName("No Prueba")]
        public string NoPrueba { get; set; }
        public decimal CantMediciones { get; set; }
        [DisplayName("Idioma")]
        [Required(ErrorMessage = "Requerido")]
        public string ClaveIdioma { get; set; }
        [DisplayName("Prueba")]
        [Required(ErrorMessage = "Requerido")]
        public string Pruebas { get; set; }


        [DisplayName("Comentarios")]
        [MaxLength(300, ErrorMessage = "Límite máximo de Caracteres.")]
        public string Comments { get; set; }

        public string Date { get; set; }

        public List<TreeViewItemDTO> TreeViewItem { get; set; }
        public decimal AcceptanceValueFP { get; set; }
        public decimal AcceptanceValueCap { get; set; }
        public decimal TempFP { get; set; }
        public decimal TempTandD { get; set; }
        public Workbook Workbook { get; set; }
        public Workbook OfficialWorkbook { get; set; }
        public bool IsReportAproved { get; set; }
        public string Base64PDF { get; set; }
        public string FechaSec1 { get; set; }
        public string FechaSec2 { get; set; }
        public string Error { get; set; }


        [DisplayName("Lleva TC's")]
        //[Required(ErrorMessage = "Requerido")]
        public string LlevaTC { get; set; }
        public List<ArchivosFrontViewModel> Archivos { get; set; }
        public SettingsToDisplayINDReportsDTO SettingsIND { get; set; }


    }
}
