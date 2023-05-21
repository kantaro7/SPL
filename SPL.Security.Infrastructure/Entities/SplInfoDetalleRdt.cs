using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Security.Infrastructure.Entities
{
    public partial class SplInfoDetalleRdt
    {
        public decimal IdCarga { get; set; }
        public string Posicion { get; set; }
        public decimal PosicionColumna { get; set; }
        public decimal? ValorColumna { get; set; }
        public decimal? ValorDesv { get; set; }
        public decimal? Hvvolts { get; set; }
        public decimal? Lvvolts { get; set; }
        public decimal? ValorNominal { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
        public decimal? OrdenPosicion { get; set; }
    }
}
