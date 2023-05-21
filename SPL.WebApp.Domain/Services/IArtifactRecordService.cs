namespace SPL.WebApp.Domain.Services
{
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;

    public interface IArtifactRecordService
    {
        List<ValuesChanges> GenerateLetters(int position, int nominal, bool reverse = false);
        List<ValuesChanges> GenerateArrayNum(int position, int nominal, bool reverse = false);
        List<ValuesChanges> GenerateArrayRandL(string letterSup, string letterInf, int posSup, int posInf, bool reverse);
        void FixConnections(IEnumerable<CatSidcoOthersDTO> catSidcoOthers, ref InformationArtifactDTO artifactDTO);

        void FixDerivations(ref InformationArtifactDTO artifactDTO);
    }
}
