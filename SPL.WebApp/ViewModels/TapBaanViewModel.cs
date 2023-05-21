namespace SPL.WebApp.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TapBaanViewModel
    {
        public string OrderCode { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? ComboNumericSc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? CantidadSupSc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? PorcentajeSupSc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? CantidadInfSc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? PorcentajeInfSc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? NominalSc { get; set; }
  
        public decimal? IdentificacionSc { get; set; }
        public bool? InvertidoSc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? ComboNumericBc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? CantidadSupBc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? PorcentajeSupBc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? CantidadInfBc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? PorcentajeInfBc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? NominalBc { get; set; }
        public decimal? IdentificacionBc { get; set; }
        public bool? InvertidoBc { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
