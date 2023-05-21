namespace SPL.Tests.Infrastructure.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.EntityFrameworkCore;

    using SPL.Domain.SPL.Tests;
    using SPL.Tests.Infrastructure.Entities;

    public class TestsInfrastructure : ITestsInfrastructure
    {

        private readonly dbTestsSPLContext _dbContext;
        private readonly IMapper _Mapper;

        public TestsInfrastructure(IMapper Map, dbTestsSPLContext dbContext)
        {
            _Mapper = Map;
            _dbContext = dbContext;
        }

        #region Methods

        public Task<List<Tests>> GetTests(string pTypeReport, string pKeyTests)
        {
            List<Tests> Tests = null;

            if (pTypeReport == "-1" && pKeyTests != "-1")
                Tests = _Mapper.Map<List<Tests>>(_dbContext.SplPruebas.AsNoTracking().Where(x => x.ClavePrueba.Equals(pKeyTests)).OrderBy(x => x.TipoReporte).OrderBy(x => x.ClavePrueba));

            else
            if (pTypeReport != "-1" && pKeyTests == "-1")
                Tests = _Mapper.Map<List<Tests>>(_dbContext.SplPruebas.AsNoTracking().Where(x => x.TipoReporte.Equals(pTypeReport)).OrderBy(x => x.TipoReporte).OrderBy(x => x.ClavePrueba));
            else
            if (pTypeReport == "-1" && pKeyTests == "-1")
                Tests = _Mapper.Map<List<Tests>>(_dbContext.SplPruebas.AsNoTracking().AsEnumerable().OrderBy(x => x.TipoReporte).OrderBy(x => x.ClavePrueba));

            return Task.FromResult(Tests);
        }

        public Task<long> GetNumTestNextRAD(string pNroSerie, string pKeyTest, string pTypeUnit, string pThirdWinding, string pLenguage)
        {
            IEnumerable<SplInfoGeneralRad> list = _dbContext.SplInfoGeneralRads.Where(x => x.NoSerie.Equals(pNroSerie) && x.ClavePrueba.Equals(pKeyTest) && x.TipoUnidad.Equals(pTypeUnit) && x.TercerDevanadoTipo.Equals(pThirdWinding) && x.ClaveIdioma.Equals(pLenguage)).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextRDT(string pNroSerie, string pKeyTest, string pDAngular, string pRule, string pLenguage)
        {

            IEnumerable<SplInfoGeneralRdt> list = _dbContext.SplInfoGeneralRdts.Where(x => x.NoSerie.Equals(pNroSerie) && x.ClavePrueba.Equals(pKeyTest) && x.DesplazamientoAngular.Equals(pDAngular) && x.Norma.Equals(pRule) && x.ClaveIdioma.Equals(pLenguage)).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);

        }

        public Task<long> GetNumTestNextRAN(string pNroSerie, string pKeyTest, string pLenguage, int pNumberMeasurements)
        {

            IEnumerable<SplInfoGeneralRan> list = _dbContext.SplInfoGeneralRans.Where(x => x.NoSerie.Equals(pNroSerie) && x.ClavePrueba.Equals(pKeyTest) && x.ClaveIdioma.Equals(pLenguage) && x.CantMediciones == pNumberMeasurements).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);

        }

        public Task<long> GetNumTestNextFPC(string pNroSerie, string pKeyTest, string pTypeUnit, string pSpecification, string pFrequency, string pLenguage)
        {

            IEnumerable<SplInfoGeneralFpc> list = _dbContext.SplInfoGeneralFpcs.Where(x => x.NoSerie.Equals(pNroSerie) && x.ClavePrueba.Equals(pKeyTest) && x.TipoUnidad.Equals(pTypeUnit) && x.Especificacion.Equals(pSpecification) && x.Frecuencia.Equals(pFrequency) && x.ClaveIdioma.Equals(pLenguage)).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }
        public Task<long> GetNumTestNextFPB(string pNroSerie, string pKeyTest, string pLenguage, string pTangentDelta)
        {

            IEnumerable<SplInfoGeneralFpb> list = _dbContext.SplInfoGeneralFpbs.Where(x => x.NoSerie.Equals(pNroSerie) && x.ClavePrueba.Equals(pKeyTest) && x.ClaveIdioma.Equals(pLenguage) && x.TanDelta.Equals(pTangentDelta)).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextRCT(string pNroSerie, string pKeyTest, string pLenguage, string pUnitOfMeasurement, string pTertiary, decimal pTestvoltage)
        {

            IEnumerable<SplInfoGeneralRct> list = _dbContext.SplInfoGeneralRcts.Where(x => x.NoSerie.Equals(pNroSerie) && x.ClavePrueba.Equals(pKeyTest) && x.ClaveIdioma.Equals(pLenguage) && x.UnidadMedida.Equals(pUnitOfMeasurement)).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextROD(string noSerie, string keyTest, string lenguage, string connection, string unitType, string material, string unitOfMeasurement)
        {
            IEnumerable<SplInfoGeneralRod> list = _dbContext.SplInfoGeneralRods.Where(x => x.NoSerie.Equals(noSerie) && x.ClavePrueba.Equals(keyTest) && x.TipoUnidad.Equals(unitType) && x.MaterialDevanado.Equals(material) && x.UnidadMedida.Equals(unitOfMeasurement) && x.ClaveIdioma.Equals(lenguage)).AsNoTracking().AsEnumerable();

            long result = list.Any() ? Convert.ToInt64(list.Max(x => x.NoPrueba)) : 0;
            return Task.FromResult(result + 1);
        }
        public Task<long> GetNumTestNextPCE(string pNroSerie, string pKeyTest, string pLenguage, string pEnergizedWinding)
        {

            IEnumerable<SplInfoGeneralPce> list = _dbContext.SplInfoGeneralPces.Where(x => x.NoSerie.Equals(pNroSerie) && x.ClavePrueba.Equals(pKeyTest) && x.ClaveIdioma.Equals(pLenguage)
            && x.DevEnergizado.Equals(pEnergizedWinding)).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextPCI(string pNroSerie, string pKeyTest, string pLenguage, string pWindingMaterial, bool pCapRedBaja, bool pAutotransformer, bool pMonofasico, decimal pOverElevation, string pPosPi, string pPosSec)
        {            
            string CapRedBaja = (pCapRedBaja == true) ? "Si" : "No";
            
            string Autotransformer = (pAutotransformer == true) ? "Si" : "No";
            
            string Monofasico = (pMonofasico == true) ? "Si" : "No";
            
            IEnumerable<SplInfoGeneralPci> list = _dbContext.SplInfoGeneralPcis
                .Where(x =>
                    x.NoSerie.Equals(pNroSerie)
                    && x.ClavePrueba.Equals(pKeyTest)
                    && x.ClaveIdioma.Equals(pLenguage)
                    && x.MaterialDevanado.Equals(pWindingMaterial)
                    && x.CapRedBaja.Equals(CapRedBaja)
                    && x.Autotransformador.Equals(Autotransformer)
                    && x.Monofasico.Equals(Monofasico)
                    && x.PosPri == pPosPi
                    && x.PosSec == pPosSec
                    ).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextPLR(string pNroSerie, string pKeyTest, string pLenguage, decimal pRldnt, decimal pNominalVoltage, int pAmountOfTensions)
        {

            IEnumerable<SplInfoGeneralPlr> list = _dbContext.SplInfoGeneralPlrs.
                Where(x => x.NoSerie.Equals(pNroSerie)
                && x.ClavePrueba.Equals(pKeyTest)
                && x.ClaveIdioma.Equals(pLenguage)
                && x.Rldtn == pRldnt && x.TensionNominal == pNominalVoltage && x.CantTensiones == pAmountOfTensions).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextPRD(string pNroSerie, string pKeyTest, string pLenguage, decimal pNominalVoltage)
        {

            IEnumerable<SplInfoGeneralPrd> list = _dbContext.SplInfoGeneralPrds.Where(x => x.NoSerie.Equals(pNroSerie) && x.ClavePrueba.Equals(pKeyTest) && x.ClaveIdioma.Equals(pLenguage)
            && x.VoltajeNominal == pNominalVoltage).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextPEE(string pNroSerie, string pKeyTest, string pLenguage)
        {

            IEnumerable<SplInfoGeneralPee> list = _dbContext.SplInfoGeneralPees.Where(x => x.NoSerie.Equals(pNroSerie) && x.ClavePrueba.Equals(pKeyTest) && x.ClaveIdioma.Equals(pLenguage)).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextISZ(string pNroSerie, string pKeyTest, string pLenguage,
            decimal pDegreesCor, string pPosAT, string pPosBT, string pPosTER, string pWindingEnergized, int pQTYNeutral, decimal pImpedanceGar, string pMaterialWinding)
        {
            pPosAT ??= "";
            pPosBT ??= "";
            pPosTER ??= "";
            IEnumerable<SplInfoGeneralIsz> list = _dbContext.SplInfoGeneralIszs.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper()) && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper())).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextRYE(string pNroSerie, string pKeyTest, string pLenguage,
            int pNoConnectionsAT, int pNoConnectionsBT, int pNoConnectionsTER, decimal pVoltageAT, decimal pVoltageBT, decimal pVoltageTER, string pCoolingType)
        {

            IEnumerable<SplInfoGeneralRye> list = _dbContext.SplInfoGeneralRyes.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper()) && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper()) && x.NoConexionesAt == pNoConnectionsAT && x.NoConexionesBt == pNoConnectionsBT && x.NoConexionesTer == pNoConnectionsTER && x.TensionAt == pVoltageAT && x.TensionBt == pVoltageBT && x.TensionTer == pVoltageTER && x.CoolingType.ToUpper().Equals(pCoolingType.ToUpper())).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextPIM(string pNroSerie, string pKeyTest, string pLenguage,
          string pConnection, string pApplyLow, string pVoltageLevel)
        {

            IEnumerable<SplInfoGeneralPim> list = _dbContext.SplInfoGeneralPims.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper()) && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper()) && x.Conexion.ToUpper().Equals(pConnection.ToUpper()) && x.AplicaBaja.ToUpper().Equals(pApplyLow.ToUpper()) && x.NivelTension.ToUpper().Equals(pVoltageLevel.ToUpper())).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextPIR(string pNroSerie, string pKeyTest, string pLenguage,
          string pConnection, string pIncludesTertiary, string pVoltageLevel)
        {

            IEnumerable<SplInfoGeneralPir> list = _dbContext.SplInfoGeneralPirs.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper()) && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper()) && x.Conexion.ToUpper().Equals(pConnection.ToUpper())
            && x.IncluyeTerciario.ToUpper().Equals(pIncludesTertiary.ToUpper()) && x.NivelTension.ToUpper().Equals(pVoltageLevel.ToUpper())).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextTAP(string pNroSerie, string pKeyTest, string pLenguage,
          string pUnitType, int pNoConnectionAT, int pNoConnectionBT, int pNoConnectionTER, string pIdCapAT, string pIdCapBT, string pIdCapTer)
        {
            //if (pIdCapAT == null)
            //{
            //    pIdCapAT = "";
            //}
            //if (pIdCapBT == null)
            //{
            //    pIdCapBT = "";
            //}
            //if (pIdCapTer == null)
            //{
            //    pIdCapTer = null;
            //}

            IEnumerable<SplInfoGeneralTap> list = _dbContext.SplInfoGeneralTaps.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper()) && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper()) && x.TipoUnidad.ToUpper().Equals(pUnitType.ToUpper())
            && x.NoConexionesAt == pNoConnectionAT && x.NoConexionesBt == pNoConnectionBT && x.NoConexionesTer == pNoConnectionTER).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextTIN(string pNroSerie, string pKeyTest, string pLenguage,
        string pConnection, decimal pVoltage)
        {

            IEnumerable<SplInfoGeneralTin> list = _dbContext.SplInfoGeneralTins.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper()) && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper()) && x.Conexion.ToUpper().Equals(pConnection.ToUpper())
            && x.Tension == pVoltage).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextCEM(string pNroSerie, string pKeyTest, string pLenguage,
       string pIdPosPrimary, string pPosPrimary, string pIdPosSecundary, string pPosSecundary, decimal pTestsVoltage)
        {

            IEnumerable<SplInfoGeneralCem> list = _dbContext.SplInfoGeneralCems.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper())
            && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper())
            && x.IdPosPrimaria.ToUpper().Equals(pIdPosPrimary.ToUpper())
            //&& x.PosPrimaria.ToUpper().Equals(pPosPrimary.ToUpper())
            && x.IdPosSecundaria.ToUpper().Equals(pIdPosSecundary.ToUpper())
            //&& x.PosSecundaria.ToUpper().Equals(pPosSecundary.ToUpper())
            && x.VoltajePrueba == pTestsVoltage).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextCGD(string pNroSerie, string pKeyTest, string pLenguage, string pTypeOil)
        {

            IEnumerable<SplInfoGeneralCgd> list = _dbContext.SplInfoGeneralCgds.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper())
            && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper())
            && x.TipoAceite.ToUpper().Equals(pTypeOil.ToUpper())
            ).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextTDP(string pNroSerie, string pKeyTest, string pLenguage,
int pNoCycles, int pTotalTime, int pInterval, decimal pTimeLevel, decimal pOutputLevel, int pDescMayPc, int pDescMayMv, int pIncMaxPc, string pVoltageLevels, string pMeasurementType, string pTerminalsTest)
        {

            IEnumerable<SplInfoGeneralTdp> list = _dbContext.SplInfoGeneralTdps.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper())
            && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper())
            && x.NoCiclos == pNoCycles
            && x.TiempoTotal == pTotalTime
            && x.Intervalo == pInterval
            && x.NivelHora == pTimeLevel
            && x.NivelRealce == pOutputLevel
            && x.DescMayPc == pDescMayPc
            && x.DescMayMv == pDescMayMv
            && x.IncMaxPc == pIncMaxPc
            && x.NivelesTension.ToUpper().Equals(pVoltageLevels.ToUpper())
            && x.TipoMedicion.ToUpper().Equals(pMeasurementType.ToUpper())
            && x.TerminalesPrueba.ToUpper().Equals(pTerminalsTest.ToUpper())).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextRDD(string pNroSerie, string pKeyTest, string pLenguage,
string pConfig_Winding, string pConnection, decimal pPorc_Z, decimal pPorc_Jx)
        {

            IEnumerable<SplInfoGeneralRdd> list = _dbContext.SplInfoGeneralRdds.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper())
            && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper())
            && x.ConfigDevanado.ToUpper().Equals(pConfig_Winding.ToUpper())
            && x.Conexion.ToUpper().Equals(pConnection.ToUpper())
            && x.PorcZ == pPorc_Z
            && x.PorcJx == pPorc_Jx).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextARF(string pNroSerie, string pKeyTest, string pLenguage,
string pTeam, string pTertiary2Low, string pTertiaryDisp, string pLevelsVoltage)
        {

            IEnumerable<SplInfoGeneralArf> list = _dbContext.SplInfoGeneralArves.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper())
            && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper())
            && x.Equipo.ToUpper().Equals(pTeam.ToUpper())
            && x.Terciario2dabaja.ToUpper().Equals(pTertiary2Low.ToUpper())
            && x.TerciarioDisp.ToUpper().Equals(pTertiaryDisp.ToUpper())
            && x.NivelesTension.ToUpper().Equals(pLevelsVoltage.ToUpper())
            ).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextIND(string pNroSerie, string pKeyTest, string pLenguage,
string pTcPurchased)
        {

            IEnumerable<SplInfoGeneralInd> list = _dbContext.SplInfoGeneralInds.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper())
            && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper())
            && x.TcComprados.ToUpper().Equals(pTcPurchased.ToUpper())
            ).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextFPA(string pNroSerie, string pKeyTest, string pLenguage, string pTypeOil)
        {

            IEnumerable<SplInfoGeneralFpa> list = _dbContext.SplInfoGeneralFpas.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper())
            && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper())
            && x.TipoAceite.ToUpper().Equals(pTypeOil.ToUpper())
           ).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextBPC(string pNroSerie, string pKeyTest, string pLenguage)
        {

            IEnumerable<SplInfoGeneralBpc> list = _dbContext.SplInfoGeneralBpcs.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper())
            && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper())).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextNRA(string pNroSerie, string pKeyTest, string pLanguage, string pLaboratory, string pRule, string pFeeding, string pCoolingType)
        {
            IEnumerable<SplInfoGeneralNra> list = _dbContext.SplInfoGeneralNras.Where(x => x.NoSerie.Equals(pNroSerie) && x.ClavePrueba.Equals(pKeyTest) && x.ClaveIdioma.Equals(pLanguage) && x.Laboratorio.Equals(pLaboratory) && x.ClaveNorma.Equals(pRule) && x.Alimentacion.Equals(pFeeding) && x.CoolingType.Equals(pCoolingType)).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextETD(string pNroSerie, string pKeyTest, string pLanguage, bool pTypeRegression, bool pBtDifCap, decimal pCapacityBt, string pTertiary2B, bool pTerCapRed, decimal pCapacityTer, string pWindingSplit)
        {
            IEnumerable<SplInfoGeneralEtd> list = _dbContext.SplInfoGeneralEtds.Where(x => x.NoSerie.Equals(pNroSerie) && x.ClavePrueba.Equals(pKeyTest) && x.ClaveIdioma.Equals(pLanguage) && x.TipoRegresion == pTypeRegression && x.BtDifCap == pBtDifCap && x.CapacidadBt == pCapacityBt && x.Terciario2b.Equals(pTertiary2B) && x.TerCapRed == pTerCapRed && x.CapacidadTer == pCapacityTer && x.DevanadoSplit.ToUpper().Equals(pWindingSplit)).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }

        public Task<long> GetNumTestNextDPR(string pNroSerie, string pKeyTest, string pLenguage, int pNoCycles,
 int pTotalTime, int pInterval, decimal pTimeLevel, decimal pOutputLevel, int pDescMayPc, int pDescMayMv, int pIncMaxPc, string pMeasurementType, string pTerminalsTest)
        {

            IEnumerable<SplInfoGeneralDpr> list = _dbContext.SplInfoGeneralDprs.Where(x => x.NoSerie.ToUpper().Equals(pNroSerie.ToUpper()) && x.ClavePrueba.ToUpper().Equals(pKeyTest.ToUpper())
            && x.ClaveIdioma.ToUpper().Equals(pLenguage.ToUpper())
            && x.NumeroCiclo == pNoCycles
            && x.TiempoTotal == pTotalTime
            && x.Intervalo == pInterval
            && x.NivelHora == pTimeLevel
            && x.NivelRealce == pOutputLevel
            && x.DescMayPc == pDescMayPc
            && x.DescMayMv == pDescMayMv
            && x.IncMaxPc == pIncMaxPc

            && x.TipoMedicion.ToUpper().Equals(pMeasurementType.ToUpper())
            && x.TerminalesPrueba.ToUpper().Equals(pTerminalsTest.ToUpper())).AsNoTracking().AsEnumerable();

            long result = 0;

            if (list.Any())
                result = Convert.ToInt64(list.Max(x => x.NoPrueba));

            return Task.FromResult(result + 1);
        }
        #endregion
    }
}
