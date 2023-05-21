using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SplInfoDetalleCem
    {
        public decimal IdRep { get; set; }
        public decimal Renglon { get; set; }
        public string PosSecundaria { get; set; }
        public decimal CorrienteTerm1 { get; set; }
        public decimal CorrienteTerm2 { get; set; }
        public decimal CorrienteTerm3 { get; set; }
    }
}
