using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Security.Infrastructure.Entities
{
    public partial class SplInfoDetalleFpb
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public string Posicion { get; set; }
        public string NoSerieBoq { get; set; }
        public string ColumnaT { get; set; }
        public decimal Corriente { get; set; }
        public decimal Potencia { get; set; }
        public decimal PorcFp { get; set; }
        public decimal PorcFpCorr { get; set; }
        public decimal Capacitancia { get; set; }
        public decimal IdMarca { get; set; }
        public decimal IdTipo { get; set; }
        public decimal FactorPotencia { get; set; }
        public decimal Capaci { get; set; }
        public decimal FactorCorr { get; set; }
        public decimal FactorCorr2 { get; set; }
    }
}
