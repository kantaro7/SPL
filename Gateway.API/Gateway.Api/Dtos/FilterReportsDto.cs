using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos
{
    public class FilterReportsDto
    {
        public string TipoReporte { get; set; }
        public decimal Posicion { get; set; }
        public string Catalogo { get; set; }
        public string TablaBd { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
