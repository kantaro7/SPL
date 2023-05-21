using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.BaseTemplate
{
    public class BaseTemplateConsolidatedReport
    {
        public string ClaveIdioma { get; set; }
        public byte[] Plantilla { get; set; }
        public string NombreArchivo { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
