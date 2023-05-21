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

    public class TinViewModel
    {
        public TinViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
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
        [DisplayName("SE *C")]
        public string SE { get; set; }
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

        [DisplayName("Tension")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0.000001, 999999.99, ErrorMessage = "La tensión debe ser numérica mayor a cero considerando 6 enteros con 3 decimales")]
        public string Tension { get; set; }

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
        public SettingsToDisplayTINReportsDTO SettingsTIN { get; set; }
        public string FechaSec1 { get; set; }
        public string FechaSec2 { get; set; }
        public PCIInputTestDTO PCIOutDto { get; set; }
        public string[] ReigstrosPosicionesPrimarias { get; set; }
        public string[] ReigstrosPosicionesSecundarias{ get; set; }
        public string Error { get; set; }


        [DisplayName("Grados Correccion")]
        [Required(ErrorMessage = "Requerido")]
        public string Grados { get; set; }

        [DisplayName("Otro")]
        [Required(ErrorMessage = "Requerido")]
        public string Otro { get; set; }

        public VoltageKVDTO VoltageKV { get; set; }

        public List<TestsDTO> ListaPruebas { get; set; }

        public List<string> ListaDevanadoEnergizado { get; set; }

        [DisplayName("Devanado Energizado")]
        [Required(ErrorMessage = "Requerido")]
        public string DevanadoEnergizado { get; set; }

        [DisplayName("Cantidad Neutros")]
        [Required(ErrorMessage = "Requerido")]
        public string CantidadNeutros  { get; set; }

        public int CantidadConexionesEstrellas { get; set; }

        public int SelectedIndexDevanadoEnergizado { get; set; }

        [DisplayName("Impedancia Garantizada")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0.000001, 999999.99, ErrorMessage = "La reactancia lineal de diseño a tensión nominal debe ser numérica mayor a cero considerando 6 enteros con 3 decimales")]
        public string ImpedanciaGarantizada { get; set; }


        public string ATList { get; set; }
        public string BTList { get; set; }
        public string TerList { get; set; }

        public int Filas { get; set; }

        public OutISZTestsDTO ResultadoCalculateISZ { get; set; }

        [DisplayName("Incluye terciario")]
        [Required(ErrorMessage = "Requerido")]
        public string IncluyeTerciario { get; set; }


        [DisplayName("Conexion")]
        [Required(ErrorMessage = "Requerido")]
        public string Conexion { get; set; }
        public string Resultado { get; set; }
        public string Notas { get; set; }

        public CeldasValidate Celdas { get; set; }


     

    }
}
