namespace SPL.WebApp.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NozzlesArtifactViewModel
    {
        [Required(ErrorMessage = "OrderCodeNozzles")]
        public string OrderCode { get; set; }
        public string ColumnTypeId { get; set; }
        public string ColumnTitle { get; set; }
        public string OrderIndex { get; set; }
        public string Qty { get; set; }
        public string VoltageClass { get; set; }
        public string VoltageClassOther { get; set; }

        public string ResultBilUnidad { get; set; }
        public string BilClass { get; set; }
        public string BilClassOther { get; set; }
        public string CurrentAmps { get; set; }
        public string CurrentAmpsReq { get; set; }
        public string CorrienteUnidad { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
