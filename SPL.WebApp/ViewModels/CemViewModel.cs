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

    public class CemViewModel
    {
        public CemViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
        public PositionsDTO Positions { get; set; }
   
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
     
       
        [DisplayName("Voltaje de prueba (KV)")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0.001, 999999.9, ErrorMessage = "El voltaje de prueba debe ser numérico mayor a cero considerando 6 enteros con 1 decimal")]
        [RegularExpression(@"^\d+(\.\d{1,1})?$", ErrorMessage = "El voltaje de prueba debe ser numérico mayor a cero considerando 6 enteros con 1 decimal")]
        public string VoltajePrueba { get; set; }


        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Select3 { get; set; }
        public bool Primario { get; set; }
        public bool Secundario { get; set; }
  
        public string PosicionPrimaria { get; set; }
        public string PosicionSecundaria { get; set; }

        [DisplayName("Comentarios")]
        [MaxLength(300, ErrorMessage = "Límite máximo de Caracteres.")]
        public string Comments { get; set; }
        [DisplayName("Tipo de Unidad")]
        public string Date { get; set; }
        public List<TreeViewItemDTO> TreeViewItem { get; set; }
    
    
        public Workbook Workbook { get; set; }
        public Workbook OfficialWorkbook { get; set; }
        public bool IsReportAproved { get; set; }
        public string Base64PDF { get; set; }
        public SettingsToDisplayCEMReportsDTO SettingsCEM { get; set; }
        public string FechaSec1 { get; set; }
        public string FechaSec2 { get; set; }
        public List<CEMTestsDetailsDTO> CEMOutDto { get; set; }
        public string[] ReigstrosPosicionesPrimarias { get; set; }
        public string[] ReigstrosPosicionesSecundarias { get; set; }
        public string Error { get; set; }

        public bool StatusAllPosSec { get; set; }

        public int CantidadPosicionesSecundarias { get; set; }
    }
}
