using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCO.Domain.Artifactdesign
{
    public class RulesArtifact
    {
        public string OrderCode { get; set; }
        public decimal Secuencia { get; set; }
        public string Descripcion { get; set; }
        public string ClaveNorma { get; set; }
        public string ClaveIdioma { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
