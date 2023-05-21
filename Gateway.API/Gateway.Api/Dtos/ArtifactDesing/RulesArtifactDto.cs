using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos.ArtifactDesing
{
    public class RulesArtifactDto
    {
        public string OrderCode { get; set; }
        public decimal Secuencia { get; set; }
        public string Descripcion { get; set; }
        public string ClaveNorma { get; set; }
        public string ClaveIdioma { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
