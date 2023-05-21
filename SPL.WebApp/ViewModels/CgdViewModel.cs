namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class CgdViewModel
    {
        public CgdViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
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
        [DisplayName("Tipo de Aceite")]
        [Required(ErrorMessage = "Requerido")]
        public string OilType { get; set; }

        [DisplayName("Horas de Temperatura")]
        //[Range(0, 999, ErrorMessage = "Horas de temperatura debe ser numérico mayor a cero considerando 3 enteros")]
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^([0-9]{0,3})$", ErrorMessage = "Horas de temperatura debe ser numérico mayor a cero considerando 3 enteros")]
        public int TemperatureHour1 { get; set; }
        [DisplayName("Horas de Temperatura")]
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^([0-9]{0,3})$", ErrorMessage = "Horas de temperatura debe ser numérico mayor a cero considerando 3 enteros")]
        public int TemperatureHour2 { get; set; }
        [DisplayName("Horas de Temperatura")]
        [Required(ErrorMessage = "De no poseer valor debe ser 0")]
        [RegularExpression(@"^([0-9]{0,3})$", ErrorMessage = "Horas de temperatura debe ser numérico mayor a cero considerando 3 enteros")]
        public int TemperatureHour3 { get; set; }
        public List<ContGasCGDDTO> OilTypes { get; set; }

        public List<TreeViewItemDTO> TreeViewItem { get; set; }
        public Workbook Workbook { get; set; }
        public SettingsToDisplayCGDReportsDTO CGDReportsDTO { get; set; }
        public bool IsReportAproved { get; set; }
        public string Base64PDF { get; set; }
        public List<CGDTestsDTO> CgdTestsDTOs { get; set; }
        public string Error { get; set; }
        public int AcceptanceValue { get; set; }
        public long FPCId { get; set; }
    }
}
