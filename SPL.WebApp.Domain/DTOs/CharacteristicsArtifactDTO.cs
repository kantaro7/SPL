
namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class CharacteristicsArtifactDTO
    {
        public string OrderCode { get; set; }
        public decimal Secuencia { get; set; }
        public string CoolingType { get; set; }
        public decimal? OverElevation { get; set; }
        public decimal? Hstr { get; set; }
        public decimal? DevAwr { get; set; }
        public decimal? Mvaf1 { get; set; }
        public decimal? Mvaf2 { get; set; }
        public decimal? Mvaf3 { get; set; }
        public decimal? Mvaf4 { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
