using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoDetalleArf
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public string PosAt { get; set; }
        public string PosBt { get; set; }
        public string PosTer { get; set; }
        public string NivelAceite { get; set; }
        public decimal? TempAceite { get; set; }
        public string Boquillas { get; set; }
        public string NucleoHerraje { get; set; }
        public string Terciario { get; set; }
    }
}
