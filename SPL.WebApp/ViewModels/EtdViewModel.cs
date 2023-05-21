namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ETD;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class EtdViewModel
    {
        public EtdViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }
        [DisplayName("Fecha Prueba")]
        public DateTime Date { get; set; }
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
        [DisplayName("Tipo de Regresión")]
        [Required(ErrorMessage = "Requerido")]
        public bool RegressionType { get; set; }
        [DisplayName("Sobrecarga")]
        [Required(ErrorMessage = "Requerido")]
        public string Overload { get; set; }
        [DisplayName("Baja Tensión Diferente Capacidad")]
        [Required(ErrorMessage = "Requerido")]
        public bool LVDifferentCapacity { get; set; }
        [DisplayName("Terciario con Capacidad Reducida")]
        [Required(ErrorMessage = "Requerido")]
        public bool TerReducedCapacity { get; set; }
        [DisplayName("Terciario o 2da. Baja")]
        [Required(ErrorMessage = "Requerido")]
        public string TerB2 { get; set; }
        [DisplayName("Devanado Split")]
        [Required(ErrorMessage = "Requerido")]
        public string SplitWinding { get; set; }
        [DisplayName("Conexion")]
        [RegularExpression(@"^[0-9]{1,6}$|^[0-9]{1,6}\.[0-9]{1,3}$", ErrorMessage = "La conexión debe ser numérica mayor a cero considerando 6 enteros con 3 decimales")]
        public decimal Connection { get; set; }
        [RegularExpression(@"^[0-9]{1,4}$|^[0-9]{1,4}\.[0-9]{1,3}$", ErrorMessage = "La capacidad de baja tensión debe ser numérica mayor a cero considerando 4 enteros con 3 decimales")]
        [DisplayName("Capacidad")]
        public decimal Capacity1 { get; set; }
        [RegularExpression(@"^[0-9]{1,4}$|^[0-9]{1,4}\.[0-9]{1,3}$", ErrorMessage = "“La capacidad del terciario debe ser numérica mayor a cero considerando 4 enteros con 3 decimales")]
        [DisplayName("Capacidad")]
        public decimal Capacity2 { get; set; }
        
        public List<TreeViewItemDTO> TreeViewItem { get; set; }
        public Workbook Workbook { get; set; }
        public SettingsToDisplayETDReportsDTO ETDReportsDTO { get; set; }
        public bool IsReportAproved { get; set; }
        public string Base64PDF { get; set; }
        public ETDTestsGeneralDTO EtdTestDTO { get; set; }
        public string Error { get; set; }

        #region DataFromCutting
        public decimal IdCuttingData { get; set; }
        public decimal CuttingDataCapacity { get; set; }
        public decimal CuttingDataLosses { get; set; }
        public decimal CuttingDataCurrent { get; set; }
        public string CuttingDataCoolingType { get; set; }
        public decimal CuttingDataSEC { get; set; }
        public string CuttingDataPosAT { get; set; }
        public string CuttingDataPosBT { get; set; }
        #endregion
        public List<List<Coord>> Coords { get; set; }
        public List<string> Base64Graphics { get; set; }
    }
    public class Coord
    {
        public decimal x { get; set; }
        public decimal y { get; set; }

        public Coord(decimal x, decimal y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
