namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class PceViewModel
    {
        public PceViewModel()
        {
            this.TreeViewItem = new List<TreeViewItemDTO>();
            this.VNStart = 90;
            this.VNEnd = 110;
            this.VNInterval = 10;
            this.Grafic = false;
        }

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

        [DisplayName("Posición de Prueba AT")]
        public string TestPositionAT { get; set; }
        [DisplayName("Posición de Prueba BT")]
        public string TestPositionBT { get; set; }
        [DisplayName("Posición de Prueba Ter")]
        public string TestPositionTer { get; set; }

        public bool AT { get; set; }
        public bool BT { get; set; }
        public bool Ter { get; set; }

        public SelectList TestPositionsAT { get; set; }
        public SelectList TestPositionsBT { get; set; }
        public SelectList TestPositionsTer { get; set; }
        public SelectList WindingsEnergized { get; set; }

        [DisplayName("Devanado Energizado")]
        [Required(ErrorMessage = "Requerido")]
        public string WindingEnergized  { get; set; }
        public string LessWindingEnergized { get; set; }

        [DisplayName("%VN Inicio")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 999, ErrorMessage = "El %VN de Inicio debe ser numérico mayor a cero considerando 3 enteros")]
        public int VNStart { get; set; }
        [DisplayName("%VN Fin")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 999, ErrorMessage = "El %VN de Fin debe ser numérico mayor a cero considerando 3 enteros")]
        public int VNEnd { get; set; }
        [DisplayName("%VN Intervalo")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0, 999, ErrorMessage = "El %VN de Intervalo debe ser numérico mayor a cero considerando 3 enteros")]
        public int VNInterval { get; set; }

        [DisplayName("Incluir Gráfica")]
        public bool Grafic { get; set; }

        public string Base64Graphic { get; set; }

        public List<TreeViewItemDTO> TreeViewItem { get; set; }

        public Workbook Workbook { get; set; }

        public Workbook OfficialWorkbook { get; set; }

        public SettingsToDisplayPCEReportsDTO PCEReportsDTO { get; set; }

        public bool IsReportAproved { get; set; }

        public string Base64PDF { get; set; }

        public List<PCETestsDTO> PceTestDTOs { get; set; }

        public string Error { get; set; }
    }
}
