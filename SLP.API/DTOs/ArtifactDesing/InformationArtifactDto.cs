
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Api.DTOs.Artifactdesign
{
    public class InformationArtifactDto
    {
        public InformationArtifactDto(GeneralArtifactDto pgeneralArtifac,
                                         List<NozzlesArtifactDto> pnozzlesArtifac,
                                         List<ChangingTablesArtifactDto> pchangingTablesArtifact,
                                         List<CharacteristicsArtifactDto> pcharacteristicsArtifact,
                                         VoltageKVDto pVoltageKV,
                                         NBAIBilKvDto pNBAI,
                                         ConnectionTypesDto pConnections,
                                         DerivationsDto pDerivations,
                                         TapsDto pTaps,
                                         TapBaanDto ptapBaan,
                                         NBAINeutroDto pNBAINeutro,
                                         List<LightningRodArtifactDto> plightningRodArtifact,
                                         List<RulesArtifactDto> prulesArtifact,
                                         WarrantiesArtifactDto pwarrantiesArtifact, 
                                         LabTestsArtifactDto plabTestsArtifact) {

                                        GeneralArtifact = pgeneralArtifac;
                                        NozzlesArtifact = pnozzlesArtifac;
                                        ChangingTablesArtifact = pchangingTablesArtifact;
                                        CharacteristicsArtifact = pcharacteristicsArtifact;

                                        VoltageKV = pVoltageKV;
                                        NBAI = pNBAI;
                                        Connections = pConnections;
                                        Derivations = pDerivations;
                                        Taps = pTaps;
                                        NBAINeutro = pNBAINeutro;
                                        TapBaan = ptapBaan;
                                        LightningRodArtifact = plightningRodArtifact;
                                        RulesArtifact = prulesArtifact;
                                        WarrantiesArtifact = pwarrantiesArtifact;
                                        LabTestsArtifact = plabTestsArtifact;
          

        }
        public InformationArtifactDto()
        {

        }

       
        public GeneralArtifactDto GeneralArtifact { get; set; }
        public List<NozzlesArtifactDto> NozzlesArtifact { get; set; }
        public List<ChangingTablesArtifactDto> ChangingTablesArtifact { get; set; }
        public List<CharacteristicsArtifactDto> CharacteristicsArtifact { get; set; }

        public VoltageKVDto VoltageKV { get; set; }
        public NBAIBilKvDto NBAI { get; set; }
        public ConnectionTypesDto Connections { get; set; }
        public DerivationsDto Derivations { get; set; }
        public TapsDto Taps { get; set; }
        public NBAINeutroDto NBAINeutro { get; set; }
        public TapBaanDto TapBaan { get; set; }
        public List<LightningRodArtifactDto> LightningRodArtifact { get; set; }
        public List<RulesArtifactDto> RulesArtifact { get; set; }
        public WarrantiesArtifactDto WarrantiesArtifact { get; set; }
        public LabTestsArtifactDto LabTestsArtifact { get; set; }

    }
}
