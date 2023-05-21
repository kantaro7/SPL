using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class SplInfoDetallePim
    {
        public decimal IdRep { get; set; }
        public decimal Renglon { get; set; }
        public string Terminal { get; set; }
        public string Pagina { get; set; }
    }
}
