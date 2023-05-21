using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos
{
    public class CorrectionFactorReportsDto
    {
        public decimal ClaveEsp { get; set; }
        public string ClaveIdioma { get; set; }
        public string Descripcion { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
