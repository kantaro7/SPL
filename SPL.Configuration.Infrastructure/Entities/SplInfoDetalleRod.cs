using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoDetalleRod
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public string Posicion { get; set; }
        public decimal Terminal1 { get; set; }
        public decimal Terminal2 { get; set; }
        public decimal Terminal3 { get; set; }
        public decimal ResistenciaProm { get; set; }
        public decimal Correccion20 { get; set; }
        public decimal CorreccionSe { get; set; }
        public decimal PorcDesv { get; set; }
        public decimal ResDisenio { get; set; }
        public decimal PorcDesvDis { get; set; }
    }
}
