using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Masters
{
    public interface IMastersInfrastructure
    {

        public Task<List<CatSidcoInformation>> GetCatSidcoInformation();

        public Task<List<CatSidcoOtherInformation>> GetCatSidcoOtherInformation();

        public Task<List<GeneralProperties>> GetUnitType();

        public Task<List<GeneralProperties>> GetRulesEquivalents();

        public Task<List<GeneralProperties>> GetLanguageEquivalents();

        public Task<List<GeneralProperties>> GetEquivalentsAngularDisplacement();

        public Task<List<RulesRep>> GetRulesRep(string claveIdioma, string claveNorma);

        public Task<List<FileWeight>> GetConfigurationFiles(long module);

        public Task<List<GeneralProperties>> GetThirdWinding();

    }
}
