using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.ArtifactDesign
{
    public class InformationArtifact
    {
        public InformationArtifact(GeneralArtifact pgeneralArtifac,
                                         List<NozzlesArtifact> pnozzlesArtifac,
                                         List<ChangingTablesArtifact> pchangingTablesArtifact,
                                         List<CharacteristicsArtifact> pcharacteristicsArtifact,
                                         VoltageKV pVoltageKV,
                                         NBAIBilKv pNBAI,
                                         ConnectionTypes pConnections,
                                         Derivations pDerivations,
                                         Taps pTaps,
                                         TapBaan ptapBaan,
                                         NBAINeutro pNBAINeutro,
                                         List<LightningRodArtifact> plightningRodArtifact,
                                         List<RulesArtifact> prulesArtifact,
                                         WarrantiesArtifact pwarrantiesArtifact, 
                                         LabTestsArtifact plabTestsArtifact) {

                                        GeneralArtifact = pgeneralArtifac;
                                        NozzlesArtifact = pnozzlesArtifac;
                                        ChangingTablesArtifact = pchangingTablesArtifact;
                                        CharacteristicsArtifact = pcharacteristicsArtifact;
                                        VoltageKV = pVoltageKV;
                                        NBAI = pNBAI;
                                        Connections = pConnections;
                                        Derivations = pDerivations;
                                        Taps = pTaps;
                                        Tapbaan = ptapBaan;
                                        NBAINeutro = pNBAINeutro;
                                        LightningRodArtifact = plightningRodArtifact;
                                        RulesArtifact = prulesArtifact;
                                        WarrantiesArtifact = pwarrantiesArtifact;
                                        LabTestsArtifact = plabTestsArtifact;
          

        }

        public InformationArtifact(GeneralArtifact pgeneralArtifac,
                                       List<NozzlesArtifact> pnozzlesArtifac,
                                       List<ChangingTablesArtifact> pchangingTablesArtifact,
                                       List<CharacteristicsArtifact> pcharacteristicsArtifact,
                                       
                                       List<LightningRodArtifact> plightningRodArtifact,
                                       List<RulesArtifact> prulesArtifact,
                                       WarrantiesArtifact pwarrantiesArtifact,
                                       LabTestsArtifact plabTestsArtifact)
        {

            GeneralArtifact = pgeneralArtifac;
            NozzlesArtifact = pnozzlesArtifac;
            ChangingTablesArtifact = pchangingTablesArtifact;
            CharacteristicsArtifact = pcharacteristicsArtifact;
            
            LightningRodArtifact = plightningRodArtifact;
            RulesArtifact = prulesArtifact;
            WarrantiesArtifact = pwarrantiesArtifact;
            LabTestsArtifact = plabTestsArtifact;

        }
        public InformationArtifact()
        {

        }

       
        public GeneralArtifact GeneralArtifact { get; set; }
        public List<NozzlesArtifact> NozzlesArtifact { get; set; }
        public List<ChangingTablesArtifact> ChangingTablesArtifact { get; set; }
        public List<CharacteristicsArtifact> CharacteristicsArtifact { get; set; }

        public VoltageKV VoltageKV { get; set; }
        public NBAIBilKv NBAI { get; set; }
        public ConnectionTypes Connections { get; set; }
        public Derivations Derivations { get; set; }
        public Taps Taps { get; set; }
        public NBAINeutro NBAINeutro { get; set; }
        public TapBaan Tapbaan { get; set; }
        public List<LightningRodArtifact> LightningRodArtifact { get; set; }
        public List<RulesArtifact> RulesArtifact { get; set; }
        public WarrantiesArtifact WarrantiesArtifact { get; set; }
        public LabTestsArtifact LabTestsArtifact { get; set; }

    }
}
