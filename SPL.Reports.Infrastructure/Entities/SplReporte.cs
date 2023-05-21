using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class SplReporte
    {
        public string TipoReporte { get; set; }
        public string Descripcion { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
        public string DescripcionEn { get; set; }
        public string Agrupacion { get; set; }
        public string AgrupacionEn { get; set; }
    }
}
