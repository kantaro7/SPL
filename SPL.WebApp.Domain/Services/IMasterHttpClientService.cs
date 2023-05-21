namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMasterHttpClientService
    {

        /// <summary>
        /// Get all options by master key name
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<GeneralPropertiesDTO>> GetMasterByMethodKey(MethodMasterKeyName masterKeyName, int Microservices);

        /// <summary>
        /// Get rule from SIDCO
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        Task<IEnumerable<RulesArtifactDTO>> GetRuleFromSidco(string rule, string language);

        Task<IEnumerable<CatSidcoOthersDTO>> GetCatSidcoOthers();

        Task<ApiResponse<IEnumerable<FileWeightDTO>>> GetConfigurationFiles(int moduleId);

        Task<IEnumerable<GeneralPropertiesDTO>> GetConnectionTest(string noSerie);

        Task<ApiResponse<IEnumerable<ContGasCGDDTO>>> GetContGasCGD(string idReport, string keyTests, string typeOil);

        public  Task<ApiResponse<long>> SaveContGasCGD(ContGasCGDDTO dto);

        public  Task<ApiResponse<long>> DeleteContGasCGD(ContGasCGDDTO dto);

        Task<List<GeneralPropertiesDTO>> GetRuleEquivalents();
        Task<List<GeneralPropertiesDTO>> GetUnitTypes();
    }
}
