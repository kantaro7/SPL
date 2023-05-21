using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Artifact.Infrastructure.Entities
{
    public partial class SplInfoDetalleRct
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Columna { get; set; }
        public string Fase { get; set; }
        public string Terminal { get; set; }
        public string Posicion { get; set; }
        public decimal Resistencia { get; set; }
        public string Unidad { get; set; }
        public decimal Temperatura { get; set; }
        public string TipoMedicion { get; set; }
    }
}
