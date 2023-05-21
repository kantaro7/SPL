using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Tests.Infrastructure.Entities
{
    public partial class SplInfoDetalleInd
    {
        public decimal IdRep { get; set; }
        public decimal Renglon { get; set; }
        public string NoPagina { get; set; }
        public string NoPaginaIni { get; set; }
        public string NoPaginaFin { get; set; }
        public decimal? ValorMva { get; set; }
        public decimal? ValorKw { get; set; }
    }
}
