
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.ArtifactDesign
{
    public interface IArtifactdesignInfrastructure
    {
        public Task<InformationArtifact> GetGeneralArtifactdesign(String serial);
        public Task<bool> CheckOrderNumber(String serial);
        public Task<decimal> GetBoqTerciary(String serial);
        public  Task<long> SaveInformationArtifact(InformationArtifact pData);
        public  Task<long> UpdategeneralArtifac(GeneralArtifact pData);
        public  Task<long> UpdateNozzlesArtifact(List<NozzlesArtifact> pData);
        public  Task<long> UpdateChangingTablesArtifact(AllChangingTablesArtifact pData);

        public Task<long> UpdateCharacteristicsArtifact(AllCharacteristicsArtifact pData);

        public  Task<long> UpdateLightningRodArtifact(List<LightningRodArtifact> pData);
        public  Task<long> UpdateRulesArtifact(List<RulesArtifact> pData);
        public  Task<long> UpdateWarrantiesArtifact(WarrantiesArtifact pData);
        public  Task<long> UpdateLabTestsArtifact(LabTestsArtifact pData);

        public Task<InfoCarLocal> GetInfoCarLocal(string nroSerie);
        public Task<TapBaan> GetTapBaan(string nroSerie);
        public Task<List<NozzlesArtifact>> GetSplInfoaparatoBoqs(string serial);

        public Task<List<ResistDesign>> GetResistDesign(string nroSerie, string unitOfMeasurement, string testConnection, decimal temperature, string idSection, decimal order);
        public Task<List<ResistDesign>> GetResistDesignCustom(string nroSerie, string unitOfMeasurement, string testConnection, decimal temperature, string idSection, decimal order);

        public Task<long> SaveResistDesign(List<ResistDesign> pData);

    }
}
