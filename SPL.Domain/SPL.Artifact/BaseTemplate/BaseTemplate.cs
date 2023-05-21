using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.BaseTemplate
{
    public class BaseTemplate
    {
        public string TipoReporte { get; set; }
        public string ClavePrueba { get; set; }
        public string ClaveIdioma { get; set; }
        public byte[] Plantilla { get; set; }
        public decimal ColumnasConfigurables { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
