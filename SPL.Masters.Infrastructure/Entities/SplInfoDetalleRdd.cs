using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SplInfoDetalleRdd
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public string Fase { get; set; }
        public decimal Corriente { get; set; }
        public decimal Tension { get; set; }
        public decimal Perdidas { get; set; }
        public decimal PorcFp { get; set; }
        public decimal Resistencia { get; set; }
        public decimal Impedancia { get; set; }
        public decimal Reactancia { get; set; }
        public decimal? PorcX { get; set; }
    }
}
