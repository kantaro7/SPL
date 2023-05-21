using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.ArtifactDesign
{
    public class AllCharacteristicsArtifact
    {
        public string CodOrden { get; set; }
        public List<CharacteristicsArtifact> ListEnfriamientos { get; set; }
        public VoltageKV VoltageKV { get; set; }
        public NBAIBilKv NBAI { get; set; }
        public ConnectionTypes Connections { get; set; }
        public Derivations Derivations { get; set; }
        public Taps Taps { get; set; }
        public NBAINeutro NBAINeutro { get; set; }
    }
}
