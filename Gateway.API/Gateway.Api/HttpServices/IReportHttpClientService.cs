namespace Gateway.Api.HttpServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Gateway.Api.Dtos;
    using Gateway.Api.Dtos.ArtifactDesing;
    using Gateway.Api.Dtos.Nozzle;
    using Gateway.Api.Dtos.Reports.CGD;

    using SPL.Domain;

    public interface IReportHttpClientService
    {
        public Task<ApiResponse<List<FilterReportsDto>>> GetFilterReports(string TypeReport, string tokenSesion);

        public Task<ApiResponse<List<TestsDto>>> GetTests(string typeReport, string keyTest, string tokenSesion);

        public Task<List<GeneralPropertiesDto>> GetTypeUnit(string tokenSesion);

        public Task<ApiResponse<List<GeneralPropertiesDto>>> GetThirdWinding(string tokenSesion);
        public Task<List<GeneralPropertiesDto>> GetAngularDisplacement(string tokenSesion);

        public Task<List<GeneralPropertiesDto>> GetRules(string tokenSesion);

        public Task<InformationArtifactDto> GetArtifact(string nroSerie, string tokenSesion);

        public Task<List<CatSidcoInformationDto>> GetCatSIDCO(string tokenSesion);

        public Task<ApiResponse<List<ColumnTitleRADReportsDto>>> GetTitleColumnsRAD(string typeUnit, string thirdWinding, string lenguage, string tokenSesion);
        public Task<ApiResponse<List<ConfigurationReportsDto>>> GetConfigurationReports(string typeReport, string keyTest, long numberColumns, string tokenSesion);
        public Task<ApiResponse<long>> GetNroTestNextRAD(string nroSerie, string keyTest, string typeUnit, string thirdWinding, string lenguage, string tokenSesion);
        public Task<ApiResponse<BaseTemplateDto>> GetBaseTemplate(string typeReport, string keyTest, string keyLanguage, long numberColumns, string tokenSesion);
        public Task<ApiResponse<List<ColumnTitleRDTReportsDto>>> GetTitleColumnsRDT(string keyTest, string dAngular, string rule, string lenguage, string tokenSesion);
        public Task<ApiResponse<long>> GetNroTestNextRDT(string NroSerie, string KeyTest, string DAngular, string Rule, string Lenguage, string tokenSesion);
        public Task<ApiResponse<List<PlateTensionDto>>> GetPlateTension(string nroSerie, string pType, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextFPC(string nroSerie, string keyTest, string typeUnit, string specificationm, string frequency, string lenguage, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextROD(string noSerie, string keyTest, string lenguage, string connection, string unitType, string material, string unitOfMeasurement, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextRAN(string nroSerie, string keyTest, string lenguage, int numberMeasurements, string tokenSesion);

        public Task<ApiResponse<List<ColumnTitleFPCReportsDto>>> GetTitleColumnsFPC(string typeUnit, string lenguage, string tokenSesion);

        public Task<ApiResponse<CorrectionFactorsDescDto>> GetCorrectionFactorsDesc(string pSpecification, string pKeyLenguage, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextFPB(string nroSerie, string keyTest, string lenguage, string tangentDelta, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextRCT(string nroSerie, string keyTest, string lenguage, string unitOfMeasurement, string tertiary, decimal testvoltage, string tokenSesion);

        public Task<ApiResponse<NozzlesByDesignDto>> GetInformationBoqDet(string nroSerie, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextPCE(string nroSerie, string keyTest, string lenguage, string energizedWinding, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextPCI(string nroSerie, string keyTest, string lenguage, string windingMaterial, bool capRedBaja, bool autotransformer, bool monofasico, decimal overElevation, string posPi, string posSec, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextPLR(string nroSerie, string keyTest, string lenguage, decimal rldnt, decimal nominalVoltage, int amountOfTensions, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextPRD(string nroSerie, string keyTest, string lenguage, decimal nominalVoltage, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextPEE(string nroSerie, string keyTest, string lenguage, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextISZ(string nroSerie, string keyTest, string lenguage,
        decimal degreesCor, string posAT, string posBT, string posTER, string windingEnergized, int qtyNeutral, decimal impedanceGar, string materialWinding, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextRYE(string nroSerie, string keyTest, string lenguage,
            int noConnectionsAT, int noConnectionsBT, int noConnectionsTER, decimal voltageAT, decimal voltageBT, decimal voltageTER, string coolingType, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextPIM(string nroSerie, string keyTest, string lenguage,
       string connection, string applyLow, string voltageLevel, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextPIR(string nroSerie, string keyTest, string lenguage,
          string connection, string includesTertiary, string voltageLevel, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextTAP(string nroSerie, string keyTest, string lenguage,
    string unitType, int noConnectionAT, int noConnectionBT, int noConnectionTER, string idCapAT, string idCapBT, string idCapTer, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextTIN(string nroSerie, string keyTest, string lenguage,
        string connection, decimal voltage, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextCEM(string nroSerie, string keyTest, string lenguage,
       string idPosPrimary, string posPrimary, string idPosSecundary, string posSecundary, decimal testsVoltage, string tokenSesion);

        public Task<ApiResponse<List<ColumnTitleCEMReportsDto>>> GetTitleColumnsCEM(decimal typeTrafoId, string keyLenguage, string pos, string posSecundary , string noSerieNormal, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextCGD(string nroSerie, string keyTest, string lenguage, string typeOil, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextTDP(string nroSerie, string keyTest, string lenguage,
int noCycles, int totalTime, int interval, decimal timeLevel, decimal outputLevel, int descMayPc, int descMayMv, int incMaxPc, string voltageLevels, string measurementType, string terminalsTest, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextRDD(string nroSerie, string keyTest, string lenguage,
string config_Winding, string connection, decimal porc_Z, decimal porc_Jx, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextARF(string nroSerie, string keyTest, string lenguage,
string team, string tertiary2Low, string tertiaryDisp, string levelsVoltage, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextIND(string nroSerie, string keyTest, string lenguage,
string tcPurchased, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextFPA(string nroSerie, string keyTest, string lenguage, string typeOil, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextBPC(string nroSerie, string keyTest, string lenguage, string tokenSesion);

        public Task<ApiResponse<List<ContGasCGDDto>>> GetInfoContGasCGD(string IdReport, string KeyTests, string TypeOil, string tokenSesion);

        public Task<ApiResponse<CGDTestsGeneralDto>> GetInfoCGD(string NroSerie, string KeyTests, bool Result, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextNRA(string nroSerie, string keyTest, string language, string laboratory, string rule, string feeding, string coolingType, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextETD(string nroSerie, string keyTest, string language, bool typeRegression, bool btDifCap, decimal capacityBt, string tertiary2B, bool terCapRed, decimal capacityTer, string windingSplit, string tokenSesion);

        public Task<ApiResponse<long>> GetNroTestNextDPR(string nroSerie, string keyTest, string lenguage, int noCycles,
 int totalTime, int interval, decimal timeLevel, decimal outputLevel, int descMayPc, int descMayMv, int incMaxPc, string measurementType, string terminalsTest, string tokenSesion);

        public Task<ApiResponse<List<InformationOctavesDto>>> GetInformationOctaves(string NroSerie, string TypeInformation, string DateData, string tokenSesion);

        public Task<ApiResponse<HeaderCuttingDataDto>> GetInfoHeaderCuttingData(string NroSerie, string tokenSesion);

        public Task<ApiResponse<HeaderCuttingDataDto>> GetInfoHeaderCuttingData(decimal IdCut, string tokenSesion);

        public Task<ApiResponse<List<StabilizationDataDto>>> GetStabilizationData(string NroSerie, bool? Status, bool? Stabilized, string tokenSesion);
        public Task<ApiResponse<StabilizationDesignDataDto>> GetStabilizationDesignData(string NroSerie, string tokenSesion);

        public Task<ApiResponse<List<CorrectionFactorkWTypeCoolingDto>>> GetCorrectionFactorkWTypeCooling(string tokenSesion);

        public Task<ApiResponse<List<ETDConfigFileReportDto>>> GetConfigurationETDDownload(string tokenSesion);
    }
}
