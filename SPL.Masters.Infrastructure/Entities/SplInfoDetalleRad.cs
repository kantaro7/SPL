using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SplInfoDetalleRad
    {
        public decimal IdCarga { get; set; }
        public decimal Seccion { get; set; }
        public string Tiempo { get; set; }
        public decimal PosicionColumna { get; set; }
        public string ValorColumna { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
