using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Tests.Api.DTOs.Tests.Artifactdesign
{
    public class AllCharacteristicsArtifactDto
    {
        public string CodOrden { get; set; }
        public List<CharacteristicsArtifactDto> ListEnfriamientos { get; set; }
        public VoltageKVDto VoltageKV { get; set; }
        public NBAIBilKvDto NBAI { get; set; }
        public ConnectionTypesDto Connections { get; set; }
        public DerivationsDto Derivations { get; set; }
        public TapsDto Taps{ get; set; }
        public NBAINeutroDto NBAINeutro { get; set; }

    }
}
