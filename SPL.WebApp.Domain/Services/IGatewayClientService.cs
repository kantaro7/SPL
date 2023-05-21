namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.DPR;
    using SPL.WebApp.Domain.DTOs.ETD;

    using System;
    using System.Threading.Tasks;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    public interface IGatewayClientService
    {
        Task<ApiResponse<PositionsDTO>> GetPositions(string nroSerie);
        Task<ApiResponse<SettingsToDisplayRADReportsDTO>> GetTemplate(string typeReport, string nroSerie, string keyTest, string typeUnit, string thirdWinding, string keyLanguage);
        Task<ApiResponse<SettingsToDisplayRANReportsDTO>> GetTemplate(string typeReport, string nroSerie, string keyTest, int measuring, string keyLanguage);
        Task<ApiResponse<SettingsToDisplayRDTReportsDTO>> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string angular, string norm, int conexion, string posAT, string posBT, string posTer);
        Task<ApiResponse<SettingsToDisplayFPCReportsDTO>> GetTemplate(string noSerie, string clavePrueba, string unitType, string specification, string voltageLevel, decimal frequency, string claveIdioma);
        Task<ApiResponse<SettingsToDisplayRODReportsDTO>> GetTemplate(string noSerie, string keyTest, string lenguage, string connection, string unitType, string material, string unitOfMeasurement, decimal? testVoltage, long numberColumns, string atPositions, string btPositions, string terPositions);
        Task<ApiResponse<SettingsToDisplayRCTReportsDTO>> GetTemplate(string NroSerie, string KeyTest, string Lenguage, string UnitOfMeasurement, string Tertiary, decimal? Testvoltage);
        Task<ApiResponse<SettingsToDisplayFPBReportsDTO>> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string tangtDelta);
        Task<ApiResponse<SettingsToDisplayPCEReportsDTO>> GetTemplate(string nroSerie, string keyTest, string lenguage, string posAT, string posBT, string posTER, int beginning, int end, int interval, bool graph, string energizedWinding);
        Task<ApiResponse<SettingsToDisplayPCIReportsDTO>> GetTemplate(string nroSerie, string keyTest, string lenguage, string windingMaterial, bool capRedBaja, bool autotransformer, bool monofasico, decimal overElevation, string testCapacity, int cantPosPri, int cantPosSec, string posPi, string posSec);
        Task<ApiResponse<SettingsToDisplayPRDReportsDTO>> GetTemplate(string nroSerie, string keyTest, string lenguage, decimal nominalVoltage);
        Task<ApiResponse<SettingsToDisplayPLRReportsDTO>> GetTemplate(string nroSerie, string keyTest, string lenguage, decimal rldnt, decimal nominalVoltage, int amountOfTensions, int amountOfTime);
        Task<ApiResponse<SettingsToDisplayPEEReportsDTO>> GetTemplate(string nroSerie, string keyTest, string lenguage);
        Task<ApiResponse<SettingsToDisplayRYEReportsDTO>> GetTemplate(string nroSerie, string keyTest, string lenguage,
            int noConnectionsAT, int noConnectionsBT, int noConnectionsTER, decimal voltageAT, decimal voltageBT, decimal voltageTER, string coolingType);
        Task<ApiResponse<SettingsToDisplayISZReportsDTO>> GetTemplateISZ(string nroSerie, string keyTest, string lenguage,
           decimal degreesCor, string posAT, string posBT, string posTER, int qtyNeutral, string materialWinding, string denavado,string impedancia);

        Task<ApiResponse<SettingsToDisplayPIRReportsDTO>> GetTemplatePIR(string nroSerie, string keyTest, string lenguage, string connectionAt, string connectionBt, string connectionTer, string includesTertiary);
        Task<ApiResponse<SettingsToDisplayPIMReportsDTO>> GetTemplatePIM(string nroSerie, string keyTest, string lenguage, string connectionAt, string connectionBt, string applyLow);

        public Task<ApiResponse<SettingsToDisplayCEMReportsDTO>> GetTemplateCEM(string nroSerie, string keyTest, string lenguage,
       string idPosPrimary, string posPrimary, string idPosSecundary, string posSecundary, decimal testsVoltage);
        Task<ApiResponse<SettingsToDisplayTAPReportsDTO>> GetTemplateTAP(string nroSerie, string keyTest, string lenguage,
        string unitType, int noConnectionAT, int noConnectionBT, int noConnectionTER, string idCapAT, string idCapBT, string idCapTer);
        Task<ApiResponse<SettingsToDisplayTINReportsDTO>> GetTemplateTIM(string nroSerie, string keyTest, string lenguage, string connection, string tension);

        Task<ApiResponse<SettingsToDisplayTDPReportsDTO>> GetTemplateTDP(string nroSerie, string keyTest, string lenguage,
int noCycles, int totalTime, int interval, decimal timeLevel, decimal outputLevel, int descMayPc, int descMayMv, int incMaxPc, string voltageLevels, string measurementType, string terminalsTest);

        Task<ApiResponse<SettingsToDisplayCGDReportsDTO>> GetTemplateCGD(string nroSerie, string keyTest, string lenguage, int temperatureHour1, int temperatureHour2, int temperatureHour3, string oilType);

        Task<ApiResponse<SettingsToDisplayRDDReportsDTO>> GetTemplateRDD(string nroSerie, string keyTest, string lenguage, string configWinding, string connection, decimal porcZ, decimal porcJx);

        Task<ApiResponse<SettingsToDisplayBPCReportsDTO>> GetTemplateBPC(string nroSerie, string keyTest, string lenguage);
        Task<ApiResponse<SettingsToDisplayARFReportsDTO>> GetTemplateARF(string nroSerie, string keyTest, string lenguage, string team, string tertiary2Low, string tertiaryDisp, string levelsVoltage);

        Task<ApiResponse<SettingsToDisplayFPAReportsDTO>> GetTemplateFPA(string nroSerie, string keyTest, string lenguage, string oilType, int nroCol);
        Task<ApiResponse<SettingsToDisplayINDReportsDTO>> GetTemplateIND(string nroSerie, string keyTest, string lenguage, string tCBuyers);
        Task<ApiResponse<SettingsToDisplayNRAReportsDTO>> GetTemplateNRA(string nroSerie, string keyTest, string language, bool loadData, string selectloadData, string laboratory, string rule, string feeding, decimal feedingKVValue, string coolingType, int amountMeasureExistsOctaveInfo, DateTime? dataDate, int cantidadMaximadeMediciones);

        public  Task<ApiResponse<SettingsToDisplayDPRReportsDTO>> GetTemplateDPR(string nroSerie, string keyTest, string lenguage, int noCycles, int totalTime, int interval, decimal timeLevel, decimal outputLevel, int descMayPc, int descMayMv, int incMaxPc, string measurementType, string terminalsTest);

        Task<ApiResponse<SettingsToDisplayETDReportsDTO>> GetTemplateETD(string nroSerie, string keyTest, string lenguage, bool typeRegression, string overload, bool btDifCap, decimal capacityBt, string tertiary2B, bool terCapRed, decimal capacityTer, string windingSplit, decimal idCuttingData, decimal connection);

        Task<ApiResponse<SettingsToDisplayETDReportsDTO>> GetDownloadTemplateETD(string nroSerie, string keyTest, string lenguage);

        }
}
