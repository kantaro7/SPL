using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SPL.Tests.Infrastructure.Entities
{
    public partial class SplInfoaparatoDg
    {
        public string OrderCode { get; set; }
        public string Descripcion { get; set; }
        public decimal? Phases { get; set; }
        public string CustomerName { get; set; }
        public decimal? Frecuency { get; set; }
        public string PoNumeric { get; set; }
        public decimal? AltitudeF1 { get; set; }
        public string AltitudeF2 { get; set; }
        public decimal? Typetrafoid { get; set; }
        public string TipoUnidad { get; set; }
        public decimal? Applicationid { get; set; }
        public decimal? Standardid { get; set; }
        public string Norma { get; set; }
        public decimal? LanguageId { get; set; }
        public string ClaveIdioma { get; set; }
        public decimal? PolarityId { get; set; }
        public string PolarityOther { get; set; }
        public string TipoAceite { get; set; }
        public string MarcaAceite { get; set; }
        public string DesplazamientoAngular { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
