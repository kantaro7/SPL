using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SplInfoaparatoGar
    {
        public string OrderCode { get; set; }
        public decimal? Iexc100 { get; set; }
        public decimal? Iexc110 { get; set; }
        public decimal? Kwfe100 { get; set; }
        public decimal? Kwfe110 { get; set; }
        public decimal? TolerancyKwfe { get; set; }
        public decimal? KwcuMva { get; set; }
        public decimal? KwcuKv { get; set; }
        public decimal? Kwcu { get; set; }
        public decimal? TolerancyKwCu { get; set; }
        public decimal? Kwaux3 { get; set; }
        public decimal? Kwaux4 { get; set; }
        public decimal? Kwaux1 { get; set; }
        public decimal? Kwaux2 { get; set; }
        public decimal? TolerancyKwAux { get; set; }
        public decimal? Kwtot100 { get; set; }
        public decimal? Kwtot110 { get; set; }
        public decimal? TolerancyKwtot { get; set; }
        public decimal? ZPositiveMva { get; set; }
        public decimal? ZPositiveHx { get; set; }
        public decimal? ZPositiveHy { get; set; }
        public decimal? ZPositiveXy { get; set; }
        public decimal? TolerancyZpositive { get; set; }
        public decimal? TolerancyZpositive2 { get; set; }
        public decimal? NoiseOa { get; set; }
        public decimal? NoiseFa1 { get; set; }
        public decimal? NoiseFa2 { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
