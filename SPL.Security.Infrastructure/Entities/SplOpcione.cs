using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Security.Infrastructure.Entities
{
    public partial class SplOpcione
    {
        public decimal Clave { get; set; }
        public string Descripcion { get; set; }
        public double Orden { get; set; }
        public decimal? ClavePadre { get; set; }
        public string SubMenu { get; set; }
        public string Url { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
