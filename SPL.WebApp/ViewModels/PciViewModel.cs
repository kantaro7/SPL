namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.Domain.SPL.Tests.PCI;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Validations;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class PciViewModel
    {
        public PciViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
        public PositionsDTO Positions { get; set; }
        public List<CharacteristicsArtifactDTO> CharacteristicsArtifact { get; set; }
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
        [DisplayName("Material del devanado")]
        [Required(ErrorMessage = "Requerido")]
        public string MaterialDevanado { get; set; }
        [DisplayName("Capacidad Reducida en Baja")]
        [Required(ErrorMessage = "Requerido")]
        public string CapacidadReducidaBaja { get; set; }
        [DisplayName("AutoTransformador")]
        [Required(ErrorMessage = "Requerido")]
        public string AutoTransformador { get; set; }
        [DisplayName("Monofásico")]
        [Required(ErrorMessage = "Requerido")]
        public string Monofasico { get; set; }
        [DisplayName("Capacidad de Prueba")]
        [Required(ErrorMessage = "Requerido")]
        public string CapacidadPrueba { get; set; }        
        public bool SeccionAT { get; set; }
        public bool SeccionBT { get; set; }
        public bool SeccionTer { get; set; }
        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Select3 { get; set; }
        public bool Primario { get; set; }
        public bool Secundario { get; set; }
        public int CantidadPosicionPrimaria { get; set; }
        public int CantidadPosicionSecundaria { get; set; }
        public string PosicionPrimaria { get; set; }
        public string PosicionSecundaria { get; set; }
        [DisplayName("Comentarios")]
        [MaxLength(300, ErrorMessage = "Límite máximo de Caracteres.")]
        public string Comments { get; set; }
        [DisplayName("Tipo de Unidad")]
        [Required(ErrorMessage = "Requerido")]
        public string UnitType { get; set; }
        public decimal Tension { get; set; }
        public decimal Temperature { get; set; }
        public string UmTension { get; set; }
        public string UmTemperature { get; set; }
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
        public SettingsToDisplayPCIReportsDTO SettingsPCI { get; set; }
        public string FechaSec1 { get; set; }
        public string FechaSec2 { get; set; }
        public string[] ReigstrosPosicionesPrimarias { get; set; }
        public string[] ReigstrosPosicionesSecundarias { get; set; }
        public string Error { get; set; }
        public bool EsMonofasico { get; set; }
        public bool EsAutotransformador { get; set; }

        public string ClavePruebaFind { get; set; }
        public string ConexionPruebaFind { get; set; }

        public PCITestResponseDTO Result { get; set; }
    }
}