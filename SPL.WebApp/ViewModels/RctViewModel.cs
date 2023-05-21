namespace SPL.WebApp.ViewModels
{
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class RctViewModel
    {
        public RctViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
        [DisplayName("No Serie")]
        public string NoSerie { get; set; }
        [DisplayName("No Prueba")]
        public decimal NoPrueba { get; set; }
        [DisplayName("Idioma")]
        [Required(ErrorMessage = "Requerido")]
        public string ClaveIdioma { get; set; }
        [DisplayName("Prueba")]
        [Required(ErrorMessage = "Requerido")]
        public string ClavePrueba { get; set; }
        [DisplayName("Terciario o 2da Baja")]
 
        public string ThirdSecondDown { get; set; }
        [DisplayName("Comentarios")]
        [MaxLength(300, ErrorMessage = "Límite máximo de Caracteres.")]
        public string Comments { get; set; }
        [DisplayName("Unidad de Medida")]
        [Required(ErrorMessage = "Requerido")]
        public string UnitMeasuring { get; set; }
        [DisplayName("Tensión de Prueba")]
        public string TensionTest { get; set; }
        public List<string> AT { get; set; }
        public List<string> ABT { get; set; }
        public List<string> ABTER { get; set; }
        public bool ATR { get; set; }
        public bool BTR { get; set; }
        public bool TERR { get; set; }
        public decimal AcceptanceValue { get; set; }
        public int NumberColumns { get; set; }
        public bool TensionTestRequired { get; set; }
        public bool ThirdSecondDownRequired { get; set; }
        public Workbook Workbook { get; set; }
        public SettingsToDisplayRCTReportsDTO RCTReportsDTO { get; set; }
        public bool IsReportAproved { get; set; }
        public string Base64PDF { get; set; }
        public List<TreeViewItemDTO> TreeViewItem { get; set; }
        public List<RCTTestsDTO> RctTestDTOs { get; set; }
        public IEnumerable<GeneralPropertiesDTO> Tests { get; set; }
        public string Error { get; set; }
    }
}
