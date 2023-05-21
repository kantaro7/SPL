using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports
{
    public class WarrantiesArtifactDto
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
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
