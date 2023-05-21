using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class SplTituloColumnasRdt
    {
        public string ClavePrueba { get; set; }
        public string DesplazamientoAngular { get; set; }
        public string Norma { get; set; }
        public decimal PosColumna { get; set; }
        public string ClaveIdioma { get; set; }
        public string Titulo { get; set; }
        public string PrimerRenglon { get; set; }
        public string SegundoRenglon { get; set; }
        public string TitPos1 { get; set; }
        public string TitPos2 { get; set; }
        public string TitPosUnica1 { get; set; }
        public string TitPosUnica2 { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
