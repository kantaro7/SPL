namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Telerik.Web.Spreadsheet;

    public class RodViewModel
    {
        public RodViewModel()
        {
            this.TreeViewItem = new List<TreeViewItemDTO>();
            PosicionesAt = Array.Empty<string>();
            PosicionesBt = Array.Empty<string>();
            PosicionesTer = Array.Empty<string>();
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

        [DisplayName("Tipo de Unidad")]
        [Required(ErrorMessage = "Requerido")]
        public string UnitType { get; set; }
        public string UnitTypeName { get; set; }

        [DisplayName("Conexión de Prueba")]
        [Required(ErrorMessage = "Requerido")]
        public string Connection { get; set; }

        [DisplayName("Material del Devanado")]
        [Required(ErrorMessage = "Requerido")]
        public string Material { get; set; }

        [DisplayName("Unidad de Medida")]
        [Required(ErrorMessage = "Requerido")]
        public string UnitOfMeasurement { get; set; }

        [DisplayName("Tensión de la Conexión (Serie/Paralelo)")]
        [Range(0, 999.999, ErrorMessage = "La Tensión de la Conexión debe ser numérico mayor a cero considerando 3 enteros y 3 decimales")]
        [RegularExpression(@"^[0-9]{1,3}$|^[0-9]{1,3}\.[0-9]{1,3}$", ErrorMessage = "La Tensión de Prueba debe ser numérico mayor a cero considerando 3 enteros con 3 decimales")]
        public string TestVoltage { get; set; }

        public SelectList Connections { get; set; }

        public SelectList TypeTests { get; set; }

        public List<TreeViewItemDTO> TreeViewItem { get; set; }

        public Workbook Workbook { get; set; }

        public Workbook OfficialWorkbook { get; set; }

        public decimal AcceptanceValueFas { get; set; }
        public decimal AcceptanceValueMaDi { get; set; }
        public decimal AcceptanceValueMiDi { get; set; }

        public SettingsToDisplayRODReportsDTO RODReportsDTO { get; set; }

        public bool IsReportAproved { get; set; }

        public string Base64PDF { get; set; }

        public List<RODTestsDTO> RodTestDTOs { get; set; }

        public string Error { get; set; }

        public string Select1 { get; set; }

        public string Select2 { get; set; }

        public string Select3 { get; set; }

        public PositionsDTO Positions { get; set; }

        public string[] PosicionesAt { get; set; }

        public string[] PosicionesBt { get; set; }

        public string[] PosicionesTer { get; set; }

        public int CantidadPosicionesAt => PosicionesAt != null ? PosicionesAt.Length : 0;

        public int CantidadPosicionesBt => PosicionesTer != null ? PosicionesAt.Length : 0;

        public int CantidadPosicionesTer => PosicionesTer != null ? PosicionesAt.Length : 0;

        public bool IsAutrans { get; set; }
        public bool HasAT { get; set; }
        public bool HasBT { get; set; }
        public bool HasTer { get; set; }
        public string AutorizoCambio  { get; set; }
        public bool ClaveAutoriza { get; set; }
        public List<string> Celdas { get; set; }
        public string PosAT { get; set; }
        public string PosBT { get; set; }
        public string PosTER { get; set; }
        public int NumeroColumnas { get; set; }
        public List<ResistDesignDTO> Resistencias20Grados { get; set; }
    }
}