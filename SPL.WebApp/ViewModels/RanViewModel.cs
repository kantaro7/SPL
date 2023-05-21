namespace SPL.WebApp.ViewModels
{
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Telerik.Web.Spreadsheet;

    public class RanViewModel
    {
        public RanViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();

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
        [MaxLength(300, ErrorMessage ="Límite máximo de Caracteres.")]
        public string Comments { get; set; }

        [DisplayName("Cant. de Mediciones")]
        [Required(ErrorMessage = "Favor de indicar la cantidad de mediciones.")]
        public int Measuring { get; set; }

        public List<TreeViewItemDTO> TreeViewItem { get; set; }

        public Workbook Workbook { get; set; }

        public Workbook OfficialWorkbook { get; set; }

        public decimal AcceptanceValue { get; set; }

        public SettingsToDisplayRANReportsDTO RANReportsDTO { get; set; }

        public bool IsReportAproved { get; set; }

        public string Base64PDF { get; set; }
        public RANTestsDetailsDTO RanTestDTO { get; set; }
        public string Error { get; set; }
    }
}
