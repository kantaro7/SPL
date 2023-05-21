using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Security.Infrastructure.Entities
{
    public partial class SplTituloColumnasCem
    {
        public decimal Tipo { get; set; }
        public string ClaveIdioma { get; set; }
        public string PosSec { get; set; }
        public string TituloPos { get; set; }
        public string Columna1 { get; set; }
        public string Columna2 { get; set; }
        public string Columna3 { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
