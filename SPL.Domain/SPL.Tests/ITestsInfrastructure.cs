namespace SPL.Domain.SPL.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITestsInfrastructure
    {
        public Task<List<SPL.Tests.Tests>> GetTests(string pTypeReport, string pKeyTests);
        public Task<long> GetNumTestNextRAD(string pNroSerie, string pKeyTest, string pTypeUnit, string pThirdWinding, string pLenguage);

        public Task<long> GetNumTestNextRDT(string pNroSerie, string pKeyTest, string pDAngular, string pRule, string pLenguage);

        public Task<long> GetNumTestNextRAN(string pNroSerie, string pKeyTest, string pLenguage, int pNumberMeasurements);

        public Task<long> GetNumTestNextFPC(string pNroSerie, string pKeyTest, string pTypeUnit, string pSpecification, string pFrequency, string pLenguage);

        public Task<long> GetNumTestNextFPB(string pNroSerie, string pKeyTest, string pLenguage, string pTangentDelta);

        public Task<long> GetNumTestNextRCT(string pNroSerie, string pKeyTest, string pLenguage, string pUnitOfMeasurement, string pTertiary, decimal pTestvoltage);

        public Task<long> GetNumTestNextROD(string noSerie, string keyTest, string lenguage, string connection, string unitType, string material, string unitOfMeasurement);

        public Task<long> GetNumTestNextPCE(string pNroSerie, string pKeyTest, string pLenguage, string pEnergizedWinding
           );

        public Task<long> GetNumTestNextPCI(string pNroSerie, string pKeyTest, string pLenguage, string pWindingMaterial, bool pCapRedBaja, bool pAutotransformer, bool pMonofasico, decimal pOverElevation, string pPosPi, string pPosSec);
        public Task<long> GetNumTestNextPLR(string pNroSerie, string pKeyTest, string pLenguage, decimal pRldnt, decimal pNominalVoltage, int pAmountOfTensions);

        public Task<long> GetNumTestNextPRD(string pNroSerie, string pKeyTest, string pLenguage, decimal pNominalVoltage);

        public Task<long> GetNumTestNextPEE(string pNroSerie, string pKeyTest, string pLenguage);

        public Task<long> GetNumTestNextISZ(string pNroSerie, string pKeyTest, string pLenguage,
           decimal pDegreesCor, string pPosAT, string pPosBT, string pPosTER, string pWindingEnergized, int pQTYNeutral, decimal pImpedanceGar, string pMaterialWinding);

        public Task<long> GetNumTestNextRYE(string pNroSerie, string pKeyTest, string pLenguage,
            int pNoConnectionsAT, int pNoConnectionsBT, int pNoConnectionsTER, decimal pVoltageAT, decimal pVoltageBT, decimal pVoltageTER, string pCoolingType);

        public Task<long> GetNumTestNextPIM(string pNroSerie, string pKeyTest, string pLenguage,
          string pConnection, string ApplyLow, string VoltageLevel);

        public Task<long> GetNumTestNextPIR(string pNroSerie, string pKeyTest, string pLenguage,
          string pConnection, string IncludesTertiary, string VoltageLevel);

        public Task<long> GetNumTestNextTAP(string pNroSerie, string pKeyTest, string pLenguage,
      string pUnitType, int pNoConnectionAT, int pNoConnectionBT, int pNoConnectionTER, string pIdCapAT, string pIdCapBT, string pIdCapTer);

        public Task<long> GetNumTestNextTIN(string pNroSerie, string pKeyTest, string pLenguage,
        string pConnection, decimal pVoltage);

        public Task<long> GetNumTestNextCEM(string pNroSerie, string pKeyTest, string pLenguage,
       string pIdPosPrimary, string pPosPrimary, string pIdPosSecundary, string pPosSecundary, decimal pTestsVoltage);




        public Task<long> GetNumTestNextCGD(string pNroSerie, string pKeyTest, string pLenguage, string pTypeOil);

        public Task<long> GetNumTestNextTDP(string pNroSerie, string pKeyTest, string pLenguage,
int pNoCycles, int pTotalTime, int pInterval, decimal pTimeLevel, decimal pOutputLevel, int pDescMayPc, int pDescMayMv, int pIncMaxPc, string pVoltageLevels, string pMeasurementType, string pTerminalsTest);

        public Task<long> GetNumTestNextRDD(string pNroSerie, string pKeyTest, string pLenguage,
string pConfig_Winding, string pConnection, decimal pPorc_Z, decimal pPorc_Jx);

        public Task<long> GetNumTestNextARF(string pNroSerie, string pKeyTest, string pLenguage,
string pTeam, string pTertiary2Low, string pTertiaryDisp, string pLevelsVoltage);

        public Task<long> GetNumTestNextIND(string pNroSerie, string pKeyTest, string pLenguage,
string pTcPurchased);

        public Task<long> GetNumTestNextFPA(string pNroSerie, string pKeyTest, string pLenguage, string pTypeOil);

        public Task<long> GetNumTestNextBPC(string pNroSerie, string pKeyTest, string pLenguage);

        public Task<long> GetNumTestNextNRA(string pNroSerie, string pKeyTest, string pLenguage, string pLaboratory, string pRule, string pFeeding, string pCoolingType);

        public Task<long> GetNumTestNextETD(string pNroSerie, string pKeyTest, string pLenguage, bool pTypeRegression, bool pBtDifCap, decimal pCapacityBt, string pTertiary2B, bool pTerCapRed, decimal pCapacityTer, string pWindingSplit);

        public Task<long> GetNumTestNextDPR(string pNroSerie, string pKeyTest, string pLenguage, int pNoCycles,
int pTotalTime, int pInterval, decimal pTimeLevel, decimal pOutputLevel, int pDescMayPc, int pDescMayMv, int pIncMaxPc, string pMeasurementType, string pTerminalsTest);


    }
}