namespace SPL.Domain.SPL.Reports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using System.Linq;
    using System.Text;

    using global::SPL.Domain.SPL.Reports.FPC;
    using global::SPL.Domain.SPL.Reports.ROD;
    using global::SPL.Domain.SPL.Reports.RCT;
    using global::SPL.Domain.SPL.Reports.FPB;
    using global::SPL.Domain.SPL.Reports.PCE;
    using global::SPL.Domain.SPL.Reports.PCI;
    using global::SPL.Domain.SPL.Reports.PLR;
    using global::SPL.Domain.SPL.Reports.PRD;
    using global::SPL.Domain.SPL.Reports.PEE;
    using global::SPL.Domain.SPL.Reports.ISZ;
    using global::SPL.Domain.SPL.Reports.RYE;
    using global::SPL.Domain.SPL.Reports.PIR;
    using global::SPL.Domain.SPL.Reports.PIM;
    using global::SPL.Domain.SPL.Reports.TIN;
    using global::SPL.Domain.SPL.Reports.TAP;
    using global::SPL.Domain.SPL.Reports.CEM;
    using global::SPL.Domain.SPL.Reports.TDP;
    using global::SPL.Domain.SPL.Reports.CGD;
    using global::SPL.Domain.SPL.Reports.RDD;
    using global::SPL.Domain.SPL.Reports.FPA;
    using global::SPL.Domain.SPL.Reports.IND;
    using global::SPL.Domain.SPL.Reports.BPC;
    using global::SPL.Domain.SPL.Reports.NRA;
    using global::SPL.Domain.SPL.Reports.DPR;
    using global::SPL.Domain.SPL.Reports.ETD;

    public interface IReportsInfrastructure
    {
        #region PCI

        Task<IEnumerable<PCIParameters>> AAsync(
            string pNoSerie,
            string pWindingMaterial,
            List<int> pCapacity,
            string pAtPositions,
            string pBtPositions,
            string pTerPositions,
            bool pIsAT,
            bool pIsBT,
            bool pIsTer);

        #endregion
        public Task<CheckInfoROD> CheckInfoROD(string pNoSerie, string pWindingMaterial, string pAtPositions, string pBtPositions, string pTerPositions,
      bool pIsAT, bool pIsBT, bool pIsTer);

        public Task<CheckInfoROD> CheckInfoPCE(string pNoSerie, List<int> pCapacity, string pAtPositions, string pBtPositions, string pTerPositions,
bool pIsAT, bool pIsBT, bool pIsTer);

        public Task<List<Reports>> GetReports(string pTypeReport);
        public Task<List<InfoGeneralTypesReports>> GetResultDetailsReports(string pNroSerie, string pTypeReport);
        public Task<List<FilterReports>> GetFiltersReports(string pTypeReport);
        public Task<List<ConfigurationReports>> GetConfigurationReport(string pTypeReport, string pKeyTest, int pNumberColumns);
        public Task<List<ConsolidatedReport>> GetConsolidatedReport(string pNoSerie, string pLenguage);
        public Task<List<TypeConsolidatedReport>> GetTypeConsolidatedReport(string pNoSerie, string pLenguage);
        public Task<List<ConsolidatedReport>> GetTestedReport(string pNoSerie);

        public Task<List<ColumnTitleRADReports>> GetColumnsConfigurableRAD(string pTypeUnit, string pLenguage, string pThirdWinding);

        public Task<List<ColumnTitleRDTReports>> GetColumnsConfigurableRDT(string pKeyTest, string pDAngular, string pRule, string pLenguage);

        public Task<TitSeriresParallelReports> GetTitSeriesParallel(string pClave, string pLenguage);

        public Task<CorrectionFactorReports> GetCorrectionFactor(string pClave, string pLenguage);

        public Task<long> SaveInfoRADReport(RADReport report);

        public Task<long> SaveInfoRDTReport(RDTReport report);

        public Task<List<ColumnTitleFPCReports>> GetColumnsConfigurableFPC(string pUnitType, string pLenguage);

        public Task<long> SaveInfoRANReport(RANReport report);

        public Task<long> SaveInfoFPCReport(FPCTestsGeneral report);
        public Task<long> SaveInfoRCTReport(RCTTestsGeneral report);
        public Task<long> SaveInfoRODReport(RODTestsGeneral report);

        public Task<long> SaveInfoFPBReport(FPBTestsGeneral report);
        public Task<ReportPDF> GetPDFReport(long pCode, string pTypeReport);

        public Task<RODTestsGeneral> GetInfoRODReport(string NroSerie, string KeyTest, string TestConnection, string WindingMaterial, string UnitOfMeasurement, bool Result);
        public Task<PCETestsGeneral> GetInfoPCEReport(string NroSerie, string KeyTest, bool Result);
        public Task<long> SaveInfoPCIReport(PCITestGeneral report);

        public Task<long> SaveInfoPCEReport(PCETestsGeneral report);

        public Task<long> SaveInfoPLRReport(PLRTestsGeneral report);

        public Task<long> SaveInfoPRDReport(PRDTestsGeneral report);

        public Task<long> SaveInfoPEEReport(PEETestsGeneral report);

        public Task<long> SaveInfoISZReport(ISZTestsGeneral report);

        public Task<long> SaveInfoRYEReport(RYETestsGeneral report);

        public Task<PEETestsGeneral> GetInfoPEEReport(string NroSerie, string KeyTest, bool Result);

        public Task<PCITestGeneral> GetInfoPCIReport(string NroSerie, string KeyTest, bool Result);

        public Task<long> SaveInfoPIMReport(PIMTestsGeneral report);

        public Task<long> SaveInfoPIRReport(PIRTestsGeneral report);

        public Task<FPCTestsGeneral> GetInfoFPCReport(string NroSerie, string KeyTest, string Lenguage, string UnitType, decimal Frecuency, bool Result);

        public Task<long> SaveInfoTINReport(TINTestsGeneral report);

        public Task<long> SaveInfoTAPReport(TAPTestsGeneral report);

        public Task<List<ColumnTitleCEMReports>> GetColumnsConfigurableCEM(decimal pTypeTrafoId, string pKeyLenguage, string pPos, string pPosSecundary, string pNoSerieNormal);

        public Task<long> SaveInfoCEMReport(CEMTestsGeneral report);

        public Task<long> SaveInfoTDPReport(TDPTestsGeneral report);

        public Task<long> SaveInfoCGDReport(CGDTestsGeneral report);

        public Task<long> SaveInfoARFReport(ARFTestsGeneral report);
        public Task<long> SaveInfoRDDReport(RDDTestsGeneral report);

        public Task<CGDTestsGeneral> GetInfoCGDReport(string NroSerie, string KeyTests, bool Result);

        public Task<long> SaveInfoINDReport(INDTestsGeneral report);
        public Task<long> SaveInfoFPAReport(FPATestsGeneral report);
        public Task<long> SaveInfoBPCReport(BPCTestsGeneral report);

        public Task<long> SaveInfoNRAReport(NRATestsGeneral report);

        public Task<long> SaveInfoDPRReport(DPRTestsGeneral report);

        public Task<long> SaveInfoETDReport(ETDReport report);

        public Task<RCTTestsGeneral> GetInfoRCTReport(string NroSerie, string KeyTest, bool Result);

        public Task<List<ETDConfigFileReport>> GetConfigFileEtd();

    }
}