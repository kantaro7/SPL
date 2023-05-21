using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos
{
    public class ContGasCGDDto
    {
        public string TipoReporte { get; set; }
        public string ClavePrueba { get; set; }
        public string TipoAceite { get; set; }
        public decimal LimiteMax { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
