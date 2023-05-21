using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SplDesplazamientoAngular
    {
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public decimal HWye { get; set; }
        public decimal XWye { get; set; }
        public decimal TWye { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
