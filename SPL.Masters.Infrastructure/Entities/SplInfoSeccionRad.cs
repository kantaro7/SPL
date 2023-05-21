using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SplInfoSeccionRad
    {
        public decimal IdCarga { get; set; }
        public decimal Seccion { get; set; }
        public decimal? Tension { get; set; }
        public string Umtension { get; set; }
        public decimal? Temperatura { get; set; }
        public string Umtemp { get; set; }
        public DateTime? FechaPrueba { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
