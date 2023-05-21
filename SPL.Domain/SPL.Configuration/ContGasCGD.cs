using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Configuration
{
    public class ContGasCGD
    {
        public decimal Id { get; set; }
        public string TipoReporte { get; set; }
        public string ClavePrueba { get; set; }
        public string TipoAceite { get; set; }
        public string DesTipoReporte { get; set; }
        public string DesPrueba { get; set; }
        public decimal LimiteMax { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
