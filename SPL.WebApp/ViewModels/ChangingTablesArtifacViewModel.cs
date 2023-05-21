namespace SPL.WebApp.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ChangingTablesArtifacViewModel
    {
        [Required(ErrorMessage = "OrderCodeChanging")]
        public string OrderCode { get; set; }
        public string ColumnTypeId { get; set; }
        public string ColumnTitle { get; set; }
        public string OrderIndex { get; set; }
        public string OperationId { get; set; }
      
        public string FlagRcbnFcbn { get; set; }
        public string DerivId { get; set; }
        public string DerivOther { get; set; }
        public string DerivId2 { get; set; }
        public string Deriv2Other { get; set; }
        public string Taps { get; set; }
        public string Creadopor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
