namespace SPL.WebApp.ViewModels
{
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Validations;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class RdtViewModel
    {
        public RdtViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();

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

        [DisplayName("Desplazamiento Angular")]
        [Required(ErrorMessage = "Requerido")]
        public string AngularDisplacement { get; set; }
        public string AngularDisplacementValue { get; set; }

        [DisplayName("Norma")]
        [Required(ErrorMessage = "Requerido")]
        public string Norm { get; set; }

        [DisplayName("Conexión S/P")]
        public string Connection { get; set; }

        public IEnumerable<PlateTensionDTO> ATs { get; set; }

        public IEnumerable<PlateTensionDTO> BTs { get; set; }
        
        public IEnumerable<PlateTensionDTO> Ters { get; set; }

        public int ATCount { get; set; }

        public int BTCount { get; set; }

        public int TerCount { get; set; }

        [DisplayName("Posición AT")]
        public string ATPosition { get; set; }

        [DisplayName("Posición BT")]
        public string BTPosition { get; set; }

        [DisplayName("Posición TER")]
        public string TerPosition { get; set; }

        public IEnumerable<GeneralPropertiesDTO> Tests { get; set; }

        public List<TreeViewItemDTO> TreeViewItem { get; set; }

        public Workbook Workbook { get; set; }

        public Workbook OfficialWorkbook { get; set; }

        public decimal AcceptanceValue { get; set; }

        public SettingsToDisplayRDTReportsDTO RDTReportsDTO { get; set; }

        public bool IsReportAproved { get; set; }

        public string Base64PDF { get; set; }
        public RDTTestsDTO RdtTestDTO { get; set; }
        public string Error { get; set; }
    }
}
