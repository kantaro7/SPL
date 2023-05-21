using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Tests.Infrastructure.Entities
{
    public partial class SplInfoDetalleFpc
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public string DevE { get; set; }
        public string DevT { get; set; }
        public string DevG { get; set; }
        public string DevUst { get; set; }
        public string IdCap { get; set; }
        public decimal Corriente { get; set; }
        public decimal Potencia { get; set; }
        public decimal PorcFp { get; set; }
        public decimal Capacitancia { get; set; }
        public decimal CorrPorcFp { get; set; }
        public decimal TangPorcFp { get; set; }
    }
}
