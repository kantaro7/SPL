namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class AllCharacteristicsArtifactDTO
    {
        public string CodOrden { get; set; }
        public List<CharacteristicsArtifactDTO> ListEnfriamientos { get; set; }
        public VoltageKVDTO VoltageKV { get; set; }
        public NBAIBilKvDTO NBAI { get; set; }
        public ConnectionTypesDTO Connections { get; set; }
        public DerivationsDTO Derivations { get; set; }
        public TapsDTO Taps { get; set; }
        public NBAINeutroDTO NBAINeutro { get; set; }
    }
}
