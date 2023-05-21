using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Configuration
{
    public interface IConfigurationInfrastructure
    {

        public Task<List<CorrectionFactorSpecification>> GetCorrectionFactorSpecificationFPC(string pSpecification, decimal pTemperature, decimal pCorrectionFactor);

        public Task<long> saveCorrectionFactorSpecification(CorrectionFactorSpecification pData);

        public Task<long> deleteCorrectionFactorSpecification(CorrectionFactorSpecification pData);

        public Task<List<NozzleMarks>> GetNozzleMarks(long pIdMark, bool pStatus);

        public Task<List<TypesNozzleMarks>> GetTypeXMarksNozzle(long pIdMark, bool pStatus);

        public Task<List<CorrectionFactorsXMarksXTypes>> GetCorrectionFactorsXMarksXTypes();

        public Task<long> saveCorrectionFactorsXMarksXTypes(CorrectionFactorsXMarksXTypes pData);

        public Task<long> deleteCorrectionFactorsXMarksXTypes(CorrectionFactorsXMarksXTypes pData);

        public Task<CorrectionFactorsDesc> GetCorrectionFactorsDesc(string pSpecification, string pKeyLenguage);

        public Task<long> saveNozzleTypesByBrand(TypesNozzleMarks pData);
        public Task<long> deleteNozzleTypesByBrand(TypesNozzleMarks pData);
        public Task<List<TypesNozzleMarks>> GetNozzleTypesByBrand(long pIdMark);

        public Task<long> saveNozzleBrands(NozzleMarks pData);

        public Task<long> deleteNozzleBrands(NozzleMarks pData);

        public Task<List<NozzleMarks>> GetNozzleBrands(long pIdMark);

        public Task<List<ValidationTestsIsz>> GetValidationTestsISZ();

        public Task<List<ContGasCGD>> GetInfoContGasCGD(string pIdReport, string pKeyTests, string pTypeOil);


        public Task<List<InformationOctaves>> GetInformationOctaves(string pNroSerie, string pTypeInformation, string pDateData);

        public Task<long> ImportInformationOctaves(List<InformationOctaves> pData);

        public Task<long> UpdateInformationOctaves(List<InformationOctaves> pData);

        public Task<StabilizationDesignData> GetStabilizationDesignData(string pNroSerie);

        public Task<long> saveStabilizationDesignData(StabilizationDesignData pData);

        public Task<List<CorrectionFactorkWTypeCooling>> GetCorrectionFactorkWTypeCooling();

        public Task<long> saveCorrectionFactorkWTypeCooling(List<CorrectionFactorkWTypeCooling> pData);



        public Task<List<StabilizationData>> GetStabilizationData(string pNroSerie, bool? pStatus, bool? pStabilized);

        public Task<long> saveStabilizationData(StabilizationData pData);

        public Task<List<InformationLaboratories>> GetInformationLaboratories();

        public Task<long> closeStabilizationData(string pNroSerie, decimal pIdReg);

        public Task<HeaderCuttingData> GetInfoHeaderCuttingData(decimal pNroSerie);

        public Task<long> saveCuttingData(HeaderCuttingData pData);

        public Task<List<HeaderCuttingData>> GetCuttingDatas(string pNroSerie);



        public Task<long> saveInfoContGasCGD(ContGasCGD pData);


        public Task<long> deleteInfoContGasCGD(ContGasCGD pData);
    }
}
