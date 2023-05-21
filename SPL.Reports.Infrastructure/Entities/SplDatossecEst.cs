using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class SplDatossecEst
    {
        public decimal IdReg { get; set; }
        public decimal Seccion { get; set; }
        public decimal Capacidad { get; set; }
        public string CoolingType { get; set; }
        public decimal OverElevation { get; set; }
        public decimal FactEnf { get; set; }
        public decimal? Perdidas { get; set; }
        public decimal? Corriente { get; set; }
        public decimal PorcCarga { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
