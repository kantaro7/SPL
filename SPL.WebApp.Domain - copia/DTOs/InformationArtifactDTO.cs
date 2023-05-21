namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class InformationArtifactDTO
    {
        public InformationArtifactDTO()
        {
            this.NozzlesArtifact = new List<NozzlesArtifactDTO>();
            this.ChangingTablesArtifact = new List<ChangingTablesArtifactDTO>();
            this.CharacteristicsArtifact = new List<CharacteristicsArtifactDTO>();
            this.LightningRodArtifact = new List<LightningRodArtifactDTO>();
            this.RulesArtifact = new List<RulesArtifactDTO>();
            this.Derivations = new DerivationsDTO();
        }

        public GeneralArtifactDTO GeneralArtifact { get; set; }

        public List<NozzlesArtifactDTO> NozzlesArtifact { get; set; }

        public List<ChangingTablesArtifactDTO> ChangingTablesArtifact { get; set; }

        public List<CharacteristicsArtifactDTO> CharacteristicsArtifact { get; set; }

        public VoltageKVDTO VoltageKV { get; set; }

        public NBAIBilKvDTO NBAI { get; set; }

        public ConnectionTypesDTO Connections { get; set; }

        public DerivationsDTO Derivations { get; set; }

        public TapsDTO Taps { get; set; }

        public NBAINeutroDTO NBAINeutro { get; set; }

        public TapBaanDTO TapBaan { get; set; }

        public List<LightningRodArtifactDTO> LightningRodArtifact { get; set; }

        public List<RulesArtifactDTO> RulesArtifact { get; set; }

        public WarrantiesArtifactDTO WarrantiesArtifact { get; set; }

        public LabTestsArtifactDTO LabTestsArtifact { get; set; }
    }
}
