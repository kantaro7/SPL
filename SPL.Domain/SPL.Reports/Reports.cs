using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports
{
    public class Reports
    {
        public string TipoReporte { get; set; }
        public string Descripcion { get; set; }
        public string Creadopor { get; set; }
        public string AgrupacionEn { get; set; }
        public string Agrupacion { get; set; }
        public string DescripcionEn { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
