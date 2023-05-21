using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Security.Infrastructure.Entities
{
    public partial class SplInfoaparatoEst
    {
        public string NoSerie { get; set; }
        public decimal TorLimite { get; set; }
        public decimal TorHenf { get; set; }
        public decimal? AorLimite { get; set; }
        public decimal AorHenf { get; set; }
        public decimal? GradienteLimAt { get; set; }
        public decimal? GradienteHentAt { get; set; }
        public decimal? GradienteLimBt { get; set; }
        public decimal? GradienteHentBt { get; set; }
        public decimal? GradienteLimTer { get; set; }
        public decimal? GradienteHentTer { get; set; }
        public decimal? AwrLimAt { get; set; }
        public decimal? AwrHenfAt { get; set; }
        public decimal? AwrLimBt { get; set; }
        public decimal? AwrHenfBt { get; set; }
        public decimal? AwrLimTer { get; set; }
        public decimal? AwrHenfTer { get; set; }
        public decimal? HsrLimAt { get; set; }
        public decimal? HsrHenfAt { get; set; }
        public decimal? HsrLimBt { get; set; }
        public decimal? HsrHenfBt { get; set; }
        public decimal? HsrLimTer { get; set; }
        public decimal? HsrHenfTer { get; set; }
        public decimal CteTiempoTrafo { get; set; }
        public decimal AmbienteCte { get; set; }
        public decimal Bor { get; set; }
        public decimal KwDiseno { get; set; }
        public decimal Toi { get; set; }
        public decimal ToiLimite { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
