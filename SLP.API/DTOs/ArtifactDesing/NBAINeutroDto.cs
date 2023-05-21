using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Api.DTOs.Artifactdesign
{
    public class NBAINeutroDto
    {
        public NBAINeutroDto() { }
        public decimal? ValorNbaiNeutroAltaTension { get; set; }
        public decimal? ValorNbaiNeutroBajaTension { get; set; }
        public decimal? ValorNbaiNeutroSegundaBaja { get; set; }
        public decimal? ValorNbaiNeutroTercera { get; set; }
    }
}
