using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoDetalleNra
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public string TipoInfo { get; set; }
        public string Pos { get; set; }
        public string Altura { get; set; }
        public decimal DbaMedido { get; set; }
        public decimal? D315 { get; set; }
        public decimal? D63 { get; set; }
        public decimal? D125 { get; set; }
        public decimal? D250 { get; set; }
        public decimal? D500 { get; set; }
        public decimal? D1000 { get; set; }
        public decimal? D2000 { get; set; }
        public decimal? D4000 { get; set; }
        public decimal? D8000 { get; set; }
        public decimal? D10000 { get; set; }
        public decimal? DbaCorr { get; set; }
    }
}
