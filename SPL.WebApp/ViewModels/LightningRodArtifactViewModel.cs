namespace SPL.WebApp.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class LightningRodArtifactViewModel
    {
        [Required(ErrorMessage = "OrderCodeLightningRod")]
        public string OrderCode { get; set; }
        public string ColumnTypeId { get; set; }
        public string ColumnTitle { get; set; }
        public string OrderIndex { get; set; }
        public string Qty { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
