using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class SplInfoGraficaEtd
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public decimal ValorX { get; set; }
        public decimal ValorY { get; set; }
        public decimal ValorZ { get; set; }
    }
}
