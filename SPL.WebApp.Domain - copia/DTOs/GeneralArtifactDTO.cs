namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class GeneralArtifactDTO
    {
        #region Properties
        public string OrderCode { get; set; }
        public string Descripcion { get; set; }
        public decimal? Phases { get; set; }
        public string CustomerName { get; set; }
        public decimal? Frecuency { get; set; }
        public string PoNumeric { get; set; }
        public decimal? AltitudeF1 { get; set; }
        public string AltitudeF2 { get; set; }
        public decimal? TypeTrafoId { get; set; }
        public string TipoUnidad { get; set; }
        public decimal? Applicationid { get; set; }
        public decimal? StandardId { get; set; }
        public string Norma { get; set; }
        public decimal? LanguageId { get; set; }
        public string ClaveIdioma { get; set; }
        public decimal? PolarityId { get; set; }
        public string PolarityOther { get; set; }
        public string OilType { get; set; }
        public string OilBrand { get; set; }
        public string DesplazamientoAngular { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }

        #endregion
    }
}
