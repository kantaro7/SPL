namespace SPL.Reports.Infrastructure.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.BPC;
    using SPL.Domain.SPL.Reports.CEM;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.DPR;
    using SPL.Domain.SPL.Reports.ETD;
    using SPL.Domain.SPL.Reports.FPA;
    using SPL.Domain.SPL.Reports.FPB;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Reports.IND;
    using SPL.Domain.SPL.Reports.ISZ;
    using SPL.Domain.SPL.Reports.NRA;
    using SPL.Domain.SPL.Reports.PCE;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.PEE;
    using SPL.Domain.SPL.Reports.PIM;
    using SPL.Domain.SPL.Reports.PIR;
    using SPL.Domain.SPL.Reports.PLR;
    using SPL.Domain.SPL.Reports.PRD;
    using SPL.Domain.SPL.Reports.RCT;
    using SPL.Domain.SPL.Reports.RDD;
    using SPL.Domain.SPL.Reports.ROD;
    using SPL.Domain.SPL.Reports.RYE;
    using SPL.Domain.SPL.Reports.TAP;
    using SPL.Domain.SPL.Reports.TDP;
    using SPL.Domain.SPL.Reports.TIN;
    using SPL.Reports.Infrastructure.Common;
    using SPL.Reports.Infrastructure.Entities;

    public class ReportsInfrastructure : IReportsInfrastructure
    {

        private readonly dbReportsSPLContext _dbContext;
        private readonly IMapper _Mapper;

        public ReportsInfrastructure(IMapper Map, dbReportsSPLContext dbContext)
        {
            _Mapper = Map;
            _dbContext = dbContext;
        }

        #region Methods

        public Task<List<Reports>> GetReports(string pTypeReport) => pTypeReport == "-1"
                ? Task.FromResult(_Mapper.Map<List<Reports>>(_dbContext.SplReportes.AsNoTracking().AsEnumerable().OrderBy(x => x.TipoReporte)))
                : Task.FromResult(_Mapper.Map<List<Reports>>(_dbContext.SplReportes.AsNoTracking().Where(x => x.TipoReporte.Equals(pTypeReport)).OrderBy(x => x.TipoReporte)));

        public Task<List<InfoGeneralTypesReports>> GetResultDetailsReports(string pNroSerie, string pTypeReport)
        {

            List<InfoGeneralTypesReports> listaResult = new();
            List<SplInfoGeneralRad> resultRAD;
            List<SplInfoGeneralRdt> resultRDT;
            List<SplInfoGeneralRan> resultRAN;
            List<SplInfoGeneralFpc> resultFPC;
            List<SplInfoGeneralRct> resultRCT;
            List<SplInfoGeneralFpb> resultFPB;
            List<SplInfoGeneralRod> resultROD;
            List<SplInfoGeneralPci> resultPCI;
            List<SplInfoGeneralPce> resultPCE;
            List<SplInfoGeneralPrd> resultPRD;
            List<SplInfoGeneralPlr> resultPLR;
            List<SplInfoGeneralPee> resultPEE;
            List<SplInfoGeneralIsz> resultISZ;
            List<SplInfoGeneralRye> resultRYE;
            List<SplInfoGeneralTin> resultTIN;
            List<SplInfoGeneralTap> resultTAP;
            List<SplInfoGeneralPim> resultPIM;
            List<SplInfoGeneralPir> resultPIR;
            List<SplInfoGeneralCem> resultCEM;

            List<SplInfoGeneralTdp> resultTDP;
            List<SplInfoGeneralCgd> resultCGD;
            List<SplInfoGeneralRdd> resultRDD;
            List<SplInfoGeneralArf> resultARF;
            List<SplInfoGeneralInd> resultIND;
            List<SplInfoGeneralFpa> resultFPA;
            List<SplInfoGeneralBpc> resultBPC;

            List<SplInfoGeneralNra> resultNRA;
            List<SplInfoGeneralEtd> resultETD;
            List<SplInfoGeneralDpr> resultDPR;

            if (pTypeReport == Enums.EnumsGen.FilterAll.ToString())
            {
                resultRAD = _dbContext.SplInfoGeneralRads.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultRAD.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.RAD.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultRAD)));

                resultRDT = _dbContext.SplInfoGeneralRdts.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultRDT.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.RDT.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultRDT)));

                resultRAN = _dbContext.SplInfoGeneralRans.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultRAN.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.RAN.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultRAN)));

                resultFPC = _dbContext.SplInfoGeneralFpcs.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultFPC.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.FPC.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultFPC)));

                resultFPB = _dbContext.SplInfoGeneralFpbs.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultFPB.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.FPB.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultFPB)));

                resultRCT = _dbContext.SplInfoGeneralRcts.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultRCT.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.RCT.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultRCT)));

                resultROD = _dbContext.SplInfoGeneralRods.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultROD.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.ROD.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultROD)));

                resultPCI = _dbContext.SplInfoGeneralPcis.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultPCI.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PCI.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPCI)));

                resultPCE = _dbContext.SplInfoGeneralPces.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultPCE.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PCE.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPCE)));

                resultPLR = _dbContext.SplInfoGeneralPlrs.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultPLR.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PLR.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPLR)));

                resultPRD = _dbContext.SplInfoGeneralPrds.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultPRD.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PRD.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPRD)));

                resultPEE = _dbContext.SplInfoGeneralPees.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultPEE.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PEE.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPEE)));

                resultISZ = _dbContext.SplInfoGeneralIszs.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultISZ.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.ISZ.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultISZ)));

                resultRYE = _dbContext.SplInfoGeneralRyes.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultRYE.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.RYE.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultRYE)));

                resultTIN = _dbContext.SplInfoGeneralTins.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultTIN.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.TIN.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultTIN)));

                resultTAP = _dbContext.SplInfoGeneralTaps.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultTAP.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.TAP.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultTAP)));

                resultPIR = _dbContext.SplInfoGeneralPirs.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultPIR.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PIR.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPIR)));

                resultPIM = _dbContext.SplInfoGeneralPims.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultPIM.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PIM.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPIM)));

                resultCEM = _dbContext.SplInfoGeneralCems.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultCEM.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.CEM.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultCEM)));

                resultTDP = _dbContext.SplInfoGeneralTdps.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultTDP.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.TDP.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultTDP)));

                resultNRA = _dbContext.SplInfoGeneralNras.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                resultNRA.ForEach(c => c.Archivo = null);
                listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.NRA.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultNRA)));

            }

            else
            {
                switch (pTypeReport)
                {
                    case "RAD":
                        resultRAD = _dbContext.SplInfoGeneralRads.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(10).ToList();
                        resultRAD.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.RAD.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultRAD)));
                        break;
                    case "RDT":
                        resultRDT = _dbContext.SplInfoGeneralRdts.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(10).ToList();
                        resultRDT.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.RDT.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultRDT)));
                        break;
                    case "RAN":

                        resultRAN = _dbContext.SplInfoGeneralRans.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(10).ToList();
                        resultRAN.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.RAN.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultRAN)));
                        break;
                    case "FPC":
                        resultFPC = _dbContext.SplInfoGeneralFpcs.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(10).ToList();
                        resultFPC.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.FPC.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultFPC)));
                        break;

                    case "FPB":
                        resultFPB = _dbContext.SplInfoGeneralFpbs.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultFPB.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.FPB.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultFPB)));
                        break;
                    case "RCT":

                        resultRCT = _dbContext.SplInfoGeneralRcts.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultRCT.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.RCT.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultRCT)));
                        break;

                    case "ROD":
                        resultROD = _dbContext.SplInfoGeneralRods.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultROD.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.ROD.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultROD)));
                        break;

                    case "PCI":

                        resultPCI = _dbContext.SplInfoGeneralPcis.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultPCI.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PCI.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPCI)));
                        break;

                    case "PCE":
                        resultPCE = _dbContext.SplInfoGeneralPces.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultPCE.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PCE.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPCE)));
                        break;

                    case "PLR":
                        resultPLR = _dbContext.SplInfoGeneralPlrs.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultPLR.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PLR.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPLR)));
                        break;

                    case "PRD":
                        resultPRD = _dbContext.SplInfoGeneralPrds.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultPRD.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PRD.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPRD)));
                        break;

                    case "PEE":
                        resultPEE = _dbContext.SplInfoGeneralPees.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultPEE.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PEE.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPEE)));
                        break;

                    case "ISZ":
                        resultISZ = _dbContext.SplInfoGeneralIszs.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultISZ.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.ISZ.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultISZ)));
                        break;
                    case "RYE":
                        resultRYE = _dbContext.SplInfoGeneralRyes.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultRYE.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.RYE.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultRYE)));
                        break;
                    case "PIM":
                        resultPIM = _dbContext.SplInfoGeneralPims.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultPIM.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PIM.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPIM)));
                        break;
                    case "PIR":
                        resultPIR = _dbContext.SplInfoGeneralPirs.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultPIR.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.PIR.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultPIR)));
                        break;
                    case "TIN":
                        resultTIN = _dbContext.SplInfoGeneralTins.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultTIN.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.TIN.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultTIN)));
                        break;
                    case "TAP":
                        resultTAP = _dbContext.SplInfoGeneralTaps.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultTAP.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.TAP.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultTAP)));
                        break;
                    case "CEM":
                        resultCEM = _dbContext.SplInfoGeneralCems.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultCEM.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.CEM.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultCEM)));
                        break;

                    case "TDP":
                        resultTDP = _dbContext.SplInfoGeneralTdps.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultTDP.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.TDP.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultTDP)));
                        break;

                    case "CGD":
                        resultCGD = _dbContext.SplInfoGeneralCgds.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultCGD.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.CGD.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultCGD)));
                        break;

                    case "RDD":
                        resultRDD = _dbContext.SplInfoGeneralRdds.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultRDD.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.RDD.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultRDD)));
                        break;

                    case "ARF":
                        resultARF = _dbContext.SplInfoGeneralArves.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultARF.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.ARF.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultARF)));
                        break;

                    case "IND":
                        resultIND = _dbContext.SplInfoGeneralInds.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultIND.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.IND.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultIND)));
                        break;

                    case "FPA":
                        resultFPA = _dbContext.SplInfoGeneralFpas.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultFPA.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.FPA.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultFPA)));
                        break;

                    case "BPC":
                        resultBPC = _dbContext.SplInfoGeneralBpcs.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultBPC.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.BPC.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultBPC)));
                        break;

                    case "NRA":
                        resultNRA = _dbContext.SplInfoGeneralNras.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultNRA.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.NRA.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultNRA)));
                        break;

                    case "ETD":
                        resultETD = _dbContext.SplInfoGeneralEtds.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultETD.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.ETD.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultETD)));
                        break;

                    case "DPR":
                        resultDPR = _dbContext.SplInfoGeneralDprs.AsNoTracking().Where(x => x.NoSerie.Equals(pNroSerie)).OrderByDescending(x => x.Fechacreacion).Take(5).ToList();
                        resultDPR.ForEach(c => c.Archivo = null);
                        listaResult.Add(new InfoGeneralTypesReports(Enums.EnumsTypeReport.DPR.ToString(), _Mapper.Map<List<InfoGeneralReports>>(resultDPR)));
                        break;

                    default:

                        break;
                }
            }

            return Task.FromResult(listaResult.OrderBy(x => x.TipoReporte).ToList());

        }

        public Task<List<FilterReports>> GetFiltersReports(string pTypeReport)
        {
            List<SplFiltrosreporte> getFilterReports = new();

            getFilterReports = pTypeReport == "-1"
                ? _dbContext.SplFiltrosreportes.AsNoTracking().ToList()
                : _dbContext.SplFiltrosreportes.AsNoTracking().Where(x => x.TipoReporte.Equals(pTypeReport)).ToList();

            return Task.FromResult(_Mapper.Map<List<FilterReports>>(getFilterReports));

        }

        public Task<List<ETDConfigFileReport>> GetConfigFileEtd()
        {

            List<SplConfigArchivo> getConfigReports = _dbContext.SplConfigArchivos.AsNoTracking().ToList();

            return Task.FromResult(_Mapper.Map<List<ETDConfigFileReport>>(getConfigReports));

        }

        public Task<List<ConfigurationReports>> GetConfigurationReport(string pTypeReport, string pKeyTest, int pNumberColumns)
        {
            List<SqlParameter> parametros = new()
            {
                //if (serial != )
                Methods.CrearSqlParameter("@V_TIPO_REPORTE", SqlDbType.VarChar, pTypeReport),
                Methods.CrearSqlParameter("@V_CLAVE_PRUEBA", SqlDbType.VarChar, pKeyTest),
                Methods.CrearSqlParameter("@V_NO_COLUMNAS_CONFIGURADAS", SqlDbType.Int, pNumberColumns)
            };

            //EJECUTAR CONSULTA
            DataSet ds = Methods.EjecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_CONFIG_REPORTE]  (@V_TIPO_REPORTE,@V_CLAVE_PRUEBA,@V_NO_COLUMNAS_CONFIGURADAS)", parametros, "Configuration", _dbContext.Database.GetConnectionString());

            return Task.FromResult(ds.Tables["Configuration"].AsEnumerable().Select(result => new ConfigurationReports()
            {

                TipoReporte = result["TIPO_REPORTE"].ToString(),
                ClavePrueba = result["CLAVE_PRUEBA"].ToString(),
                Apartado = result["APARTADO"].ToString(),

                Seccion = string.IsNullOrEmpty(result["SECCION"].ToString()) ? 0 : Convert.ToDecimal(result["SECCION"].ToString()),

                Dato = result["DATO"].ToString(),

                Celda = result["CELDA"].ToString(),
                TipoDato = result["TIPO_DATO"].ToString(),
                Formato = result["FORMATO"].ToString(),
                Obtencion = result["OBTENCION"].ToString(),
                Creadopor = result["CREADOPOR"].ToString(),

                Fechacreacion = string.IsNullOrEmpty(result["FECHACREACION"].ToString()) ? null : Convert.ToDateTime(result["FECHACREACION"].ToString()),

                Modificadopor = result["MODIFICADOPOR"].ToString(),
                Fechamodificacion = string.IsNullOrEmpty(result["FECHAMODIFICACION"].ToString()) ? null : Convert.ToDateTime(result["FECHAMODIFICACION"].ToString()),

            }).ToList());
        }

        public Task<List<ColumnTitleRADReports>> GetColumnsConfigurableRAD(string pTypeUnit, string pLenguage, string pThirdWinding) => Task.FromResult(_Mapper.Map<List<ColumnTitleRADReports>>(_dbContext.SplTituloColumnasRads.Where(x => x.TipoUnidad.Equals(pTypeUnit) && x.TercerDevanadoTipo.Equals(pThirdWinding) && x.ClaveIdioma.Equals(pLenguage)).AsNoTracking().AsEnumerable()));

        public Task<List<ColumnTitleRDTReports>> GetColumnsConfigurableRDT(string pKeyTest, string pDAngular, string pRule, string pLenguage) => Task.FromResult(_Mapper.Map<List<ColumnTitleRDTReports>>(_dbContext.SplTituloColumnasRdts.Where(x => x.ClavePrueba.Equals(pKeyTest) && x.DesplazamientoAngular.Equals(pDAngular) && x.Norma.Equals(pRule) && x.ClaveIdioma.Equals(pLenguage)).AsNoTracking().AsEnumerable()));

        public Task<TitSeriresParallelReports> GetTitSeriesParallel(string pClave, string pLenguage) => Task.FromResult(_Mapper.Map<TitSeriresParallelReports>(_dbContext.SplTitSerieparalelos.Where(x => x.ClaveSepa.Equals(pClave) && x.ClaveIdioma.Equals(pLenguage)).AsNoTracking().FirstOrDefault()));

        public Task<CorrectionFactorReports> GetCorrectionFactor(string pClave, string pLenguage) => Task.FromResult(_Mapper.Map<CorrectionFactorReports>(_dbContext.SplDescFactorcorreccions.Where(x => x.ClaveEsp.Equals(pClave) && x.ClaveIdioma.Equals(pLenguage)).AsNoTracking().FirstOrDefault()));

        public Task<long> SaveInfoRADReport(RADReport report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                SplInfoGeneralRad data = _dbContext.SplInfoGeneralRads.AsNoTracking().FirstOrDefault(CE => CE.IdCarga == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralRad infoGenRad = new()
                {

                    FechaCarga = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    TipoUnidad = report.TypeUnit,
                    TercerDevanadoTipo = report.ThirdWindingType,
                    ValorMinimo = report.DataTests.AcceptanceValue,
                    Comentario = report.Comment,
                    Creadopor = report.DataTests.Creadopor,
                    Fechacreacion = report.DataTests.Fechacreacion,
                    Modificadopor = report.DataTests.Modificadopor,
                    Fechamodificacion = report.DataTests.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralRads.Add(infoGenRad) : _dbContext.SplInfoGeneralRads.Update(infoGenRad);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenRad.IdCarga;

                List<SplInfoDetalleRad> listdetails = new();

                foreach (string tiempo in report.DataTests.Times)
                {
                    int posTime = report.DataTests.Times.IndexOf(tiempo);

                    foreach (Domain.SPL.Tests.Column column in report.DataTests.Columns)
                    {
                        listdetails.Add(new SplInfoDetalleRad()
                        {
                            Seccion = 1,
                            IdCarga = idCarga,
                            Tiempo = tiempo,
                            PosicionColumna = report.DataTests.Columns.IndexOf(column) + 1,
                            ValorColumna = column.Values[posTime].ToString(),
                            Creadopor = report.DataTests.Creadopor,
                            Fechacreacion = report.DataTests.Fechacreacion,
                            Fechamodificacion = report.DataTests.Fechamodificacion,
                            Modificadopor = report.DataTests.Modificadopor,

                        });
                    }
                }

                if (report.DataTests.Columns2.Count > 0)
                {
                    foreach (string tiempo in report.DataTests.Times)
                    {
                        int posTime = report.DataTests.Times.IndexOf(tiempo);

                        foreach (Domain.SPL.Tests.Column column in report.DataTests.Columns2)
                        {
                            listdetails.Add(new SplInfoDetalleRad()
                            {
                                Seccion = 2,
                                Tiempo = tiempo,
                                IdCarga = idCarga,
                                PosicionColumna = report.DataTests.Columns2.IndexOf(column) + 1,
                                ValorColumna = column.Values[posTime].ToString(),
                                Creadopor = report.DataTests.Creadopor,
                                Fechacreacion = report.DataTests.Fechacreacion,
                                Fechamodificacion = report.DataTests.Fechamodificacion,
                                Modificadopor = report.DataTests.Modificadopor,
                            });
                        }
                    }
                }

                SplInfoDetalleRad datadetail = _dbContext.SplInfoDetalleRads.AsNoTracking().FirstOrDefault(CE => CE.IdCarga == report.IdLoad);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleRads.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleRads.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                List<SplInfoSeccionRad> listHeaders = new();

                foreach (Domain.SPL.Tests.HeaderRADTests item in report.DataTests.Headers)
                {

                    listHeaders.Add(new SplInfoSeccionRad() { IdCarga = idCarga, Seccion = item.Section, Tension = item.Tension, Umtension = item.UnitOfMeasureOfTension, Temperatura = item.Temperature, Umtemp = item.UnitOfMeasureOfTemperature, FechaPrueba = item.TestDate, Creadopor = report.DataTests.Creadopor, Fechacreacion = report.DataTests.Fechacreacion, Fechamodificacion = report.DataTests.Fechamodificacion, Modificadopor = report.DataTests.Modificadopor });

                }

                SplInfoSeccionRad datasec = _dbContext.SplInfoSeccionRads.AsNoTracking().FirstOrDefault(CE => CE.IdCarga == report.IdLoad);

                if (datasec is null)
                    _dbContext.SplInfoSeccionRads.AddRange(listHeaders);
                else
                    _dbContext.SplInfoSeccionRads.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(SaveInfoRADReport));

            }
        }

        public Task<long> SaveInfoRDTReport(RDTReport report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                SplInfoGeneralRdt data = _dbContext.SplInfoGeneralRdts.AsNoTracking().FirstOrDefault(CE => CE.IdCarga == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralRdt infoGenRdt = new()
                {
                    FechaCarga = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    DesplazamientoAngular = report.AngularDisplacement,
                    Norma = report.Rule,
                    ConexionSp = report.Connection_sp,
                    PostPruebaBt = report.PostTestBt,
                    Ter = report.Ter,
                    Fecha = report.Date,
                    PosAt = report.Pos_at,
                    Comentario = report.Comment,
                    Creadopor = report.DataTests.Creadopor,
                    Fechacreacion = report.DataTests.Fechacreacion,
                    Modificadopor = report.DataTests.Modificadopor,
                    Fechamodificacion = report.DataTests.Fechamodificacion,
                };

                _ = data is null ? _dbContext.SplInfoGeneralRdts.Add(infoGenRdt) : _dbContext.SplInfoGeneralRdts.Update(infoGenRdt);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenRdt.IdCarga;

                List<SplInfoDetalleRdt> listdetails = new();

                foreach (string item in report.DataTests.Positions)
                {
                    int position = report.DataTests.Positions.IndexOf(item);

                    listdetails.Add(new SplInfoDetalleRdt()
                    {

                        IdCarga = idCarga,
                        Posicion = item,
                        PosicionColumna = 1,

                        ValorNominal = report.DataTests.Columns[0].Values[position],
                        ValorColumna = report.DataTests.Columns[1].Values[position],
                        ValorDesv = report.DataTests.Columns[2].Values[position],

                        Hvvolts = report.DataTests.Columns[7].Values[position],
                        Lvvolts = report.DataTests.Columns[8].Values[position],

                        Creadopor = report.DataTests.Creadopor,
                        Fechacreacion = report.DataTests.Fechacreacion,
                        Modificadopor = report.DataTests.Modificadopor,
                        Fechamodificacion = report.DataTests.Fechamodificacion,
                        OrdenPosicion = report.DataTests.OrderPositions[position],

                    });

                    listdetails.Add(new SplInfoDetalleRdt()
                    {

                        IdCarga = idCarga,
                        Posicion = item,
                        PosicionColumna = 2,

                        ValorNominal = report.DataTests.Columns[0].Values[position],
                        ValorColumna = report.DataTests.Columns[3].Values[position],
                        ValorDesv = report.DataTests.Columns[4].Values[position],

                        Hvvolts = report.DataTests.Columns[7].Values[position],
                        Lvvolts = report.DataTests.Columns[8].Values[position],

                        Creadopor = report.DataTests.Creadopor,
                        Fechacreacion = report.DataTests.Fechacreacion,
                        Modificadopor = report.DataTests.Modificadopor,
                        Fechamodificacion = report.DataTests.Fechamodificacion,
                        OrdenPosicion = report.DataTests.OrderPositions[position],

                    });

                    listdetails.Add(new SplInfoDetalleRdt()
                    {

                        IdCarga = idCarga,
                        Posicion = item,
                        PosicionColumna = 3,

                        ValorNominal = report.DataTests.Columns[0].Values[position],
                        ValorColumna = report.DataTests.Columns[5].Values[position],
                        ValorDesv = report.DataTests.Columns[6].Values[position],

                        Hvvolts = report.DataTests.Columns[7].Values[position],
                        Lvvolts = report.DataTests.Columns[8].Values[position],

                        Creadopor = report.DataTests.Creadopor,
                        Fechacreacion = report.DataTests.Fechacreacion,
                        Modificadopor = report.DataTests.Modificadopor,
                        Fechamodificacion = report.DataTests.Fechamodificacion,
                        OrdenPosicion = report.DataTests.OrderPositions[position],

                    });
                }

                SplInfoDetalleRdt datadetail = _dbContext.SplInfoDetalleRdts.AsNoTracking().FirstOrDefault(CE => CE.IdCarga == report.IdLoad);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleRdts.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleRdts.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(SaveInfoRDTReport));

            }
        }

        public Task<long> SaveInfoRANReport(RANReport report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                SplInfoGeneralRan data = _dbContext.SplInfoGeneralRans.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;

                SplInfoGeneralRan infoGenRan = new()
                {
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    CantMediciones = report.NumberMeasurements,
                    Comentario = report.Comment,
                    Creadopor = report.Creadopor,
                    Fechacreacion = DateTime.Now,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralRans.Add(infoGenRan) : _dbContext.SplInfoGeneralRans.Update(infoGenRan);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenRan.IdRep;

                List<SplInfoDetalleRan> listdetails = new();
                listdetails.AddRange(report.Rans.SelectMany(item => item.RANTestsDetailsRAs
                        .GroupBy(e => e.Section, (key, rows) => rows.Select((e, index) => new SplInfoDetalleRan()
                        {
                            IdRep = idCarga,
                            Seccion = e.Section,
                            Renglon = index + 1,
                            FechaPrueba = e.Section == 1 ? item.RANTestsDetailsRAs.FirstOrDefault().DateTest : item.RANTestsDetailsTAs.FirstOrDefault().DateTest,
                            Descripcion = e.Description,
                            Medicion = e.Measurement,
                            UmMedicion = e.UMMeasurement,
                            Vcd = e.VCD,
                            Limite = e.Limit,
                            Duracion = e.Duration,
                            Tiempo = e.Time,
                            UmTiempo = e.UMTime,
                            Valido = null
                        }))
                        .SelectMany(e => e)));

                listdetails.AddRange(report.Rans.SelectMany(item => item.RANTestsDetailsTAs
                    .GroupBy(e => e.Section, (key, rows) => rows.Select((e, index) => new SplInfoDetalleRan()
                    {
                        IdRep = idCarga,
                        Seccion = e.Section,
                        Renglon = index + 1,
                        FechaPrueba = e.Section == 2 ? item.RANTestsDetailsRAs.FirstOrDefault().DateTest : item.RANTestsDetailsTAs.FirstOrDefault().DateTest,
                        Descripcion = e.Description,
                        Medicion = null,
                        UmMedicion = null,
                        Vcd = e.VCD,
                        Limite = e.Limit,
                        Duracion = e.Duration,
                        Tiempo = e.Time,
                        UmTiempo = e.UMTime,
                        Valido = report.LanguageKey.Equals("EN") ? e.Valid ? "Yes" : "Not" : e.Valid ? "Si" : "No"
                    }))
                    .SelectMany(e => e)));

                SplInfoDetalleRan datadetail = _dbContext.SplInfoDetalleRans.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleRans.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleRans.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(SaveInfoRANReport));

            }
        }

        public Task<long> SaveInfoFPCReport(FPCTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoSeccionFpc> listHeaders = new();
                SplInfoGeneralFpc data = _dbContext.SplInfoGeneralFpcs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralFpc infoGenFPC = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    TipoUnidad = report.TypeUnit,
                    Especificacion = report.Specification,
                    Frecuencia = report.Frequency,
                    NivelesTension = string.Empty,
                    Comentario = report.Comment,
                    Creadopor = report.Creadopor,
                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralFpcs.Add(infoGenFPC) : _dbContext.SplInfoGeneralFpcs.Update(infoGenFPC);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenFPC.IdRep;

                List<SplInfoDetalleFpc> listdetails = new();

                foreach (FPCTests obj in report.Data)
                {
                    int pos = report.Data.IndexOf(obj);

                    foreach (FPCTestsDetails item in obj.FPCTestsDetails)
                    {
                        listdetails.Add(new SplInfoDetalleFpc()
                        {
                            IdRep = idCarga,
                            Seccion = pos + 1,
                            Renglon = item.Row,
                            DevE = item.WindingA,
                            DevT = item.WindingB,
                            DevG = item.WindingC,
                            DevUst = item.WindingD,
                            IdCap = item.Id,
                            Corriente = item.Current,
                            Potencia = item.Power,
                            PorcFp = item.PercentageA,
                            Capacitancia = item.Capacitance,
                            CorrPorcFp = item.PercentageC,
                            TangPorcFp = item.PercentageB,
                        });
                    }

                    listHeaders.Add(new SplInfoSeccionFpc()
                    {
                        IdRep = idCarga,
                        Seccion = pos + 1,
                        FechaPrueba = obj.Date,
                        UmTension = obj.UmTension,
                        TempAceiteSup = obj.UpperOilTemperature,
                        UmTempacSup = obj.UmTempacSup,
                        TempAceiteInf = obj.LowerOilTemperature,
                        UmTempacInf = obj.UmTempacInf,
                        FactorCorr = obj.CorrectionFactorSpecifications.FactorCorr,
                        TempProm = decimal.Round(obj.TempFP, 0),
                        TempCt = obj.TempTanD,
                        AceptCap = obj.AcceptanceValueCap,
                        AcepFp = obj.AcceptanceValueFP,
                        TensionPrueba = obj.Tension

                    });
                }

                SplInfoDetalleFpc datadetail = _dbContext.SplInfoDetalleFpcs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleFpcs.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleFpcs.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                SplInfoSeccionFpc datasec = _dbContext.SplInfoSeccionFpcs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datasec is null)
                    _dbContext.SplInfoSeccionFpcs.AddRange(listHeaders);
                else
                    _dbContext.SplInfoSeccionFpcs.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);
            }
        }

        public Task<List<ColumnTitleFPCReports>> GetColumnsConfigurableFPC(string pUnitType, string pLenguage) => Task.FromResult(_Mapper.Map<List<ColumnTitleFPCReports>>(_dbContext.SplTituloColumnasFpcs.Where(x => x.TipoUnidad.Equals(pUnitType) && x.ClaveIdioma.Equals(pLenguage)).AsNoTracking().AsEnumerable()));

        public Task<long> SaveInfoRCTReport(RCTTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoSeccionFpc> listHeaders = new();
                SplInfoGeneralRct data = _dbContext.SplInfoGeneralRcts.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralRct infoGenRCT = new()
                {
                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    Ter2baja = report.Ter2low,
                    TensionPrueba = report.TensionTest,
                    UnidadMedida = report.UnitMeasure,
                    Comentario = report.Comment,
                    Creadopor = report.Creadopor,
                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,
                };

                _ = data is null ? _dbContext.SplInfoGeneralRcts.Add(infoGenRCT) : _dbContext.SplInfoGeneralRcts.Update(infoGenRCT);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenRCT.IdRep;

                List<SplInfoDetalleRct> listdetails = new();

                foreach (RCTTests obj in report.RCTTests)
                {
                    int Columna = report.RCTTests.IndexOf(obj);

                    foreach (RCTTestsDetails item in obj.RCTTestsDetails)
                    {
                        int Seccion = obj.RCTTestsDetails.IndexOf(item);

                        listdetails.Add(new SplInfoDetalleRct()
                        {
                            IdRep = idCarga,
                            Seccion = Seccion + 1,
                            Columna = Columna + 1,
                            Fase = item.Phase,
                            Posicion = item.Position,
                            Resistencia = item.Resistencia,
                            Temperatura = item.Temperature,
                            Terminal = item.Terminal,
                            TipoMedicion = report.MeasurementType,
                            Unidad = item.Unit
                        });
                    }
                }

                SplInfoDetalleRct datadetail = _dbContext.SplInfoDetalleRcts.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleRcts.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleRcts.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(SaveInfoFPCReport));

            }
        }

        public Task<RCTTestsGeneral> GetInfoRCTReport(string NroSerie, string KeyTest, bool Result)
        {
            try
            {
                List<RCTTests> listHeaders = new();
                SplInfoGeneralRct data = KeyTest == "-1"
                    ? _dbContext.SplInfoGeneralRcts.AsNoTracking().Where(CE => CE.NoSerie.Equals(NroSerie) && CE.Resultado == Result).OrderByDescending(x => x.IdRep).FirstOrDefault()
                    : _dbContext.SplInfoGeneralRcts.AsNoTracking().Where(CE => CE.NoSerie.Equals(NroSerie) && CE.ClavePrueba.Equals(KeyTest) && CE.Resultado == Result).OrderByDescending(x => x.IdRep).FirstOrDefault();
                if (data is null)
                {
                    throw new ArgumentException("No se encuentra prueba para el reporte -> RCT");
                }

                List<SplInfoDetalleRct> dataDetalle = _dbContext.SplInfoDetalleRcts.AsNoTracking().Where(CE => CE.IdRep == data.IdRep).ToList();

                List<RCTTestsDetails> listdetails = new();
                List<RCTTests> list = new();

                foreach (SplInfoDetalleRct item in dataDetalle)
                {

                    listdetails.Add(new RCTTestsDetails()
                    {

                        Phase = item.Fase,
                        Position = item.Posicion,
                        Resistencia = item.Resistencia is null ? 0 : (decimal)item.Resistencia,
                        Temperature = item.Temperatura,
                        Terminal = item.Terminal,
                        Unit = item.Unidad,

                    });

                }
                list.Add(new RCTTests() { RCTTestsDetails = listdetails });

                RCTTestsGeneral infoGenRCT = new()
                {

                    IdLoad = Convert.ToInt64(data.IdRep),
                    LoadDate = data.FechaRep,
                    TestNumber = Convert.ToInt32(data.NoPrueba),
                    SerialNumber = data.NoSerie,
                    LanguageKey = data.ClaveIdioma,
                    Customer = data.Cliente,
                    Capacity = data.Capacidad,
                    Result = data.Resultado,
                    NameFile = data.NombreArchivo,
                    File = data.Archivo,
                    TypeReport = data.TipoReporte,
                    KeyTest = data.ClavePrueba,
                    Ter2low = data.Ter2baja,
                    TensionTest = data.TensionPrueba == null ? 0 : Convert.ToDecimal(data.TensionPrueba),
                    UnitMeasure = data.UnidadMedida,
                    Comment = data.Comentario,
                    Creadopor = data.Creadopor,
                    Fechacreacion = data.Fechacreacion,
                    Modificadopor = data.Modificadopor,
                    Fechamodificacion = data.Fechamodificacion,
                    MeasurementType = data.UnidadMedida,
                    RCTTests = list,

                };

                return Task.FromResult(infoGenRCT);
            }

            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
        }

        public Task<long> SaveInfoFPBReport(FPBTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoSeccionFpb> listHeaders = new();
                SplInfoGeneralFpb data = _dbContext.SplInfoGeneralFpbs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralFpb infoGenFPB = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    TanDelta = report.TanDelta,
                    CantBoq = report.CantBoq,

                    Comentario = report.Comment,
                    Creadopor = report.Creadopor,
                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralFpbs.Add(infoGenFPB) : _dbContext.SplInfoGeneralFpbs.Update(infoGenFPB);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenFPB.IdRep;

                List<SplInfoDetalleFpb> listdetails = new();

                foreach (FPBTests obj in report.Data)
                {
                    int pos = report.Data.IndexOf(obj);

                    foreach (FPBTestsDetails item in obj.FPBTestsDetails)
                    {
                        int renglon = obj.FPBTestsDetails.IndexOf(item);
                        listdetails.Add(new SplInfoDetalleFpb()
                        {
                            IdRep = idCarga,
                            Seccion = pos + 1,
                            Renglon = renglon + 1,
                            Posicion = item.Id,
                            NoSerieBoq = item.NroSerie,
                            ColumnaT = item.T,
                            Corriente = item.Current,
                            Potencia = item.Power,
                            PorcFp = item.PercentageA,
                            PorcFpCorr = item.PercentageB,
                            Capacitancia = item.Capacitance,
                            IdMarca = item.NozzlesByDesign.IdMarca,
                            IdTipo = item.NozzlesByDesign.IdTipo,
                            FactorPotencia = item.NozzlesByDesign.FactorPotencia,
                            Capaci = item.NozzlesByDesign.Capacitancia,
                            FactorCorr = item.CorrectionFactorSpecificationsTemperature.FactorCorr,
                            FactorCorr2 = item.CorrectionFactorSpecifications20Grados.FactorCorr

                        });
                    }

                    listHeaders.Add(new SplInfoSeccionFpb()
                    {
                        IdRep = idCarga,
                        Seccion = pos + 1,
                        FechaPrueba = obj.Date,
                        TensionPrueba = obj.Tension,
                        UmTension = obj.UmTension,
                        Temperatura = obj.Temperature,
                        UmTemp = obj.UmTemperature,
                        TempPorcfp = obj.TempFP,
                        TempFptand = obj.TempTanD,
                        AceptCap = obj.AcceptanceValueCap,
                        AceptFp = obj.AcceptanceValueFP,

                    });
                }

                SplInfoDetalleFpb datadetail = _dbContext.SplInfoDetalleFpbs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleFpbs.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleFpbs.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                SplInfoSeccionFpb datasec = _dbContext.SplInfoSeccionFpbs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datasec is null)
                    _dbContext.SplInfoSeccionFpbs.AddRange(listHeaders);
                else
                    _dbContext.SplInfoSeccionFpbs.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(SaveInfoFPCReport));
            }
        }

        public Task<ReportPDF> GetPDFReport(long pCode, string pTypeReport)
        {

            ReportPDF repor = new();

            switch (pTypeReport)
            {
                case "RAD":
                    SplInfoGeneralRad data = _dbContext.SplInfoGeneralRads.AsNoTracking().Where(x => x.IdCarga.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(data.IdCarga);
                    repor.File = data.Archivo;
                    repor.ReportName = data.NombreArchivo;
                    break;
                case "RDT":
                    SplInfoGeneralRdt data2 = _dbContext.SplInfoGeneralRdts.AsNoTracking().Where(x => x.IdCarga.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(data2.IdCarga);
                    repor.File = data2.Archivo;
                    repor.ReportName = data2.NombreArchivo;
                    break;
                case "RAN":

                    SplInfoGeneralRan data3 = _dbContext.SplInfoGeneralRans.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(data3.IdRep);
                    repor.File = data3.Archivo;
                    repor.ReportName = data3.NombreArchivo;
                    break;
                case "FPC":
                    SplInfoGeneralFpc data4 = _dbContext.SplInfoGeneralFpcs.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(data4.IdRep);
                    repor.File = data4.Archivo;
                    repor.ReportName = data4.NombreArchivo;
                    break;

                case "FPB":
                    SplInfoGeneralFpb data5 = _dbContext.SplInfoGeneralFpbs.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(data5.IdRep);
                    repor.File = data5.Archivo;
                    repor.ReportName = data5.NombreArchivo;
                    break;
                case "RCT":

                    SplInfoGeneralRct data6 = _dbContext.SplInfoGeneralRcts.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(data6.IdRep);
                    repor.File = data6.Archivo;
                    repor.ReportName = data6.NombreArchivo;
                    break;

                case "ROD":
                    SplInfoGeneralRod data7 = _dbContext.SplInfoGeneralRods.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(data7.IdRep);
                    repor.File = data7.Archivo;
                    repor.ReportName = data7.NombreArchivo;
                    break;

                case "PCI":
                    SplInfoGeneralPci PCI = _dbContext.SplInfoGeneralPcis.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(PCI.IdRep);
                    repor.File = PCI.Archivo;
                    repor.ReportName = PCI.NombreArchivo;
                    break;
                case "PCE":
                    SplInfoGeneralPce PCE = _dbContext.SplInfoGeneralPces.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(PCE.IdRep);
                    repor.File = PCE.Archivo;
                    repor.ReportName = PCE.NombreArchivo;
                    break;
                case "PLR":
                    SplInfoGeneralPlr PLR = _dbContext.SplInfoGeneralPlrs.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(PLR.IdRep);
                    repor.File = PLR.Archivo;
                    repor.ReportName = PLR.NombreArchivo;
                    break;
                case "PRD":
                    SplInfoGeneralPrd PRD = _dbContext.SplInfoGeneralPrds.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(PRD.IdRep);
                    repor.File = PRD.Archivo;
                    repor.ReportName = PRD.NombreArchivo;
                    break;
                case "PEE":
                    SplInfoGeneralPee PEE = _dbContext.SplInfoGeneralPees.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(PEE.IdRep);
                    repor.File = PEE.Archivo;
                    repor.ReportName = PEE.NombreArchivo;
                    break;

                case "ISZ":
                    SplInfoGeneralIsz ISZ = _dbContext.SplInfoGeneralIszs.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(ISZ.IdRep);
                    repor.File = ISZ.Archivo;
                    repor.ReportName = ISZ.NombreArchivo;
                    break;

                case "RYE":
                    SplInfoGeneralRye RYE = _dbContext.SplInfoGeneralRyes.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(RYE.IdRep);
                    repor.File = RYE.Archivo;
                    repor.ReportName = RYE.NombreArchivo;
                    break;

                case "PIM":
                    SplInfoGeneralPim PIM = _dbContext.SplInfoGeneralPims.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(PIM.IdRep);
                    repor.File = PIM.Archivo;
                    repor.ReportName = PIM.NombreArchivo;
                    break;

                case "PIR":
                    SplInfoGeneralPir PIR = _dbContext.SplInfoGeneralPirs.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(PIR.IdRep);
                    repor.File = PIR.Archivo;
                    repor.ReportName = PIR.NombreArchivo;
                    break;

                case "TIN":
                    SplInfoGeneralTin TIN = _dbContext.SplInfoGeneralTins.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(TIN.IdRep);
                    repor.File = TIN.Archivo;
                    repor.ReportName = TIN.NombreArchivo;
                    break;

                case "TAP":
                    SplInfoGeneralTap TAP = _dbContext.SplInfoGeneralTaps.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(TAP.IdRep);
                    repor.File = TAP.Archivo;
                    repor.ReportName = TAP.NombreArchivo;
                    break;

                case "CEM":
                    SplInfoGeneralCem CEM = _dbContext.SplInfoGeneralCems.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(CEM.IdRep);
                    repor.File = CEM.Archivo;
                    repor.ReportName = CEM.NombreArchivo;
                    break;

                case "TDP":
                    SplInfoGeneralTdp TDP = _dbContext.SplInfoGeneralTdps.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(TDP.IdRep);
                    repor.File = TDP.Archivo;
                    repor.ReportName = TDP.NombreArchivo;
                    break;

                case "CGD":
                    SplInfoGeneralCgd CGD = _dbContext.SplInfoGeneralCgds.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(CGD.IdRep);
                    repor.File = CGD.Archivo;
                    repor.ReportName = CGD.NombreArchivo;
                    break;

                case "RDD":
                    SplInfoGeneralRdd RDD = _dbContext.SplInfoGeneralRdds.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(RDD.IdRep);
                    repor.File = RDD.Archivo;
                    repor.ReportName = RDD.NombreArchivo;
                    break;

                case "ARF":
                    SplInfoGeneralArf ARF = _dbContext.SplInfoGeneralArves.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(ARF.IdRep);
                    repor.File = ARF.Archivo;
                    repor.ReportName = ARF.NombreArchivo;
                    break;

                case "IND":
                    SplInfoGeneralInd IND = _dbContext.SplInfoGeneralInds.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(IND.IdRep);
                    repor.File = IND.Archivo;
                    repor.ReportName = IND.NombreArchivo;
                    break;

                case "FPA":
                    SplInfoGeneralFpa FPA = _dbContext.SplInfoGeneralFpas.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(FPA.IdRep);
                    repor.File = FPA.Archivo;
                    repor.ReportName = FPA.NombreArchivo;
                    break;

                case "BPC":
                    SplInfoGeneralBpc PCB = _dbContext.SplInfoGeneralBpcs.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(PCB.IdRep);
                    repor.File = PCB.Archivo;
                    repor.ReportName = PCB.NombreArchivo;
                    break;

                case "NRA":
                    SplInfoGeneralNra NRA = _dbContext.SplInfoGeneralNras.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(NRA.IdRep);
                    repor.File = NRA.Archivo;
                    repor.ReportName = NRA.NombreArchivo;
                    break;

                case "ETD":
                    SplInfoGeneralEtd ETD = _dbContext.SplInfoGeneralEtds.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(ETD.IdRep);
                    repor.File = ETD.Archivo;
                    repor.ReportName = ETD.NombreArchivo;
                    break;

                case "DPR":
                    SplInfoGeneralDpr DPR = _dbContext.SplInfoGeneralDprs.AsNoTracking().Where(x => x.IdRep.Equals(pCode)).FirstOrDefault();
                    repor.IdLoad = Convert.ToInt32(DPR.IdRep);
                    repor.File = DPR.Archivo;
                    repor.ReportName = DPR.NombreArchivo;
                    break;
                default:

                    break;
            }

            return Task.FromResult(repor);

        }

        #region ROD

        public Task<RODTestsGeneral> GetInfoRODReport(string NroSerie, string KeyTest, string TestConnection, string WindingMaterial, string UnitOfMeasurement, bool Result)
        {

            try
            {
                List<RODTests> listHeaders = new();
                SplInfoGeneralRod data = _dbContext.SplInfoGeneralRods.AsNoTracking().Where(CE => CE.NoSerie.Equals(NroSerie) && CE.ClavePrueba.Equals(KeyTest) && CE.MaterialDevanado.Equals(WindingMaterial) && CE.UnidadMedida.Equals(UnitOfMeasurement) && CE.Resultado == Result).OrderByDescending(x => x.IdRep).FirstOrDefault();

                if (data is null)
                {
                    throw new ArgumentException("No se encuentra prueba para el reporte -> Resistencia Óhmica de los Devanados con esas coincidencias -> NoSerie = " + NroSerie + "ConexionPrueba = " + TestConnection + " " + "MaterialDevanado = " + WindingMaterial + " UnidadMedida = " + UnitOfMeasurement);
                }

                List<SplInfoSeccionRod> dataSeccion = _dbContext.SplInfoSeccionRods.AsNoTracking().Where(CE => CE.IdRep == data.IdRep).ToList();

                List<SplInfoDetalleRod> dataDetalle = _dbContext.SplInfoDetalleRods.AsNoTracking().Where(CE => CE.IdRep == data.IdRep).ToList();

                List<RODTestsDetails> listdetails = new();

                foreach (SplInfoDetalleRod item in dataDetalle)
                {

                    listdetails.Add(new RODTestsDetails()
                    {

                        Position = item.Posicion,
                        TerminalH1 = item.Terminal1,
                        TerminalH2 = item.Terminal2,
                        TerminalH3 = item.Terminal3,
                        AverageResistance = item.ResistenciaProm,
                        PercentageA = item.Correccion20,
                        PercentageB = item.CorreccionSe,
                        Desv = item.PorcDesv,
                        DesvDesign = item.PorcDesvDis,
                        ResistDesigns = new Domain.SPL.Artifact.ArtifactDesign.ResistDesign() { Resistencia = item.ResDisenio }
                    });
                }

                foreach (SplInfoSeccionRod obj in dataSeccion)
                {

                    listHeaders.Add(new RODTests()
                    {

                        Date = obj.FechaPrueba,
                        Temperature = obj.Temperatura,
                        UmTemperature = obj.UmTemp,
                        Title1 = obj.TitTerm1,
                        Title2 = obj.TitTerm2,
                        Title3 = obj.TitTerm3,
                        TemperatureSE = obj.TempSe,
                        UmTemperatureSE = obj.UmTempse,
                        FactorCor20 = obj.Fc20,
                        FactorCorSE = obj.FcSe,
                        ValorAcepPhases = obj.VaDesv,
                        ValorAcMaDesign = obj.VaMaxDis,
                        AcMiDesignValue = obj.VaMinDis,
                        MaxPorc_Desv = obj.MaxDesv,
                        MaxPorc_DesvxDesign = obj.MaxDdis,
                        MinPorc_DesvxDesign = obj.MinDdis,
                        RODTestsDetails = listdetails,
                        ConexionPrueba = obj.ConexionPrueba

                    });
                }

                RODTestsGeneral infoGenROD = new()
                {

                    IdLoad = Convert.ToInt64(data.IdRep),
                    LoadDate = data.FechaRep,
                    TestNumber = Convert.ToInt32(data.NoPrueba),
                    SerialNumber = data.NoSerie,
                    LanguageKey = data.ClaveIdioma,
                    Customer = data.Cliente,
                    Capacity = data.Capacidad,
                    Result = data.Resultado,
                    NameFile = data.NombreArchivo,
                    File = data.Archivo,
                    TypeReport = data.TipoReporte,
                    KeyTest = data.ClavePrueba,
                    UnitType = data.TipoUnidad,
                    AutorizoCambio = data.AutorizoCambio,
                    WindingMaterial = data.MaterialDevanado,
                    TestVoltage = data.TensionPrueba,
                    UnitOfMeasurement = data.UnidadMedida,
                    Creadopor = data.Creadopor,
                    Comment = data.Comentario,

                    Fechacreacion = data.Fechacreacion,
                    Modificadopor = data.Modificadopor,
                    Fechamodificacion = data.Fechamodificacion,

                    Data = listHeaders,

                };
                return Task.FromResult(infoGenROD);
            }

            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
        }

        public Task<long> SaveInfoRODReport(RODTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoSeccionRod> listHeaders = new();
                SplInfoGeneralRod data = _dbContext.SplInfoGeneralRods.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralRod infoGenROD = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    TipoUnidad = report.UnitType,
                    AutorizoCambio = report.AutorizoCambio,
                    MaterialDevanado = report.WindingMaterial,
                    TensionPrueba = report.TestVoltage,
                    UnidadMedida = report.UnitOfMeasurement,
                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralRods.Add(infoGenROD) : _dbContext.SplInfoGeneralRods.Update(infoGenROD);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenROD.IdRep;

                List<SplInfoDetalleRod> listdetails = new();
                int i = 0;
                var listaConex = report.TestConnection.Split(",");
                foreach (RODTests obj in report.Data)
                {

                    int pos = report.Data.IndexOf(obj);
                    foreach (RODTestsDetails item in obj.RODTestsDetails)
                    {
                        int renglon = obj.RODTestsDetails.IndexOf(item);
                        listdetails.Add(new SplInfoDetalleRod()
                        {
                            IdRep = idCarga,
                            Seccion = pos + 1,
                            Renglon = renglon + 1,
                            Posicion = item.Position,
                            Terminal1 = Math.Round(item.TerminalH1, 6),
                            Terminal2 = Math.Round(item.TerminalH2, 6),
                            Terminal3 = Math.Round(item.TerminalH3, 6),
                            ResistenciaProm = Math.Round(item.AverageResistance, 4),
                            Correccion20 = Math.Round(item.PercentageA, 4),
                            CorreccionSe = Math.Round(item.PercentageB, 4),
                            PorcDesv = Math.Round(item.Desv > 1000 ? 999 : item.Desv, 2),
                            PorcDesvDis = Math.Round(item.DesvDesign > 1000 ? 999 : item.DesvDesign, 2),
                            ResDisenio = Math.Round(item.ResistDesigns.Resistencia, 6)

                        });
                    }

                    listHeaders.Add(new SplInfoSeccionRod()
                    {
                        IdRep = idCarga,
                        Seccion = pos + 1,
                        FechaPrueba = obj.Date,
                        Temperatura = Math.Round(obj.Temperature, 1),
                        UmTemp = obj.UmTemperature,
                        TitTerm1 = obj.Title1,
                        TitTerm2 = obj.Title2,
                        TitTerm3 = obj.Title3,
                        TempSe = Math.Round(obj.TempTanD, 1),
                        UmTempse = obj.UmTemperatureSE,
                        Fc20 = Math.Round(obj.FactorCor20, 6),
                        FcSe = Math.Round(obj.FactorCorSE, 6),
                        VaDesv = Math.Round(obj.ValorAcepPhases, 0),
                        VaMaxDis = Math.Round(obj.ValorAcMaDesign, 0),
                        VaMinDis = Math.Round(obj.AcMiDesignValue, 0),
                        MaxDesv = Math.Round(obj.MaxPorc_Desv > 100 ? 99 : obj.MaxPorc_Desv, 2),
                        MaxDdis = Math.Round(obj.MaxPorc_DesvxDesign > 100 ? 99 : obj.MaxPorc_DesvxDesign, 2),
                        MinDdis = Math.Round(obj.MinPorc_DesvxDesign > 100 ? 99 : obj.MinPorc_DesvxDesign, 2),
                        ConexionPrueba = report.TestConnection.Contains("L-L") || report.TestConnection.Contains("L-N") ? listaConex[0] : listaConex[i]

                    });

                    i++;
                }

                SplInfoDetalleRod datadetail = _dbContext.SplInfoDetalleRods.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleRods.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleRods.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                SplInfoSeccionRod datasec = _dbContext.SplInfoSeccionRods.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datasec is null)
                    _dbContext.SplInfoSeccionRods.AddRange(listHeaders);
                else
                    _dbContext.SplInfoSeccionRods.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(SaveInfoRODReport));

            }
        }

        #endregion

        #region PCE

        public Task<PCETestsGeneral> GetInfoPCEReport(string NroSerie, string KeyTest, bool Result)
        {
            try
            {
                List<PCETests> listHeaders = new();

                SplInfoGeneralPce data = _dbContext.SplInfoGeneralPces.AsNoTracking().Where(CE => CE.NoSerie.Equals(NroSerie) && CE.ClavePrueba.Equals(KeyTest) && CE.Resultado == Result).OrderByDescending(x => x.IdRep).FirstOrDefault();
                if (data is null)
                {
                    throw new ArgumentException("El reporte para esas coincidencias no se encuentra registrado");
                }

                List<SplInfoSeccionPce> dataSeccion = _dbContext.SplInfoSeccionPces.AsNoTracking().Where(CE => CE.IdRep == data.IdRep).ToList();

                List<SplInfoDetallePce> dataDetails = _dbContext.SplInfoDetallePces.AsNoTracking().Where(CE => CE.IdRep == data.IdRep).ToList();

                List<PCETestsDetails> listdetails = new();

                foreach (SplInfoDetallePce item in dataDetails)
                {

                    listdetails.Add(new PCETestsDetails()
                    {

                        CorrienteIRMS = item.CorrienteIrms,
                        NominalKV = item.NominalKv,
                        Corregidas20KW = item.PerdidasCorr20,
                        PerdidasKW = item.PerdidasKw,
                        PerdidasOndaKW = item.PerdidasOnda,
                        PorcentajeIexc = item.PorcIexc,
                        PorcentajeVN = item.PorcVn,
                        TensionKVAVG = item.TensionAvg,
                        TensionKVRMS = item.TensionRms

                    });
                }
                foreach (SplInfoSeccionPce item in dataSeccion)
                {
                    listHeaders.Add(new PCETests()
                    {

                        Date = item.FechaPrueba,
                        Temperatura = item.Temperatura,
                        UmTemperatura = item.UmTemp,
                        Capacidad = item.CapMin,
                        UmCapacidad = item.UmCapmin,
                        Frecuencia = item.Frecuencia,
                        PosPruebaAT = item.PosAt,
                        PosPruebaBT = item.PosBt,
                        PosPruebaTER = item.PosTer,
                        UmFrecuencia = item.UmFrec,
                        UmVoltajeBase = item.UmVolbase,
                        VoltajeBase = item.VoltajeBase,
                        IFin = Convert.ToInt32(item.VnFin),
                        IInicio = Convert.ToInt32(item.VnInicio),
                        Intervalo = Convert.ToInt32(item.VnIntervalo),
                        GarCExcitacion = data.GarantiaCexitacion,
                        GarPerdidas = data.GarantiaPerdidas,
                        UmGarCExcitacion = data.UmGarcexit,
                        UmGarPerdidas = data.UmGarperd,
                        PCETestsDetails = listdetails

                    }); ;

                }

                PCETestsGeneral infoGenPCE = new()
                {

                    IdLoad = Convert.ToInt64(data.IdRep),
                    LoadDate = data.FechaRep,
                    TestNumber = Convert.ToInt32(data.NoPrueba),
                    SerialNumber = data.NoSerie,
                    LanguageKey = data.ClaveIdioma,
                    Customer = data.Cliente,
                    Capacity = data.Capacidad,
                    Result = data.Resultado,
                    NameFile = data.NombreArchivo,
                    File = data.Archivo,
                    TypeReport = data.TipoReporte,
                    KeyTest = data.ClavePrueba,
                    EnergizedWinding = data.DevEnergizado,

                    Creadopor = data.Creadopor,
                    Comment = data.Comentario,

                    Fechacreacion = data.Fechacreacion,
                    Modificadopor = data.Modificadopor,
                    Fechamodificacion = data.Fechamodificacion,
                    Data = listHeaders
                };

                return Task.FromResult(infoGenPCE);
            }

            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
        }

        public Task<long> SaveInfoPCEReport(PCETestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoSeccionPce> listHeaders = new();
                SplInfoGeneralPce data = _dbContext.SplInfoGeneralPces.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralPce infoGenPCE = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    DevEnergizado = report.EnergizedWinding,
                    GarantiaCexitacion = report.Data.FirstOrDefault().GarCExcitacion,
                    GarantiaPerdidas = report.Data.FirstOrDefault().GarPerdidas,
                    UmGarcexit = report.Data.FirstOrDefault().UmGarCExcitacion,
                    UmGarperd = report.Data.FirstOrDefault().UmGarPerdidas,
                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralPces.Add(infoGenPCE) : _dbContext.SplInfoGeneralPces.Update(infoGenPCE);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenPCE.IdRep;

                List<SplInfoDetallePce> listdetails = new();

                foreach (PCETests obj in report.Data)
                {
                    int pos = report.Data.IndexOf(obj);

                    foreach (PCETestsDetails item in obj.PCETestsDetails)
                    {
                        int renglon = obj.PCETestsDetails.IndexOf(item);
                        listdetails.Add(new SplInfoDetallePce()
                        {
                            IdRep = idCarga,
                            Seccion = pos + 1,
                            Renglon = renglon + 1,
                            CorrienteIrms = item.CorrienteIRMS,
                            NominalKv = item.NominalKV,
                            PerdidasCorr20 = item.Corregidas20KW,
                            PerdidasKw = item.PerdidasKW,
                            PerdidasOnda = item.PerdidasOndaKW,
                            PorcIexc = item.PorcentajeIexc,
                            PorcVn = item.PorcentajeVN,
                            TensionAvg = item.TensionKVAVG,
                            TensionRms = item.TensionKVRMS

                        });
                    }

                    listHeaders.Add(new SplInfoSeccionPce()
                    {
                        IdRep = idCarga,
                        Seccion = pos + 1,
                        FechaPrueba = obj.Date,
                        Temperatura = Math.Round(obj.Temperatura, 1),
                        UmTemp = obj.UmTemperatura,
                        CapMin = obj.Capacidad,
                        UmCapmin = obj.UmCapacidad,
                        Frecuencia = obj.Frecuencia,
                        PosAt = obj.PosPruebaAT,
                        PosBt = obj.PosPruebaBT,
                        PosTer = obj.PosPruebaTER,
                        UmFrec = obj.UmFrecuencia,
                        UmVolbase = obj.UmVoltajeBase,
                        VoltajeBase = obj.VoltajeBase,
                        VnFin = obj.IFin,
                        VnInicio = obj.IInicio,
                        VnIntervalo = obj.Intervalo

                    });
                }

                SplInfoDetallePce datadetail = _dbContext.SplInfoDetallePces.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetallePces.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetallePces.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                SplInfoSeccionPce datasec = _dbContext.SplInfoSeccionPces.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datasec is null)
                    _dbContext.SplInfoSeccionPces.AddRange(listHeaders);
                else
                    _dbContext.SplInfoSeccionPces.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(SaveInfoPCEReport));

            }
        }

        #endregion

        #region PCI

        public async Task<PCITestGeneral> GetInfoPCIReport(string nroSerie, string keyTest, bool result)
        {
            try
            {
                SplInfoGeneralPci data = await _dbContext.SplInfoGeneralPcis
                    .AsNoTracking()
                    .Where(e => e.NoSerie == nroSerie
                        && e.ClavePrueba == keyTest
                        && e.Resultado == result)
                    .OrderByDescending(e => e.IdRep)
                    .FirstOrDefaultAsync()
                    ?? throw new ArgumentException("El reporte para esas coincidencias no se encuentra registrado.");

                PCITestGeneral test = new()
                {
                    IdLoad = Convert.ToInt64(data.IdRep),
                    LoadDate = data.FechaRep,
                    TypeReport = data.TipoReporte,
                    KeyTest = data.ClavePrueba,
                    LanguageKey = data.ClaveIdioma,
                    TestNumber = Convert.ToInt32(data.NoPrueba),
                    Customer = data.Cliente,
                    Capacity = data.Capacidad,
                    SerialNumber = data.NoSerie,
                    Comment = data.Comentario,
                    Result = data.Resultado,
                    NameFile = data.NombreArchivo,
                    File = data.Archivo,
                    Creadopor = data.Creadopor,
                    Fechacreacion = data.Fechacreacion,
                    Modificadopor = data.Modificadopor,
                    Fechamodificacion = data.Fechamodificacion,
                    Ratings = _dbContext.SplInfoSeccionPcis
                        .AsNoTracking()
                        .Where(e => e.IdRep == data.IdRep)
                        .Select(e => new PCITestRating()
                        {
                            BaseRating = e.CapPrueba,
                            //UmCapPrueba = e.UmCapPrueba,
                            Frequency = e.Frecuencia,
                            //UmFrec = e.UmFrec,
                            Temperature = e.Temp,
                            //UmTemp = e.UmTemp,
                            //TapPosition = e.NuevoCampo
                            ResistanceTemperature = e.TempRespri,
                            //TempRessec = e.TempRessec, Se eliminará?
                            //TempPerd = e.TempPerd, Se eliminará?
                            SecondaryPositions = _dbContext.SplInfoDetallePcis
                                .AsNoTracking()
                                .Where(e => e.IdRep == e.IdRep)
                                .Select(item => new PCISecondaryPositionTest()
                                {
                                    //Current = item.Corriente,
                                    CurrentIrms = item.CorrienteIrms,
                                    //Vnon = item.VnomPri,
                                    //Vnon_sec = item.VnomSec,
                                    //Inon_pi = item.InomPri,
                                    //Inon_sec = item.InomSec,
                                    //Wcu_cor = item.WcuCorr,
                                    //I2r_pri = item.I2rPri,
                                    //I2r_sec = item.I2rSec,
                                    //I2r_lv = Convert.ToDecimal(item.I2rLv),
                                    //I2r_tot = item.I2rTot,
                                    //I2r_tot_corr = Convert.ToDecimal(item.I2rTotCorr),
                                    //Wind_cor = item.WindCorr,
                                    //PorcentajeR = item.PorcR,
                                    //VoltagekVrms = item.TensionRms,
                                    //PorcentajeZ = item.PorcZ,
                                    //PorcentajeX = item.PorcX,
                                    //Wfe_20 = item.Wfe20,
                                    //LossesTotal = item.PerdTotal,
                                    //LossesCorrected = item.PerdCorregidas,
                                    //Lostkv = item.PerdidasKw,
                                    //PowerKW = item.Potencia,
                                    //Voltage = item.Tension,
                                    //VoltageAVG = item.TensionAvg,
                                    //Wind = item.Wind,
                                    //Wfe_Sen = item.WfeSen,
                                    //Position = item.PosSec
                                })
                                .ToList()
                        })
                        .ToList()
                };

                return test;
            }

            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
        }

        public async Task<CheckInfoROD> CheckInfoROD(string pNoSerie, string pWindingMaterial, string pAtPositions, string pBtPositions, string pTerPositions,
            bool pIsAT, bool pIsBT, bool pIsTer)
        {
            SqlParameter[] parametros =
            {
                //if (serial != )
                Methods.CrearSqlParameter("@POSICIONES_AT", SqlDbType.VarChar, pAtPositions),
                Methods.CrearSqlParameter("@POSICIONES_BT", SqlDbType.VarChar, pBtPositions),
                Methods.CrearSqlParameter("@POSICIONES_TER", SqlDbType.VarChar, pTerPositions),
                Methods.CrearSqlParameter("@NO_SERIE", SqlDbType.VarChar, pNoSerie),
                Methods.CrearSqlParameter("@DEVANADO", SqlDbType.VarChar, pWindingMaterial),
                Methods.CrearSqlParameter("@IS_AT", SqlDbType.VarChar, pIsAT),
                Methods.CrearSqlParameter("@IS_BT", SqlDbType.VarChar, pIsBT),
                Methods.CrearSqlParameter("@IS_TER", SqlDbType.VarChar, pIsTer),
            };

            //EJECUTAR CONSULTA
            IEnumerable<CheckInfoROD> result = Methods.EjecutarSP<CheckInfoROD>("[dbo].[CHECKINFO_ROD]", parametros, _dbContext.Database.GetConnectionString());

            // return result.ToList();

            return await Task.FromResult(result.ToList().FirstOrDefault());
        }

        public async Task<CheckInfoROD> CheckInfoPCE(string pNoSerie, List<int> pCapacity, string pAtPositions, string pBtPositions, string pTerPositions, bool pIsAT, bool pIsBT, bool pIsTer)
        {
            DataTable Capacidades = new();
            _ = Capacidades.Columns.Add("Capacity", typeof(int));

            foreach (int item in pCapacity)
            {
                DataRow Row = Capacidades.NewRow();
                Row["Capacity"] = item;
                Capacidades.Rows.Add(Row);
            }

            SqlParameter[] parametros =
           {
                //if (serial != )
                Methods.CrearSqlParameter("@POSICIONES_AT", SqlDbType.VarChar, pAtPositions),
                Methods.CrearSqlParameter("@POSICIONES_BT", SqlDbType.VarChar, pBtPositions),
                Methods.CrearSqlParameter("@POSICIONES_TER", SqlDbType.VarChar, pTerPositions),
                Methods.CrearSqlParameter("@NO_SERIE", SqlDbType.VarChar, pNoSerie),
                Methods.CrearSqlParameter("@IS_AT", SqlDbType.VarChar, pIsAT),
                Methods.CrearSqlParameter("@IS_BT", SqlDbType.VarChar, pIsBT),
                Methods.CrearSqlParameter("@IS_TER", SqlDbType.VarChar, pIsTer),
                new SqlParameter("@LISTA", SqlDbType.Structured) { Value = Capacidades, TypeName = "CapacityType" }
            };

            //EJECUTAR CONSULTA
            IEnumerable<CheckInfoROD> result = Methods.EjecutarSP<CheckInfoROD>("[dbo].[CHECKINFO_PCE]", parametros, _dbContext.Database.GetConnectionString());

            // return result.ToList();

            return await Task.FromResult(result.ToList().FirstOrDefault());
        }

        public Task<long> SaveInfoPCIReport(PCITestGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();

            try
            {
                List<SplInfoSeccionPci> listHeaders = new();

                SplInfoGeneralPci data = _dbContext.SplInfoGeneralPcis
                    .AsNoTracking()
                    .FirstOrDefault(CE => CE.IdRep == report.IdLoad);

                decimal idCarga;

                SplInfoGeneralPci infoGenPCE = new()
                {
                    IdRep = report.IdLoad,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    ClaveIdioma = report.LanguageKey,
                    NoPrueba = report.TestNumber,
                    FechaRep = report.Date,
                    FechaPrueba = DateTime.Today,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    NoSerie = report.SerialNumber,
                    CapRedBaja = report.ReducedCapacityAtLowVoltage ? "Si" : "No",
                    MaterialDevanado = report.WindingMaterial,
                    Autotransformador = report.Autotransformer ? "Si" : "No",
                    Monofasico = report.Monofasico ? "Si" : "No",
                    PosPri = report.Ratings
                        .Select(e => e.TapPosition)
                        .FirstOrDefault(),
                    PosSec = report.Ratings
                        .Select(e => e.SecondaryPositions.Select(s => s.TapPosition).FirstOrDefault())
                        .FirstOrDefault(),
                    Kwcu = report.Kwcu,
                    KwcuMva = report.KwcuMva,
                    Kwtot100 = report.Kwtot100,
                    Kwtot100M = report.Kwtot100M,
                    Comentario = report.Comment,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    Creadopor = report.Creadopor,
                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion
                };

                _ = data is null
                    ? _dbContext.SplInfoGeneralPcis.Add(infoGenPCE)
                    : _dbContext.SplInfoGeneralPcis.Update(infoGenPCE);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenPCE.IdRep;

                List<SplInfoDetallePci> listdetails = new();

                foreach (PCITestRating rating in report.Ratings)
                {
                    int pos = report.Ratings.IndexOf(rating);

                    listHeaders.Add(new SplInfoSeccionPci()
                    {
                        IdRep = idCarga,
                        Seccion = pos + 1,
                        CapPrueba = rating.BaseRating,
                        UmCapPrueba = rating.UmBaseRating,
                        OverElevation = rating.TemperatureElevation,
                        //UmOverElevation = rating.UmTemperatureElevation,
                        //TapPosition = rating.TapPosition, /*NuevoCampo*/
                        Frecuencia = rating.Frequency,
                        UmFrec = rating.UmFrequency,
                        Temp = Math.Round(rating.Temperature, 0),
                        UmTemp = rating.UmTemperature,
                        TempRespri = rating.ResistanceTemperature,
                        TempRessec = report.Ratings
                            .Select(e => e.SecondaryPositions.Select(s => s.ResistanceTemperature).FirstOrDefault())
                            .FirstOrDefault(),
                        FacCorrPri = report.Ratings
                            .Select(e => e.SecondaryPositions.Select(s => s.PrimaryReductionCorrectionFactor).FirstOrDefault())
                            .FirstOrDefault(),
                        FacCorrSec = report.Ratings
                            .Select(e => e.SecondaryPositions.Select(s => s.SecondaryReductionCorrectionFactor).FirstOrDefault())
                            .FirstOrDefault(),
                        //AverageResistance = rating.AverageResistance, /*NuevoCampo*/
                        //Tension = rating.Tension, /*NuevoCampo*/
                        FacCorrSee = report.Ratings
                            .Select(e => e.SecondaryPositions.Select(s => s.ElevationCorrectionFactor).FirstOrDefault())
                            .FirstOrDefault(),
                    });

                    foreach (PCISecondaryPositionTest item in rating.SecondaryPositions)
                    {
                        int renglon = rating.SecondaryPositions.IndexOf(item);

                        listdetails.Add(new SplInfoDetallePci()
                        {
                            IdRep = idCarga,
                            Seccion = pos + 1,
                            Renglon = renglon + 1,
                            PosPri = rating.Position,
                            PosSec = item.Position,
                            Corriente = item.CurrentIrms,
                            CorrienteIrms = item.CurrentIrms,
                            Tension = item.Voltage,
                            TensionRms = item.VoltagekVrms,
                            Potencia = item.Power,
                            PotenciaKw = item.PowerKW,
                            ResisPri = rating.AverageResistance,
                            ResisSec = item.AverageResistance,
                            TensionPri = rating.Tension,
                            TensionSec = item.Tension,
                            VnomPri = item.VnomPrimary,
                            VnomSec = item.VnomSecondary,
                            InomPri = item.InomPrimary,
                            InomSec = item.InomSecondary,
                            I2rPri = item.I2RPrimary,
                            I2rSec = item.I2RSecondary > 999999m ? 999999m : item.I2RSecondary,
                            I2rTot = item.I2RTotal > 999999m ? 999999m : item.I2RTotal,
                            I2rTotCorr = item.I2RCorrectedTotal > 999999m ? 999999m : item.I2RCorrectedTotal,
                            WcuCorr = item.WcuCor > 99999999m ? 99999999m : item.WcuCor,
                            Wind = item.Wind > 999999m ? 999999m : item.Wind,
                            WindCorr = item.WindCorr > 999999m ? 999999m : item.WindCorr,
                            Wcu = item.WCu > 999999m ? 999999m : item.WCu,
                            PorcR = item.PercentageR > 999m ? 999m : item.PercentageR,
                            PorcZ = item.PercentageZ > 999m ? 999m : item.PercentageZ,
                            PorcX = item.PercentageX > 999m ? 999m : item.PercentageX,
                            Wfe20 = item.Corregidas20KW,
                            PerdTotal = item.LossesTotal,
                            PerdCorregidas = item.LossesCorrected > 999 ? 999 : item.LossesCorrected,
                        });
                    }
                }

                SplInfoSeccionPci datasec = _dbContext.SplInfoSeccionPcis
                    .AsNoTracking()
                    .FirstOrDefault(e => e.IdRep == idCarga);

                if (datasec is null)
                    _dbContext.SplInfoSeccionPcis.AddRange(listHeaders);
                else
                    _dbContext.SplInfoSeccionPcis.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                SplInfoDetallePci datadetail = _dbContext.SplInfoDetallePcis
                    .AsNoTracking()
                    .FirstOrDefault(e => e.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetallePcis.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetallePcis.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();

                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }

                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message + " " + ex.InnerException.Message);
            }
        }

        public async Task<IEnumerable<PCIParameters>> AAsync(
            string pNoSerie,
            string pWindingMaterial,
            List<int> pCapacity,
            string pAtPositions,
            string pBtPositions,
            string pTerPositions,
            bool pIsAT,
            bool pIsBT,
            bool pIsTer)
        {
            IEnumerable<InfoROD> rods = Methods.EjecutarSP<InfoROD>("[dbo].[GET_INFO_ROD]", new SqlParameter[]
                {
                    Methods.CrearSqlParameter("@POSICIONES_AT", SqlDbType.VarChar, pAtPositions),
                    Methods.CrearSqlParameter("@POSICIONES_BT", SqlDbType.VarChar, pBtPositions),
                    Methods.CrearSqlParameter("@POSICIONES_TER", SqlDbType.VarChar, pTerPositions),
                    Methods.CrearSqlParameter("@NO_SERIE", SqlDbType.VarChar, pNoSerie),
                    Methods.CrearSqlParameter("@IS_AT", SqlDbType.VarChar, pIsAT),
                    Methods.CrearSqlParameter("@IS_BT", SqlDbType.VarChar, pIsBT),
                    Methods.CrearSqlParameter("@IS_TER", SqlDbType.VarChar, pIsTer),
                    Methods.CrearSqlParameter("@DEVANADO", SqlDbType.VarChar, pWindingMaterial)
                },
                _dbContext.Database.GetConnectionString());

            IEnumerable<InfoPCE> pces = Methods.EjecutarSP<InfoPCE>("[dbo].[GET_INFO_PCE]", new SqlParameter[]
                {
                    Methods.CrearSqlParameter("@POSICIONES_AT", SqlDbType.VarChar, pAtPositions),
                    Methods.CrearSqlParameter("@POSICIONES_BT", SqlDbType.VarChar, pBtPositions),
                    Methods.CrearSqlParameter("@POSICIONES_TER", SqlDbType.VarChar, string.IsNullOrEmpty(pTerPositions)? DBNull.Value: pTerPositions),
                    Methods.CrearSqlParameter("@NO_SERIE", SqlDbType.VarChar, pNoSerie),
                    Methods.CrearSqlParameter("@IS_AT", SqlDbType.VarChar, pIsAT),
                    Methods.CrearSqlParameter("@IS_BT", SqlDbType.VarChar, pIsBT),
                    Methods.CrearSqlParameter("@IS_TER", SqlDbType.VarChar, pIsTer),
                    Methods.CrearSqlParameter("@Capacidades", SqlDbType.VarChar, string.Join(',', pCapacity.Select(e=> e * 1000m)))
                }, _dbContext.Database.GetConnectionString());

            List<PCIParameters> result = new();

            return await Task.FromResult(pces.Select(pce => new PCIParameters()
                {
                    PrimaryWinding = pce.Dev_pri,
                    PrimaryPosition = pce.Pos_pri,
                    PrimaryTemperature = rods.Where(r => r.Devanado == pce.Dev_pri && r.Posicion == pce.Pos_pri)
                            .Select(r => r.Temperatura)
                            .FirstOrDefault(),
                    PrimaryAverageResistance = rods.Where(r => r.Devanado == pce.Dev_pri && r.Posicion == pce.Pos_pri)
                            .Select(r => r.Resprom)
                            .FirstOrDefault(),
                    SecondaryWinding = pce.Dev_sec,
                    SecondaryPosition = pce.Pos_sec,
                    SecondaryCorrection20 = pce.Perd_corr,
                    SecondaryTemperature = rods.Where(r => r.Devanado == pce.Dev_sec && r.Posicion == pce.Pos_sec)
                            .Select(r => r.Temperatura)
                            .FirstOrDefault(),
                    SecondaryAverageResistance = rods.Where(r => r.Devanado == pce.Dev_sec && r.Posicion == pce.Pos_sec)
                            .Select(r => r.Resprom)
                            .FirstOrDefault(),
                })
                .ToArray());
        }

        #endregion

        public Task<long> SaveInfoPLRReport(PLRTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoDetallePlr> listHeaders = new();
                SplInfoGeneralPlr data = _dbContext.SplInfoGeneralPlrs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralPlr infoGenPLR = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    CantTensiones = report.AmountOfTensions,
                    CantTiempos = report.AmountOfTime,
                    FechaPrueba = report.LoadDate,
                    PorcDevtn = report.Data.PorcDeviationNV,
                    Rldtn = report.Data.Rldnt,
                    TensionNominal = report.Data.NominalVoltage,
                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralPlrs.Add(infoGenPLR) : _dbContext.SplInfoGeneralPlrs.Update(infoGenPLR);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenPLR.IdRep;

                List<SplInfoDetallePlr> listdetails = new();

                foreach (PLRTestsDetails item in report.Data.PLRTestsDetails)
                {
                    int renglon = report.Data.PLRTestsDetails.IndexOf(item);
                    listdetails.Add(new SplInfoDetallePlr()
                    {
                        IdRep = idCarga,
                        Renglon = renglon + 1,
                        Corriente = item.Current,
                        PorcDesv = item.PorcD,
                        Reactancia = item.Reactance,
                        Tension = item.Tension,
                        Tiempo = item.Time
                    });
                }

                SplInfoDetallePlr datadetail = _dbContext.SplInfoDetallePlrs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetallePlrs.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetallePlrs.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(SaveInfoPLRReport));

            }
        }

        public Task<long> SaveInfoPRDReport(PRDTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoDetallePrd> listHeaders = new();
                SplInfoGeneralPrd data = _dbContext.SplInfoGeneralPrds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralPrd infoGenPRD = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,

                    FechaPrueba = report.LoadDate,

                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    VoltajeNominal = report.PRDTests.NominalVoltage,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralPrds.Add(infoGenPRD) : _dbContext.SplInfoGeneralPrds.Update(infoGenPRD);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenPRD.IdRep;

                List<SplInfoDetallePrd> listdetails = new()
                {
                    new SplInfoDetallePrd()
                    {
                        IdRep = idCarga,
                        C4F = report.PRDTests.PRDTestsDetails.C4,
                        CapKvar = report.PRDTests.PRDTestsDetails.Cap,
                        CnF = report.PRDTests.PRDTestsDetails.Cn,
                        Fc = report.PRDTests.PRDTestsDetails.Fc,
                        Fc2 = report.PRDTests.PRDTestsDetails.Fc2,
                        LxpH = report.PRDTests.PRDTestsDetails.Lxp,
                        M3H = report.PRDTests.PRDTestsDetails.M3,
                        PjmcW = report.PRDTests.PRDTestsDetails.Pjmc,
                        PjmW = report.PRDTests.PRDTestsDetails.Pjm,
                        PeW = report.PRDTests.PRDTestsDetails.Pe,
                        PtW = report.PRDTests.PRDTestsDetails.Pt,
                        TmC = report.PRDTests.PRDTestsDetails.Tm,
                        TmpC = report.PRDTests.PRDTestsDetails.Tmp,
                        VmV = report.PRDTests.PRDTestsDetails.Vm,
                        U = report.PRDTests.PRDTestsDetails.U,
                        TrC = report.PRDTests.PRDTestsDetails.Tr,
                        R4sOhms = report.PRDTests.PRDTestsDetails.R4s,
                        RmOhms = report.PRDTests.PRDTestsDetails.Rm,
                        GarantiaW = report.PRDTests.PRDTestsDetails.Warranty,
                        PorcDesv = report.PRDTests.PRDTestsDetails.PorcDesv,
                        PfeW = report.PRDTests.PRDTestsDetails.Pfe,
                        XcOhms = report.PRDTests.PRDTestsDetails.Xc,
                        XmOhms = report.PRDTests.PRDTestsDetails.Xm,
                        RxpOhms = report.PRDTests.PRDTestsDetails.Rxp,
                        IAmps = report.PRDTests.PRDTestsDetails.I,
                        ImA = report.PRDTests.PRDTestsDetails.Im,
                        PW = report.PRDTests.PRDTestsDetails.P,
                        VVolts = report.PRDTests.PRDTestsDetails.V

                    }
                };

                SplInfoDetallePrd datadetail = _dbContext.SplInfoDetallePrds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetallePrds.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetallePrds.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(SaveInfoPRDReport));

            }
        }

        public Task<long> SaveInfoPEEReport(PEETestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoDetallePee> listHeaders = new();
                SplInfoGeneralPee data = _dbContext.SplInfoGeneralPees.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralPee infoGenPEE = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,

                    FechaPrueba = report.LoadDate,

                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralPees.Add(infoGenPEE) : _dbContext.SplInfoGeneralPees.Update(infoGenPEE);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenPEE.IdRep;

                List<SplInfoDetallePee> listdetails = new();

                foreach (PEETestsDetails item in report.PEETests.PEETestsDetails)
                {
                    int renglon = report.PEETests.PEETestsDetails.IndexOf(item);
                    listdetails.Add(new SplInfoDetallePee()
                    {
                        IdRep = idCarga,
                        Renglon = renglon + 1,
                        CoolingType = item.CoolingType,
                        CorrienteRms = item.CurrentRMS,
                        KwauxGar = item.Kwaux_gar,
                        MvaauxGar = item.Mvaaux_gar,
                        PotenciaKw = item.PowerKW,
                        TensionRms = item.VoltageRMS

                    });
                }

                SplInfoDetallePee datadetail = _dbContext.SplInfoDetallePees.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetallePees.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetallePees.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(SaveInfoPEEReport));

            }
        }

        public Task<long> SaveInfoISZReport(ISZTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoDetalleIsz> listHeaders = new();
                List<SplInfoSeccionIsz> listsecciones = new();
                SplInfoGeneralIsz data = _dbContext.SplInfoGeneralIszs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralIsz infoGenISZ = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    CantNeutros = report.Data.QtyNeutral,
                    GradosCorr = report.Data.DegreesCor,
                    MaterialDevanado = report.Data.MaterialWinding,
                    PosAt = report.Data.PosAT,
                    PosBt = report.Data.PosBT,
                    PosTer = report.Data.PosTER,
                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,
                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralIszs.Add(infoGenISZ) : _dbContext.SplInfoGeneralIszs.Update(infoGenISZ);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenISZ.IdRep;

                /****************************************/
                var splirDev = report.Data.WindingEnergized.Split(",");
                var splirImpedancia = report.Data.ImpedanceGar.Split(",");
                decimal impedancia1 = 0;
                decimal impedancia2 = 0;
                decimal impedancia3 = 0;
                _ = decimal.TryParse(splirImpedancia[0], out impedancia1);
                if (report.KeyTest == "ABT")
                {
                    _ = decimal.TryParse(splirImpedancia[1], out impedancia2);
                    _ = decimal.TryParse(splirImpedancia[2], out impedancia3);
                }

                decimal factorCoor = report.Data.SeccionesISZTestsDetails[0].ISZTestsDetails[0].FactorCorr;

                listsecciones.Add(new SplInfoSeccionIsz
                {
                    IdRep = idCarga,
                    IdSeccion = 1,
                    DevEnergizado = splirDev[0],
                    CapBase = report.Data.CapBaseMin,
                    UmcapBase = report.Data.UmCapBaseMin,
                    Temperatura = report.Data.Temperature,
                    UmTemp = report.Data.UmTemperature,
                    FactorCorr = factorCoor,
                    ImpedanciaGar = impedancia1,
                    PorcJx = report.KeyTest == "ABT" ? report.Data.SeccionesISZTestsDetails[0].Porc_X : null,
                    PorcR = report.KeyTest == "ABT" ? report.Data.SeccionesISZTestsDetails[0].Porc_R : null,
                    PorcZ = report.KeyTest == "ABT" ? report.Data.SeccionesISZTestsDetails[0].Porc_Z : null
                });
                if (report.KeyTest == "ABT")
                {

                    listsecciones.Add(new SplInfoSeccionIsz
                    {
                        IdRep = idCarga,
                        IdSeccion = 2,
                        DevEnergizado = splirDev[1],
                        CapBase = report.Data.CapBaseMin,
                        UmcapBase = report.Data.UmCapBaseMin,
                        Temperatura = report.Data.Temperature,
                        UmTemp = report.Data.UmTemperature,
                        FactorCorr = factorCoor,
                        ImpedanciaGar = impedancia2,
                        PorcJx = report.Data.SeccionesISZTestsDetails[1].Porc_X,
                        PorcR = report.Data.SeccionesISZTestsDetails[1].Porc_R,
                        PorcZ = report.Data.SeccionesISZTestsDetails[1].Porc_Z
                    });
                    listsecciones.Add(new SplInfoSeccionIsz
                    {
                        IdRep = idCarga,
                        IdSeccion = 3,
                        DevEnergizado = splirDev[2],
                        CapBase = report.Data.CapBaseMin,
                        UmcapBase = report.Data.UmCapBaseMin,
                        Temperatura = report.Data.Temperature,
                        UmTemp = report.Data.UmTemperature,
                        FactorCorr = factorCoor,
                        ImpedanciaGar = impedancia3,
                        PorcJx = report.Data.SeccionesISZTestsDetails[2].Porc_X,
                        PorcR = report.Data.SeccionesISZTestsDetails[2].Porc_R,
                        PorcZ = report.Data.SeccionesISZTestsDetails[2].Porc_Z
                    });
                }

                _dbContext.SplInfoSeccionIszs.AddRange(listsecciones);
                _ = _dbContext.SaveChanges();


                /***************************************/


                //transaction.Commit();
                List<SplInfoDetalleIsz> listdetails = new();
                int seccion = 1;
                int renglon = 1;
                foreach (var item in report.Data.SeccionesISZTestsDetails)
                {
                    foreach (var item2 in item.ISZTestsDetails)
                    {
                        listdetails.Add(new SplInfoDetalleIsz
                        {

                            IdRep = idCarga,
                            Renglon = renglon,
                            IdSeccion = seccion,
                            Posicion1 = item2.Position1,
                            Posicion2 = item2.Position2,
                            Tension1 = item2.Voltage1,
                            Tension2 = item2.Voltage2,
                            TensionVrms = item2.VoltsVRMS,
                            CorrienteIrms = item2.CurrentsIRMS,
                            PotenciaCorrKw = item2.PowerKWVoltage1,
                            PotenciaKw = item2.PowerKW,
                            PorcJxo = item2.PercentagejXo,
                            PorcRo = item2.PercentageRo,
                            PorcZo = item2.PercentageZo,
                            ZBase = item2.ZBase,
                            ZOhms = item2.ZOhms
                        });
                        renglon++;

                    }
                    seccion++;
                    renglon = 1;
                }



                SplInfoDetalleIsz datadetail = _dbContext.SplInfoDetalleIszs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleIszs.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleIszs.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<long> SaveInfoRYEReport(RYETestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoRegRye> listHeaders = new();
                SplInfoGeneralRye data = _dbContext.SplInfoGeneralRyes.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralRye infoGenRYE = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.Data.Date,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    NoConexionesAt = report.NoConnectiosAT == 0 ? null : report.NoConnectiosAT,
                    NoConexionesBt = report.NoConnectiosBT == 0 ? null : report.NoConnectiosBT,
                    NoConexionesTer = report.NoConnectiosTER == 0 ? null : report.NoConnectiosTER,
                    TensionAt = report.VoltageAT,
                    TensionBt = report.VoltageBT,
                    TensionTer = report.VoltageTER,
                    CoolingType = report.CoolingType,
                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralRyes.Add(infoGenRYE) : _dbContext.SplInfoGeneralRyes.Update(infoGenRYE);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenRYE.IdRep;

                List<SplInfoDetalleRye> listdetails = new();

                listHeaders.Add(new SplInfoRegRye()
                {
                    IdRep = idCarga,

                    FechaPrueba = report.Data.Date,
                    Capacidad = report.Data.Capacity,
                    FactPot1 = report.Data.FactPot1,
                    FactPot2 = report.Data.FactPot2,
                    FactPot3 = report.Data.FactPot3,
                    FactPot4 = report.Data.FactPot4,
                    FactPot5 = report.Data.FactPot5,
                    FactPot6 = report.Data.FactPot6,
                    FactPot7 = report.Data.FactPot7,
                    PorcReg1 = report.Data.PorcReg1,
                    PorcReg2 = report.Data.PorcReg2,
                    PorcReg3 = report.Data.PorcReg3,
                    PorcReg4 = report.Data.PorcReg4,
                    PorcReg5 = report.Data.PorcReg5,
                    PorcReg6 = report.Data.PorcReg6,
                    PorcReg7 = report.Data.PorcReg7,
                    PorcR = report.Data.PorcR,
                    PorcX = report.Data.PorcX,
                    PorcZ = report.Data.PorcZ,
                    ValorG = report.Data.ValueG,
                    ValorW = report.Data.ValueW,
                    XEntreR = report.Data.XIntoR,
                    PerdidaCarga = report.Data.Lostload,
                    PerdidaEnf = report.Data.LostCooldown,
                    PerdidaTotal = report.Data.TotalLosses,
                    PerdidaVacio = report.Data.EmptyLosses,

                });

                foreach (RYETestsDetails obj in report.Data.RYETestsDetails)
                {
                    int pos = report.Data.RYETestsDetails.IndexOf(obj);

                    listdetails.Add(new SplInfoDetalleRye()
                    {
                        IdRep = idCarga,

                        Renglon = pos + 1,
                        Eficiencia1 = obj.Efficiency1,
                        Eficiencia2 = obj.Efficiency2,
                        Eficiencia3 = obj.Efficiency3,
                        Eficiencia4 = obj.Efficiency4,
                        Eficiencia5 = obj.Efficiency5,
                        Eficiencia6 = obj.Efficiency6,
                        Eficiencia7 = obj.Efficiency7,
                        PorcMva = obj.PercentageMVA

                    });

                }

                SplInfoDetalleRye datadetail = _dbContext.SplInfoDetalleRyes.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleRyes.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleRyes.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                SplInfoRegRye datasec = _dbContext.SplInfoRegRyes.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datasec is null)
                    _dbContext.SplInfoRegRyes.AddRange(listHeaders);
                else
                    _dbContext.SplInfoRegRyes.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<PEETestsGeneral> GetInfoPEEReport(string NroSerie, string KeyTest, bool Result)
        {
            try
            {
                List<PCETests> listHeaders = new();

                SplInfoGeneralPee data = _dbContext.SplInfoGeneralPees.AsNoTracking().Where(CE => CE.NoSerie.Equals(NroSerie) && CE.ClavePrueba.Equals(KeyTest) && CE.Resultado == Result).OrderByDescending(x => x.IdRep).FirstOrDefault();

                if (data is null)
                {
                    throw new ArgumentException("El reporte para esas coincidencias no se encuentra registrado");
                }

                List<SplInfoDetallePee> dataDetails = _dbContext.SplInfoDetallePees.AsNoTracking().Where(CE => CE.IdRep == data.IdRep).ToList();

                List<PEETestsDetails> listdetails = new();

                foreach (SplInfoDetallePee item in dataDetails)
                {

                    listdetails.Add(new PEETestsDetails()
                    {

                        CoolingType = item.CoolingType,
                        CurrentRMS = item.CorrienteRms,
                        Kwaux_gar = item.KwauxGar,
                        Mvaaux_gar = item.MvaauxGar,
                        PowerKW = item.PotenciaKw,
                        VoltageRMS = item.TensionRms,

                    });
                }

                PEETests tests = new()
                {

                    KeyTest = data.ClavePrueba,
                    PEETestsDetails = listdetails,

                };

                PEETestsGeneral infoGenPEE = new()
                {

                    IdLoad = Convert.ToInt64(data.IdRep),
                    LoadDate = data.FechaRep,
                    TestNumber = Convert.ToInt32(data.NoPrueba),
                    SerialNumber = data.NoSerie,
                    LanguageKey = data.ClaveIdioma,
                    Customer = data.Cliente,
                    Capacity = data.Capacidad,
                    Result = data.Resultado,
                    NameFile = data.NombreArchivo,
                    File = data.Archivo,
                    TypeReport = data.TipoReporte,
                    KeyTest = data.ClavePrueba,
                    Creadopor = data.Creadopor,
                    Comment = data.Comentario,
                    Fechacreacion = data.Fechacreacion,
                    Modificadopor = data.Modificadopor,
                    Fechamodificacion = data.Fechamodificacion,
                    PEETests = tests,

                };

                return Task.FromResult(infoGenPEE);
            }

            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
        }

        public Task<long> SaveInfoPIRReport(RYETestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoRegRye> listHeaders = new();
                SplInfoGeneralRye data = _dbContext.SplInfoGeneralRyes.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralRye infoGenRYE = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    NoConexionesAt = report.NoConnectiosAT,
                    NoConexionesBt = report.NoConnectiosBT,
                    NoConexionesTer = report.NoConnectiosTER,
                    TensionAt = report.VoltageAT,
                    TensionBt = report.VoltageBT,
                    TensionTer = report.VoltageTER,
                    CoolingType = report.CoolingType,
                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralRyes.Add(infoGenRYE) : _dbContext.SplInfoGeneralRyes.Update(infoGenRYE);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenRYE.IdRep;

                List<SplInfoDetalleRye> listdetails = new();

                listHeaders.Add(new SplInfoRegRye()
                {
                    IdRep = idCarga,

                    FechaPrueba = report.Data.Date,
                    Capacidad = report.Data.Capacity,
                    FactPot1 = report.Data.FactPot1,
                    FactPot2 = report.Data.FactPot2,
                    FactPot3 = report.Data.FactPot3,
                    FactPot4 = report.Data.FactPot4,
                    FactPot5 = report.Data.FactPot5,
                    FactPot6 = report.Data.FactPot6,
                    FactPot7 = report.Data.FactPot7,
                    PorcReg1 = report.Data.PorcReg1,
                    PorcReg2 = report.Data.PorcReg2,
                    PorcReg3 = report.Data.PorcReg3,
                    PorcReg4 = report.Data.PorcReg4,
                    PorcReg5 = report.Data.PorcReg5,
                    PorcReg6 = report.Data.PorcReg6,
                    PorcReg7 = report.Data.PorcReg7,
                    PorcR = report.Data.PorcR,
                    PorcX = report.Data.PorcX,
                    PorcZ = report.Data.PorcZ,
                    ValorG = report.Data.ValueG,
                    ValorW = report.Data.ValueW,
                    XEntreR = report.Data.XIntoR,
                    PerdidaCarga = report.Data.Lostload,
                    PerdidaEnf = report.Data.LostCooldown,
                    PerdidaTotal = report.Data.TotalLosses,
                    PerdidaVacio = report.Data.EmptyLosses,

                });

                foreach (RYETestsDetails obj in report.Data.RYETestsDetails)
                {
                    int pos = report.Data.RYETestsDetails.IndexOf(obj);

                    listdetails.Add(new SplInfoDetalleRye()
                    {
                        IdRep = idCarga,

                        Renglon = pos + 1,
                        Eficiencia1 = obj.Efficiency1,
                        Eficiencia2 = obj.Efficiency2,
                        Eficiencia3 = obj.Efficiency3,
                        Eficiencia4 = obj.Efficiency4,
                        Eficiencia5 = obj.Efficiency5,
                        Eficiencia6 = obj.Efficiency6,
                        Eficiencia7 = obj.Efficiency7,
                        PorcMva = obj.PercentageMVA

                    });

                }

                SplInfoDetalleRye datadetail = _dbContext.SplInfoDetalleRyes.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleRyes.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleRyes.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                SplInfoRegRye datasec = _dbContext.SplInfoRegRyes.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datasec is null)
                    _dbContext.SplInfoRegRyes.AddRange(listHeaders);
                else
                    _dbContext.SplInfoRegRyes.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<long> SaveInfoPIMReport(PIMTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoArchivosPim> listHeaders = new();
                SplInfoGeneralPim data = _dbContext.SplInfoGeneralPims.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralPim infoGenPIM = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.Date,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    AplicaBaja = report.ApplyLow,
                    Conexion = report.Connection,
                    FechaPrueba = report.Date,
                    NivelTension = report.VoltageLevel,
                    Tension = report.VoltageLevel == null ? null : report.Voltage,
                    TotalPags = report.TotalPags,

                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralPims.Add(infoGenPIM) : _dbContext.SplInfoGeneralPims.Update(infoGenPIM);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenPIM.IdRep;

                List<SplInfoDetallePim> listdetails = new();

                foreach (Files obj in report.Files)
                {
                    int pos = report.Files.IndexOf(obj) + 1;
                    listHeaders.Add(new SplInfoArchivosPim()
                    {
                        IdRep = idCarga,
                        Orden = pos,
                        Archivo = obj.File,
                        Nombre = obj.Name
                    });
                }

                foreach (PIMTestsDetails obj in report.Data)
                {
                    int pos = report.Data.IndexOf(obj);

                    listdetails.Add(new SplInfoDetallePim()
                    {
                        IdRep = idCarga,
                        Renglon = pos + 1,
                        Pagina = obj.Page,
                        Terminal = obj.Terminal,

                    });

                }

                SplInfoDetallePim datadetail = _dbContext.SplInfoDetallePims.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetallePims.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetallePims.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                SplInfoArchivosPim datasec = _dbContext.SplInfoArchivosPims.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datasec is null)
                    _dbContext.SplInfoArchivosPims.AddRange(listHeaders);
                else
                    _dbContext.SplInfoArchivosPims.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<long> SaveInfoPIRReport(PIRTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoArchivosPir> listHeaders = new();
                SplInfoGeneralPir data = _dbContext.SplInfoGeneralPirs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;

                SplInfoGeneralPir infoGenPIR = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    IncluyeTerciario = report.IncludeTertiary,
                    Conexion = report.Connection,
                    FechaPrueba = report.Date,
                    NivelTension = report.VoltageLevel,
                    Tension = report.VoltageLevel == null ? null : report.Voltage,
                    TotalPags = report.TotalPags,

                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralPirs.Add(infoGenPIR) : _dbContext.SplInfoGeneralPirs.Update(infoGenPIR);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenPIR.IdRep;

                List<SplInfoDetallePir> listdetails = new();

                foreach (Files obj in report.Files)
                {
                    int pos = report.Files.IndexOf(obj) + 1;
                    listHeaders.Add(new SplInfoArchivosPir()
                    {
                        IdRep = idCarga,
                        Orden = pos,
                        Archivo = obj.File,
                        Nombre = obj.Name
                    });
                }

                foreach (PIRTestsDetails obj in report.Data)
                {
                    int pos = report.Data.IndexOf(obj);

                    listdetails.Add(new SplInfoDetallePir()
                    {
                        IdRep = idCarga,
                        Renglon = pos + 1,
                        Pagina = obj.Page,
                        Terminal = obj.Terminal,

                    });

                }

                SplInfoDetallePir datadetail = _dbContext.SplInfoDetallePirs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetallePirs.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetallePirs.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                SplInfoArchivosPim datasec = _dbContext.SplInfoArchivosPims.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datasec is null)
                    _dbContext.SplInfoArchivosPirs.AddRange(listHeaders);
                else
                    _dbContext.SplInfoArchivosPirs.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<FPCTestsGeneral> GetInfoFPCReport(string NroSerie, string KeyTest, string Lenguage, string UnitType, decimal Frecuency, bool Result)
        {
            try
            {
                List<FPCTests> listHeaders = new();
                SplInfoGeneralFpc data = Frecuency == 0
                    ? _dbContext.SplInfoGeneralFpcs.AsNoTracking().Where(CE => CE.NoSerie.Equals(NroSerie) && CE.ClavePrueba.Equals(KeyTest) && CE.ClaveIdioma.Equals(Lenguage) && CE.TipoUnidad.Equals(UnitType) && CE.Resultado == Result).OrderByDescending(x => x.IdRep).FirstOrDefault()
                    : _dbContext.SplInfoGeneralFpcs.AsNoTracking().Where(CE => CE.NoSerie.Equals(NroSerie) && CE.ClavePrueba.Equals(KeyTest) && CE.ClaveIdioma.Equals(Lenguage) && CE.Frecuencia == Frecuency && CE.TipoUnidad.Equals(UnitType) && CE.Resultado == Result).OrderByDescending(x => x.IdRep).FirstOrDefault();
                if (data is null)
                {
                    throw new ArgumentException("El reporte para esas coincidencias no se encuentra registrado");
                }

                List<SplInfoSeccionFpc> dataSeccion = _dbContext.SplInfoSeccionFpcs.AsNoTracking().Where(CE => CE.IdRep == data.IdRep).ToList();

                List<SplInfoDetalleFpc> dataDetails = _dbContext.SplInfoDetalleFpcs.AsNoTracking().Where(CE => CE.IdRep == data.IdRep).ToList();

                List<FPCTestsDetails> listdetails = new();

                foreach (SplInfoDetalleFpc item in dataDetails)
                {
                    listdetails.Add(new FPCTestsDetails()
                    {
                        Row = Convert.ToInt32(item.Renglon),
                        WindingA = item.DevE,
                        WindingB = item.DevT,
                        WindingC = item.DevG,
                        WindingD = item.DevUst,
                        Id = item.IdCap,
                        Current = item.Corriente,
                        Power = item.Potencia,
                        PercentageA = item.PorcFp,
                        Capacitance = item.Capacitancia,
                        PercentageC = item.CorrPorcFp,
                        PercentageB = item.TangPorcFp,
                    });
                }
                foreach (SplInfoSeccionFpc item in dataSeccion)
                {
                    listHeaders.Add(new FPCTests()
                    {
                        Date = item.FechaPrueba,
                        UmTension = item.UmTension,
                        UpperOilTemperature = item.TempAceiteSup,
                        UmTempacSup = item.UmTempacSup,
                        LowerOilTemperature = item.TempAceiteInf,
                        UmTempacInf = item.UmTempacInf,
                        CorrectionFactorSpecifications = new Domain.SPL.Configuration.CorrectionFactorSpecification() { FactorCorr = Convert.ToDecimal(item.FactorCorr) },
                        TempFP = item.TempProm,
                        TempTanD = item.TempCt,
                        AcceptanceValueCap = item.AceptCap,
                        AcceptanceValueFP = item.AcepFp,
                        FPCTestsDetails = listdetails
                    }); ;
                }

                FPCTestsGeneral infoGenPCI = new()
                {
                    IdLoad = Convert.ToInt64(data.IdRep),
                    LoadDate = data.FechaRep,
                    TestNumber = Convert.ToInt32(data.NoPrueba),
                    SerialNumber = data.NoSerie,
                    LanguageKey = data.ClaveIdioma,
                    Customer = data.Cliente,
                    Capacity = data.Capacidad,
                    Result = data.Resultado,
                    NameFile = data.NombreArchivo,
                    File = data.Archivo,
                    TypeReport = data.TipoReporte,
                    KeyTest = data.ClavePrueba,
                    Frequency = data.Frecuencia,
                    Specification = data.Especificacion,
                    TypeUnit = data.TipoUnidad,

                    Creadopor = data.Creadopor,
                    Comment = data.Comentario,

                    Fechacreacion = data.Fechacreacion,
                    Modificadopor = data.Modificadopor,
                    Fechamodificacion = data.Fechamodificacion,
                    Data = listHeaders
                };

                return Task.FromResult(infoGenPCI);
            }

            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<long> SaveInfoTINReport(TINTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                SplInfoGeneralTin data = _dbContext.SplInfoGeneralTins.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                SplInfoGeneralTin infoGenTIN = new()
                {
                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    FechaPrueba = report.Date,
                    Conexion = report.Connection,
                    Tension = report.Voltage,
                    DevEnergizado = report.EnergizedWinding,
                    DevInducido = report.InducedWinding,
                    Frecuencia = report.Frecuency,
                    Notas = report.Grades,
                    PosAt = report.PosAT,
                    PosBt = report.PosBT,
                    PosTer = report.PosTER,
                    RelTension = report.RelVoltage,
                    TensionAplicada = report.AppliedVoltage,
                    TensionInducida = report.InducedVoltage,
                    Tiempo = report.Time,
                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,
                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralTins.Add(infoGenTIN) : _dbContext.SplInfoGeneralTins.Update(infoGenTIN);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<long> SaveInfoTAPReport(TAPTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoArchivosPir> listHeaders = new();
                SplInfoGeneralTap data = _dbContext.SplInfoGeneralTaps.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;

                SplInfoGeneralTap infoGenTAP = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.Date,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,

                    FechaPrueba = report.Date,
                    Frecuencia = report.TAPTests.Freacuency,
                    ValAcept = report.TAPTests.ValueAcep,
                    IdCapAt = report.IdCapAT,
                    IdCapBt = report.IdCapBT,
                    IdCapTer = report.IdCapTER,
                    IdRepFpc = report.IdRepFPC,
                    NoConexionesAt = report.NoConnectionsAT,
                    NoConexionesBt = report.NoConnectionsBT,
                    NoConexionesTer = report.NoConnectionsTER,
                    TipoUnidad = report.UnitType,

                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralTaps.Add(infoGenTAP) : _dbContext.SplInfoGeneralTaps.Update(infoGenTAP);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenTAP.IdRep;

                List<SplInfoDetalleTap> listdetails = new();

                foreach (TAPTestsDetails obj in report.TAPTests.TAPTestsDetails)
                {
                    int pos = report.TAPTests.TAPTestsDetails.IndexOf(obj);

                    listdetails.Add(new SplInfoDetalleTap()
                    {
                        IdRep = idCarga,
                        Renglon = pos + 1,
                        AmpCal = obj.AmpCal,
                        Capacitancia = obj.Capacitance,
                        Corriente = obj.CurrentkV,
                        DevAterrizado = obj.WindingGrounded,
                        DevEnergizado = obj.WindingEnergized,
                        NivelTension = obj.VoltageLevel,
                        PorcCorriente = obj.CurrentPercentage,
                        TensionAplicada = obj.AppliedkV,
                        Tiempo = obj.Time,

                    });

                }

                SplInfoDetalleTap datadetail = _dbContext.SplInfoDetalleTaps.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleTaps.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleTaps.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public async Task<List<ColumnTitleCEMReports>> GetColumnsConfigurableCEM(decimal pTypeTrafoId, string pKeyLenguage, string pPosPrimaria, string pPosSecundaria, string pNoSerieNormal)
        {

            var r1 = await (from infoAparato in _dbContext.SplInfoaparatoDgs
                            join norma in _dbContext.SplNormas on infoAparato.Norma equals norma.Clave
                            join desAngunlar in _dbContext.SplDesplazamientoAngulars on infoAparato.DesplazamientoAngular equals desAngunlar.Clave
                            join idioma in _dbContext.SplIdiomas on infoAparato.ClaveIdioma equals idioma.ClaveIdioma
                            where infoAparato.OrderCode == pNoSerieNormal
                            select new { Norma = norma.Clave, NormaDes = norma.Descripcion, DesplazaDes = desAngunlar.Descripcion, DesplazamientoAngular = desAngunlar.Clave }).FirstOrDefaultAsync();

            string claveABuscar = string.Empty;

            if ((pPosPrimaria == "AT" || pPosPrimaria == "BT") && (pPosSecundaria == "AT" || pPosSecundaria == "BT"))
            {
                claveABuscar = "ABT";
            }
            else if ((pPosPrimaria == "AT" || pPosPrimaria.ToUpper() == "TER") && (pPosSecundaria == "AT" || pPosSecundaria.ToUpper() == "TER"))
            {
                claveABuscar = "ATT";
            }
            else if ((pPosPrimaria == "BT" || pPosPrimaria.ToUpper() == "TER") && (pPosSecundaria == "BT" || pPosSecundaria.ToUpper() == "TER"))
            {
                claveABuscar = "BTT";
            }
            var p = await _dbContext.SplTituloColumnasRdts.Where(x => x.ClaveIdioma == pKeyLenguage && x.ClavePrueba == claveABuscar && x.DesplazamientoAngular == r1.DesplazamientoAngular && x.Norma == r1.Norma).Select(x => new ColumnTitleCEMReports
            {
                PosColumna = x.PosColumna
            ,
                PrimerRenglon = x.PrimerRenglon,
                SegundoRenglon = x.SegundoRenglon,
                TitPos1 = x.TitPos1,
                TitPos2 = x.TitPos2
            }).ToListAsync();


            if (p.Count == 0)
            {
                throw new Exception($"No se tiene registrado los títulos de las terminales para la norma {r1.NormaDes}, desplazamiento angular {r1.DesplazaDes}, idioma {pKeyLenguage}, favor de contactar al administrador del sistema");
            }

            return p;


        }

        public Task<long> SaveInfoCEMReport(CEMTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                SplInfoGeneralCem data = _dbContext.SplInfoGeneralCems.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;

                SplInfoGeneralCem infoGenCEM = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,

                    FechaPrueba = report.LoadDate,
                    IdPosPrimaria = report.IdPosPrimary,
                    IdPosSecundaria = report.IdPosSecundary,
                    PosPrimaria = report.PosPrimary,
                    PosSecundaria = report.PosSecundary,
                    TituloTerm1 = report.TitTerm1,
                    TituloTerm2 = report.TitTerm2,
                    TituloTerm3 = report.TitTerm3,
                    VoltajePrueba = report.TestsVoltage,

                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = DateTime.Now,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralCems.Add(infoGenCEM) : _dbContext.SplInfoGeneralCems.Update(infoGenCEM);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenCEM.IdRep;

                List<SplInfoDetalleCem> listdetails = new();

                foreach (CEMTestsDetails obj in report.CEMTestsDetails)
                {
                    int pos = report.CEMTestsDetails.IndexOf(obj);

                    listdetails.Add(new SplInfoDetalleCem()
                    {
                        IdRep = idCarga,
                        Renglon = pos + 1,
                        CorrienteTerm1 = obj.CurrentTerm1,
                        CorrienteTerm2 = obj.CurrentTerm2,
                        CorrienteTerm3 = obj.CurrentTerm3,
                        PosSecundaria = obj.PosSec,

                    });

                }

                SplInfoDetalleCem datadetail = _dbContext.SplInfoDetalleCems.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleCems.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleCems.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<long> SaveInfoTDPReport(TDPTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                SplInfoGeneralTdp data = _dbContext.SplInfoGeneralTdps.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;

                SplInfoGeneralTdp infoGenTDP = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,

                    FechaPrueba = report.Date,
                    DescMayMv = report.DescMayMv,
                    DescMayPc = report.DescMayPc,
                    Frecuencia = report.Frequency,
                    IncMaxPc = report.IncMaxPc,
                    Intervalo = report.Interval,
                    NivelesTension = report.VoltageLevels,
                    NivelHora = report.TimeLevel,
                    NivelRealce = report.OutputLevel,
                    NoCiclos = report.NoCycles,
                    PosAt = report.Pos_At,
                    PosBt = report.Pos_Bt,
                    PosTer = report.Pos_Ter,
                    TerminalesPrueba = report.TerminalsTest,
                    TiempoTotal = report.TotalTime,
                    TipoMedicion = report.MeasurementType,

                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = DateTime.Now,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralTdps.Add(infoGenTDP) : _dbContext.SplInfoGeneralTdps.Update(infoGenTDP);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenTDP.IdRep;

                List<SplInfoDetalleTdp> listdetails = new();

                foreach (TDPTestsDetails obj in report.TDPTest.TDPTestsDetails)
                {
                    int pos = report.TDPTest.TDPTestsDetails.IndexOf(obj) + 1;

                    decimal Terminal1 = 0;
                    decimal Terminal2 = 0;
                    decimal Terminal3 = 0;
                    decimal Terminal4 = 0;
                    decimal Terminal5 = 0;
                    decimal Terminal6 = 0;

                    foreach (TDPTerminals item in obj.TDPTerminals)
                    {

                        int pos2 = obj.TDPTerminals.IndexOf(item) + 1;
                        switch (pos2)
                        {
                            case 1:
                                if (report.MeasurementType.ToUpper().Equals("PICOLUMNS"))
                                {
                                    Terminal1 = Convert.ToDecimal(item.pC);

                                }
                                else if (report.MeasurementType.ToUpper().Equals("MICROVOLTS"))
                                {
                                    Terminal1 = Convert.ToDecimal(item.µV);
                                }
                                else
                                {

                                    Terminal1 = Convert.ToDecimal(item.pC);

                                    Terminal2 = Convert.ToDecimal(item.µV);

                                }

                                break;
                            case 2:
                                if (report.MeasurementType.ToUpper().Equals("PICOLUMNS"))
                                {
                                    Terminal2 = Convert.ToDecimal(item.pC);

                                }
                                else if (report.MeasurementType.ToUpper().Equals("MICROVOLTS"))
                                {
                                    Terminal2 = Convert.ToDecimal(item.µV);
                                }
                                else
                                {

                                    Terminal3 = Convert.ToDecimal(item.pC);

                                    Terminal4 = Convert.ToDecimal(item.µV);

                                }
                                break;
                            case 3:
                                if (report.MeasurementType.ToUpper().Equals("PICOLUMNS"))
                                {
                                    Terminal3 = Convert.ToDecimal(item.pC);

                                }
                                else if (report.MeasurementType.ToUpper().Equals("MICROVOLTS"))
                                {
                                    Terminal3 = Convert.ToDecimal(item.µV);
                                }
                                else
                                {

                                    Terminal5 = Convert.ToDecimal(item.pC);

                                    Terminal6 = Convert.ToDecimal(item.µV);

                                }
                                break;

                            default:
                                // code block
                                break;
                        }
                    }
                    listdetails.Add(new SplInfoDetalleTdp()
                    {
                        IdRep = idCarga,
                        Renglon = pos,
                        Tension = obj.Voltage,
                        Tiempo = obj.Time,
                        Terminal1 = Terminal1,
                        Terminal2 = Terminal2,
                        Terminal3 = Terminal3,
                        Terminal4 = Terminal4,
                        Terminal5 = Terminal5,
                        Terminal6 = Terminal6,

                    });

                }

                SplInfoDetalleTdp datadetail = _dbContext.SplInfoDetalleTdps.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleTdps.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleTdps.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                SplInfoCameTdp InfoCameTdp = new()
                {
                    IdRep = idCarga,
                    Calibracion1 = report.TDPTest.TDPTestsDetailsCalibration.Calibration1,
                    Calibracion2 = report.TDPTest.TDPTestsDetailsCalibration.Calibration2,
                    Calibracion3 = report.TDPTest.TDPTestsDetailsCalibration.Calibration3,
                    Calibracion4 = report.TDPTest.TDPTestsDetailsCalibration.Calibration4,
                    Calibracion5 = report.TDPTest.TDPTestsDetailsCalibration.Calibration5,
                    Calibracion6 = report.TDPTest.TDPTestsDetailsCalibration.Calibration6,

                    Medido1 = report.TDPTest.TDPTestsDetailsCalibration.Measured1,
                    Medido2 = report.TDPTest.TDPTestsDetailsCalibration.Measured2,
                    Medido3 = report.TDPTest.TDPTestsDetailsCalibration.Measured3,
                    Medido4 = report.TDPTest.TDPTestsDetailsCalibration.Measured4,
                    Medido5 = report.TDPTest.TDPTestsDetailsCalibration.Measured5,
                    Medido6 = report.TDPTest.TDPTestsDetailsCalibration.Measured6,

                    Notas = report.TDPTest.TDPTestsDetailsCalibration.Grades,

                };

                SplInfoCameTdp dataInfoCameTdps = _dbContext.SplInfoCameTdps.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (dataInfoCameTdps is null)
                    _dbContext.SplInfoCameTdps.AddRange(InfoCameTdp);
                else
                    _dbContext.SplInfoCameTdps.UpdateRange(InfoCameTdp);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<long> SaveInfoCGDReport(CGDTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoSeccionCgd> listHeaders = new();
                SplInfoGeneralCgd data = _dbContext.SplInfoGeneralCgds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                string testType = report.KeyTest;
                SplInfoGeneralCgd infoGenCGD = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,

                    TipoAceite = report.CGDTests.FirstOrDefault().OilType,
                    ValAceptCg = report.CGDTests.FirstOrDefault().ValAcceptCg,

                    Comentario = report.Comment,
                    Creadopor = report.Creadopor,
                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralCgds.Add(infoGenCGD) : _dbContext.SplInfoGeneralCgds.Update(infoGenCGD);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenCGD.IdRep;

                List<SplInfoDetalleCgd> listdetails = new();

                foreach (CGDTests obj in report.CGDTests)
                {
                    int pos = report.CGDTests.IndexOf(obj);

                    foreach (CGDTestsDetails item in obj.CGDTestsDetails)
                    {
                        int renglon = obj.CGDTestsDetails.IndexOf(item);
                        string Descripcion = "";

                        switch (item.Key.ToUpper())
                        {
                            case Enums.CGDKeys.Hydrogen:
                                Descripcion = "(H2)";
                                NewData1(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.Oxygen:
                                Descripcion = "(O2)";
                                NewData1(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.Nitrogen:
                                Descripcion = "(N2)";
                                NewData1(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.Methane:
                                Descripcion = "(CH4)";
                                NewData1(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.CarbonMonoxide:
                                Descripcion = "(CO)";
                                NewData1(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.CarbonDioxide:
                                Descripcion = "(CO2)";
                                NewData1(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.Ethylene:
                                Descripcion = "(C2H4)";
                                NewData1(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.Ethane:
                                Descripcion = "(C2H6)";
                                NewData1(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.Acetylene:
                                Descripcion = "(C2H2)";
                                NewData1(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.TotalGases:
                                Descripcion = report.LanguageKey.ToUpper().Equals("ES") ? "GASES TOTALES" : "TOTAL GAS";
                                NewData2(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.CombustibleGases:
                                Descripcion = report.LanguageKey.ToUpper().Equals("ES") ? "GASES COMBUSTIBLES" : "COMBUSTIBLE GAS";
                                NewData2(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.PorcCombustibleGas:
                                Descripcion = report.LanguageKey.ToUpper().Equals("ES") ? "% DE GAS COMBUSTIBLE" : "COMBUSTIBLE GAS (%)";
                                NewData2(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.PorcGasContent:
                                Descripcion = report.LanguageKey.ToUpper().Equals("ES") ? "% CONTENIDO DE GAS" : "GAS CONTENT (%)";
                                NewData2(idCarga, pos, item, renglon, Descripcion, testType, ref listdetails);
                                break;

                            case Enums.CGDKeys.AcetyleneTest:
                                Descripcion = report.LanguageKey.ToUpper().Equals("ES") ? "ACETILENO" : "ACETYLENE";
                                NewData3(idCarga, obj, pos, item, renglon, Descripcion, ref listdetails);
                                break;

                            case Enums.CGDKeys.HydrogenTest:
                                Descripcion = report.LanguageKey.ToUpper().Equals("ES") ? "HIDROGENO" : "HYDROGEN";
                                NewData3(idCarga, obj, pos, item, renglon, Descripcion, ref listdetails);
                                break;

                            case Enums.CGDKeys.MethaneEthyleneEthaneTest:
                                Descripcion = report.LanguageKey.ToUpper().Equals("ES") ? " METANO + ETILENO + ETANO" : "METHANE + ETHYLENE + ETHANE";
                                NewData3(idCarga, obj, pos, item, renglon, Descripcion, ref listdetails);
                                break;

                            case Enums.CGDKeys.CarbonMonoxideTest:
                                Descripcion = report.LanguageKey.ToUpper().Equals("ES") ? "MONOXIDO DE CARBONO" : "CARBON MONOXIDE";
                                NewData3(idCarga, obj, pos, item, renglon, Descripcion, ref listdetails);
                                break;

                            case Enums.CGDKeys.CarbonDioxideTest:
                                Descripcion = report.LanguageKey.ToUpper().Equals("ES") ? "DIOXIDO DE CARBONO" : "CARBON DIOXIDE";
                                NewData3(idCarga, obj, pos, item, renglon, Descripcion, ref listdetails);
                                break;

                            default:

                                break;
                        }
                    }

                    listHeaders.Add(new SplInfoSeccionCgd()
                    {
                        IdRep = idCarga,
                        Seccion = pos + 1,
                        FechaPrueba = obj.Date,
                        HrsTemperatura = obj.Hour,
                        Metodo = obj.Method,
                        Notas = obj.Grades,
                        Resultado = obj.Result,
                    });
                }

                SplInfoDetalleCgd datadetail = _dbContext.SplInfoDetalleCgds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleCgds.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleCgds.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                SplInfoSeccionCgd datasec = _dbContext.SplInfoSeccionCgds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datasec is null)
                    _dbContext.SplInfoSeccionCgds.AddRange(listHeaders);
                else
                    _dbContext.SplInfoSeccionCgds.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));

                static void NewData1(decimal idCarga, int pos, CGDTestsDetails item, int renglon, string Descripcion, string testType, ref List<SplInfoDetalleCgd> listdetails) => listdetails.Add(new SplInfoDetalleCgd()
                {
                    IdRep = idCarga,
                    Seccion = pos + 1,
                    Renglon = renglon + 1,
                    AntesPpm = testType.Equals("ADP") ? 0 : item.Value1,
                    DespuesPpm = testType.Equals("ADP") ? 0 : item.Value2,
                    IncrementoPpm = testType.Equals("ADP") ? 0 : item.Value3,
                    LimiteMax = 0,
                    Validacion = " ",
                    Descripcion = Descripcion,
                    ResultadoPpm = testType.Equals("ADP") ? item.Value1 : 0,
                    AceptacionPpm = testType.Equals("ADP") ? item.Value2 : 0,
                });

                static void NewData2(decimal idCarga, int pos, CGDTestsDetails item, int renglon, string Descripcion, string testType, ref List<SplInfoDetalleCgd> listdetails) => listdetails.Add(new SplInfoDetalleCgd()
                {
                    IdRep = idCarga,
                    Seccion = pos + 1,
                    Renglon = renglon + 1,
                    AntesPpm = item.Value1,
                    DespuesPpm = testType.Equals("ADP") ? 0 : item.Value2,
                    IncrementoPpm = 0,
                    LimiteMax = 0,
                    Validacion = " ",
                    Descripcion = Descripcion,
                    ResultadoPpm = 0,
                    AceptacionPpm = 0,
                });

                static void NewData3(decimal idCarga, CGDTests obj, int pos, CGDTestsDetails item, int renglon, string Descripcion, ref List<SplInfoDetalleCgd> listdetails) => listdetails.Add(new SplInfoDetalleCgd()
                {
                    IdRep = idCarga,
                    Seccion = pos + 1,
                    Renglon = renglon + 1,
                    AntesPpm = 0,
                    DespuesPpm = 0,
                    IncrementoPpm = 0,
                    LimiteMax = item.Value3,
                    Validacion = (item.Value4 == 0) ? (obj.KeyTest.ToUpper().Equals("ES") ? "Aceptado" : "Accepted") : (obj.KeyTest.ToUpper().Equals("ES") ? "Rechazado" : "Rejected"),
                    Descripcion = Descripcion,
                    ResultadoPpm = 0,
                    AceptacionPpm = 0,
                });
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(SaveInfoFPCReport));

            }
        }

        public Task<long> SaveInfoARFReport(ARFTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoArchivosArf> listHeaders = new();
                SplInfoGeneralArf data = _dbContext.SplInfoGeneralArves.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralArf infoGenARF = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    Equipo = report.Team,
                    FechaPrueba = report.Date,
                    NivelesTension = report.LevelsVoltage,
                    Terciario2dabaja = report.Tertiary_2Low,
                    TerciarioDisp = report.TertiaryDisp,

                    TotalPags = report.TotalPags,

                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralArves.Add(infoGenARF) : _dbContext.SplInfoGeneralArves.Update(infoGenARF);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenARF.IdRep;

                List<SplInfoDetalleArf> listdetails = new();

                foreach (Files obj in report.Files)
                {
                    int pos = report.Files.IndexOf(obj) + 1;
                    listHeaders.Add(new SplInfoArchivosArf()
                    {
                        IdRep = idCarga,
                        Orden = pos,
                        Archivo = obj.File,
                        Nombre = obj.Name
                    });
                }

                foreach (ARFTests obj in report.ARFTests)
                {
                    int pos = report.ARFTests.IndexOf(obj) + 1;

                    listdetails.Add(new SplInfoDetalleArf()
                    {
                        IdRep = idCarga,
                        Seccion = pos,
                        Boquillas = obj.Nozzles,
                        NivelAceite = obj.LevelOil,
                        NucleoHerraje = obj.CoreHardware,
                        PosAt = obj.PosAt,
                        PosBt = obj.PosBt,
                        PosTer = obj.PosTer,
                        TempAceite = obj.TempOil,
                        Terciario = obj.Tertiary,

                    });

                }

                SplInfoDetalleArf datadetail = _dbContext.SplInfoDetalleArves.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleArves.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleArves.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                SplInfoArchivosArf datasec = _dbContext.SplInfoArchivosArves.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datasec is null)
                    _dbContext.SplInfoArchivosArves.AddRange(listHeaders);
                else
                    _dbContext.SplInfoArchivosArves.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<long> SaveInfoRDDReport(RDDTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                SplInfoGeneralRdd data = _dbContext.SplInfoGeneralRdds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;
                SplInfoGeneralRdd infoGenRDD = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.CapacityReport,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    Conexion = report.Connection,
                    CapacidadPrueba = report.Capacity,
                    ConfigDevanado = report.ConfigWinding,
                    DevCorto = report.ShortWinding,
                    DevEnergizado = report.EnergizedWinding,
                    PorcJx = report.PorcJx,
                    PorcX = report.OutRDDTests.FirstOrDefault().RDDTestsDetails.FirstOrDefault().PorcX,
                    PorcZ = report.PorcZ,
                    PosAt = report.PositionAT,
                    PosBt = report.PositionBT,
                    S3fV2 = report.OutRDDTests[0].RDDTestsDetails.FirstOrDefault().S3fV2,
                    TensionCorto = report.VoltageSW,
                    TensionEnerg = report.VoltageEW,
                    ValorAceptacion = report.ValueAcep,
                    FechaPrueba = report.DateTest,
                    DporcX = report.TporX,

                    Comentario = report.Comment,
                    Creadopor = report.Creadopor,
                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralRdds.Add(infoGenRDD) : _dbContext.SplInfoGeneralRdds.Update(infoGenRDD);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenRDD.IdRep;

                List<SplInfoDetalleRdd> listdetails = new();

                foreach (OutRDDTests obj in report.OutRDDTests)
                {
                    int pos = report.OutRDDTests.IndexOf(obj) + 1;

                    foreach (RDDTestsDetails item in obj.RDDTestsDetails)
                    {
                        int pos1 = obj.RDDTestsDetails.IndexOf(item) + 1;
                        listdetails.Add(new SplInfoDetalleRdd()
                        {
                            IdRep = idCarga,
                            Seccion = pos + 1,
                            Corriente = item.CurrentA,
                            Fase = item.Phase,
                            Impedancia = item.Impedance,
                            Perdidas = item.LossesW,
                            PorcFp = item.PorcFp,
                            PorcX = item.PorcX,
                            Reactancia = item.Reactance,
                            Resistencia = item.Resistance,
                            Tension = item.AppliedVoltage,
                            Renglon = pos1,

                        });
                    }
                }

                SplInfoDetalleRdd datadetail = _dbContext.SplInfoDetalleRdds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleRdds.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleRdds.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {

                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<CGDTestsGeneral> GetInfoCGDReport(string NroSerie, string KeyTests, bool Result)
        {
            CGDTestsGeneral infoGenCGD = null;
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<CGDTestsDetails> CGDTestsDetails = new();
                List<CGDTests> CGDTests = new();

                SplInfoGeneralCgd data = _dbContext.SplInfoGeneralCgds.AsNoTracking().FirstOrDefault(CE => CE.NoSerie.ToUpper().Equals(NroSerie) && CE.ClavePrueba.ToUpper().Equals(KeyTests) && CE.Resultado);
                //if (data ==  null )
                //{
                //    throw new ArgumentException("No existe un reporte de Cromatografía de Gases Disueltos en Aceite para las coincidencias de: Nro. de serie, Prueba Antes de Pruebas y Resultado Aceptado, para tomar % CONTENIDO DE GAS:");
                //}
                if (data != null)
                {
                    decimal idCarga = data.IdRep;

                    List<SplInfoSeccionCgd> sections = _dbContext.SplInfoSeccionCgds.AsNoTracking().Where(CE => CE.IdRep == idCarga).ToList();

                    List<SplInfoDetalleCgd> deteils = _dbContext.SplInfoDetalleCgds.AsNoTracking().Where(CE => CE.IdRep == idCarga).ToList();

                    List<CGDTestsDetails> listdetails = new();

                    foreach (SplInfoDetalleCgd item in deteils)
                    {
                        int renglon = deteils.IndexOf(item);
                        string Descripcion = "";
                        switch (item.Descripcion.ToUpper())
                        {
                            case "(H2)":
                                Descripcion = Enums.CGDKeys.Hydrogen;
                                NewData1(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "(O2)":
                                Descripcion = Enums.CGDKeys.Oxygen;
                                NewData1(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "(N2)":
                                Descripcion = Enums.CGDKeys.Nitrogen;
                                NewData1(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "(CH4)":
                                Descripcion = Enums.CGDKeys.Methane;
                                NewData1(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "(CO)":
                                Descripcion = Enums.CGDKeys.CarbonMonoxide;
                                NewData1(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "(CO2)":
                                Descripcion = Enums.CGDKeys.CarbonDioxide;
                                NewData1(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "(C2H4)":
                                Descripcion = Enums.CGDKeys.Ethylene;
                                NewData1(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "(C2H6)":
                                Descripcion = Enums.CGDKeys.Ethane; ;
                                NewData1(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "(C2H2)":
                                Descripcion = Enums.CGDKeys.Acetylene;
                                NewData1(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "GASES TOTALES" or "TOTAL GAS":
                                Descripcion = Enums.CGDKeys.TotalGases;
                                NewData2(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "GASES COMBUSTIBLES" or "COMBUSTIBLE GAS":
                                Descripcion = Enums.CGDKeys.CombustibleGases;
                                NewData2(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "% DE GAS COMBUSTIBLE" or "COMBUSTIBLE GAS (%)":
                                Descripcion = Enums.CGDKeys.PorcCombustibleGas;
                                NewData2(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "% CONTENIDO DE GAS" or "GAS CONTENT (%)":
                                Descripcion = Enums.CGDKeys.PorcGasContent;
                                NewData2(Descripcion, item, ref listdetails, data.ClavePrueba);
                                break;

                            case "ACETILENO" or "ACETYLENE":
                                Descripcion = Enums.CGDKeys.AcetyleneTest;
                                NewData3(Descripcion, item, ref listdetails);
                                break;

                            case "HIDROGENO" or "HYDROGEN":
                                Descripcion = Enums.CGDKeys.HydrogenTest;
                                NewData3(Descripcion, item, ref listdetails);
                                break;

                            case "METANO + ETILENO + ETANO" or "METHANE + ETHYLENE + ETHANE":
                                Descripcion = Enums.CGDKeys.MethaneEthyleneEthaneTest;
                                NewData3(Descripcion, item, ref listdetails);
                                break;

                            case "MONOXIDO DE CARBONO" or "CARBON MONOXIDE":
                                Descripcion = Enums.CGDKeys.CarbonMonoxideTest;
                                NewData3(Descripcion, item, ref listdetails);
                                break;

                            case "DIOXIDO DE CARBONO" or "CARBON DIOXIDE":
                                Descripcion = Enums.CGDKeys.CarbonDioxideTest;
                                NewData3(Descripcion, item, ref listdetails);
                                break;

                            default:

                                break;
                        }
                    }

                    static void NewData1(string description, SplInfoDetalleCgd data, ref List<CGDTestsDetails> listdetails, string keyTests) => listdetails.Add(new CGDTestsDetails()
                    {
                        Key = description,
                        Value1 = keyTests.Equals("ADP") ? 0 : Convert.ToDecimal(data.AntesPpm),
                        Value2 = keyTests.Equals("ADP") ? 0 : Convert.ToDecimal(data.DespuesPpm),
                        Value3 = keyTests.Equals("ADP") ? 0 : Convert.ToDecimal(data.IncrementoPpm),

                    });

                    static void NewData2(string description, SplInfoDetalleCgd data, ref List<CGDTestsDetails> listdetails, string keyTests) => listdetails.Add(new CGDTestsDetails()
                    {

                        Key = description,
                        Value1 = Convert.ToDecimal(data.AntesPpm),
                        Value2 = keyTests.Equals("ADP") ? 0 : Convert.ToDecimal(data.DespuesPpm),

                    });

                    static void NewData3(string description, SplInfoDetalleCgd data, ref List<CGDTestsDetails> listdetails) => listdetails.Add(new CGDTestsDetails()
                    {

                        Key = description,
                        Value1 = 0,
                        Value2 = 0,
                        Value3 = Convert.ToDecimal(data.LimiteMax),
                        Value4 = Convert.ToDecimal(data.Validacion),

                    });

                    foreach (SplInfoSeccionCgd item in sections)
                    {
                        CGDTests.Add(new Domain.SPL.Reports.CGD.CGDTests() { Grades = item.Notas, Date = item.FechaPrueba, Hour = Convert.ToInt32(item.HrsTemperatura), KeyTest = data.ClavePrueba, Method = item.Metodo, Result = item.Resultado, CGDTestsDetails = listdetails, });
                    }

                    infoGenCGD = new()
                    {
                        LoadDate = data.FechaRep,
                        TestNumber = Convert.ToInt32(data.NoPrueba),
                        SerialNumber = data.NoSerie,
                        LanguageKey = data.ClaveIdioma,
                        Customer = data.Cliente,
                        Capacity = data.Capacidad,
                        Result = data.Resultado,
                        NameFile = data.NombreArchivo,
                        File = data.Archivo,
                        TypeReport = data.TipoReporte,
                        KeyTest = data.ClavePrueba,
                        OilType = data.TipoAceite,
                        ValAcceptCg = Convert.ToString(data.ValAceptCg),
                        CGDTests = CGDTests,
                        Comment = data.Comentario,
                        Creadopor = data.Creadopor,
                        Fechacreacion = data.Fechacreacion,
                        Modificadopor = data.Modificadopor,
                        Fechamodificacion = data.Fechamodificacion,
                    };

                }

                return Task.FromResult(infoGenCGD);
            }
            catch (DbUpdateException e)
            {

                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, nameof(SaveInfoFPCReport));
            }
        }

        public Task<long> SaveInfoINDReport(INDTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                List<SplInfoArchivosInd> listHeaders = new();
                SplInfoGeneralInd data = _dbContext.SplInfoGeneralInds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;

                SplInfoGeneralInd infoGenIND = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,

                    FechaPrueba = report.Date,

                    TotalPags = Convert.ToString(report.TotalPags),
                    TcComprados = report.TcPurchased,
                    Anexo = report.Exhibit,
                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralInds.Add(infoGenIND) : _dbContext.SplInfoGeneralInds.Update(infoGenIND);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenIND.IdRep;

                List<SplInfoDetalleInd> listdetails = new();

                foreach (Files obj in report.Files)
                {
                    int pos = report.Files.IndexOf(obj) + 1;
                    listHeaders.Add(new SplInfoArchivosInd()
                    {
                        IdRep = idCarga,
                        Orden = pos,
                        Archivo = obj.File,
                        Nombre = obj.Name
                    });
                }

                foreach (INDTestsDetails obj in report.Data)
                {
                    int pos = report.Data.IndexOf(obj);
                    decimal? kwValue = obj.KwValue;
                    decimal? mvaValue = obj.MvaValue;
                    if(report.KeyTest != "TER")
                    {
                        kwValue = null;
                        mvaValue = null;
                    }
                    listdetails.Add(new SplInfoDetalleInd()
                    {
                        IdRep = idCarga,
                        Renglon = pos + 1,
                        NoPagina = obj.NoPage,
                        NoPaginaFin = obj.NoPagEnd,
                        NoPaginaIni = obj.NoInitPage,
                        ValorKw = kwValue,
                        ValorMva = mvaValue,

                    });

                }

                SplInfoDetalleInd datadetail = _dbContext.SplInfoDetalleInds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleInds.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleInds.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                SplInfoArchivosInd datasec = _dbContext.SplInfoArchivosInds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datasec is null)
                    _dbContext.SplInfoArchivosInds.AddRange(listHeaders);
                else
                    _dbContext.SplInfoArchivosInds.UpdateRange(listHeaders);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<long> SaveInfoFPAReport(FPATestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                SplInfoGeneralFpa data = _dbContext.SplInfoGeneralFpas.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;

                SplInfoGeneralFpa infoGenFPA = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,

                    FechaPrueba = report.Date,
                    MarcaAceite = report.BrandOil,
                    Notas = report.Grades,
                    TempAmbiente = report.Data.AmbientTemperature,
                    HumedadRelativa = report.Data.RelativeHumidity,
                    TipoAceite = report.Data.OilType,

                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralFpas.Add(infoGenFPA) : _dbContext.SplInfoGeneralFpas.Update(infoGenFPA);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenFPA.IdRep;

                List<SplInfoDetalleFpa> listdetails = new();

                foreach (FPAPowerFactor obj in report.Data.FPATestsDetails.FPAPowerFactor)
                {
                    int pos = report.Data.FPATestsDetails.FPAPowerFactor.IndexOf(obj) + 1;

                    listdetails.Add(new SplInfoDetalleFpa()
                    {
                        IdRep = idCarga,
                        Renglon = pos,
                        Seccion = 1,
                        Asmt = obj.ASMT,
                        Medicion = obj.Measurement,
                        Escala = obj.Scale,
                        FactorCorr = obj.CorrectionFactor,
                        FactorPotencia = obj.PowerFactor,
                        LimiteMax = obj.MaxLimitFP1,
                        Validacion = obj.MaxLimitFP2,
                        Descripcion = obj.Temperature

                    });

                }

                foreach (FPADielectricStrength obj in report.Data.FPATestsDetails.FPADielectricStrength)
                {
                    int pos = report.Data.FPATestsDetails.FPADielectricStrength.IndexOf(obj) + 1;

                    listdetails.Add(new SplInfoDetalleFpa()
                    {
                        IdRep = idCarga,
                        Renglon = pos,
                        Seccion = 2,
                        Apertura = obj.OpeningMm,
                        Asmt = obj.ASTM,
                        Valor1 = obj.OneSt,
                        Valor2 = obj.TwoNd,
                        Valor3 = obj.ThreeRd,
                        Valor4 = obj.FourTh,
                        Valor5 = obj.FiveTh,
                        Promedio = obj.Average,
                        Descripcion = obj.Electrodes,
                        LimiteMax = obj.MinLimit1,
                        Validacion = obj.MinLimit2

                    });

                }

                listdetails.Add(new SplInfoDetalleFpa()
                {
                    IdRep = idCarga,
                    Renglon = 1,
                    Seccion = 3,
                    Descripcion = report.Data.FPATestsDetails.FPAWaterContent.Test,
                    Asmt = report.Data.FPATestsDetails.FPAWaterContent.ASTM,
                    Valor1 = report.Data.FPATestsDetails.FPAWaterContent.OneSt,
                    Valor2 = report.Data.FPATestsDetails.FPAWaterContent.TwoNd,
                    Valor3 = report.Data.FPATestsDetails.FPAWaterContent.ThreeRd,

                    Promedio = report.Data.FPATestsDetails.FPAWaterContent.Average,

                    LimiteMax = report.Data.FPATestsDetails.FPAWaterContent.MaxLimit1,
                    Validacion = report.Data.FPATestsDetails.FPAWaterContent.MaxLimit2

                });

                listdetails.Add(new SplInfoDetalleFpa()
                {
                    IdRep = idCarga,
                    Renglon = 1,
                    Seccion = 4,
                    Descripcion = "GAS %",
                    Asmt = report.Data.FPATestsDetails.FPAGasContent.ASTM,

                    Medicion = report.Data.FPATestsDetails.FPAGasContent.Measurement,

                    LimiteMax = report.Data.FPATestsDetails.FPAGasContent.Limit1,
                    Validacion = report.Data.FPATestsDetails.FPAGasContent.Limit2

                });

                SplInfoDetalleFpa datadetail = _dbContext.SplInfoDetalleFpas.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleFpas.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleFpas.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<long> SaveInfoBPCReport(BPCTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                SplInfoGeneralBpc data = _dbContext.SplInfoGeneralBpcs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);

                SplInfoGeneralBpc infoGenBPC = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,

                    FechaPrueba = report.Date,

                    MetodologiaUsada = report.MethodologyUsed,
                    Notas = report.Grades,
                    Temperatura = report.Temperature,
                    ValorObtenido = report.ObtainedValue,

                    Creadopor = report.Creadopor,

                    Comentario = report.Comment,

                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralBpcs.Add(infoGenBPC) : _dbContext.SplInfoGeneralBpcs.Update(infoGenBPC);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<long> SaveInfoNRAReport(NRATestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                SplInfoGeneralNra data = _dbContext.SplInfoGeneralNras.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;

                SplInfoGeneralNra infoGenNRA = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,
                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,
                    Fechacreacion = report.Fechacreacion,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,
                    Alimentacion = report.Feeding,
                    ValorAlimentacion = report.FeedValue,
                    UmAlimentacion = report.UmFeeding,
                    Altura = report.Data.Height,
                    UmAltura = report.Data.UmHeight,
                    CoolingType = report.CoolingType,
                    Area = report.Data.Surface,
                    UmArea = report.Data.UmSurface,
                    Perimetro = report.Data.Length,
                    UmPerimetro = report.Data.UmLength,
                    PosAt = report.Data.HV,
                    PosBt = report.Data.LV,
                    PosTer = report.Data.TV,
                    Laboratorio = report.Laboratory,
                    FechaDatos = report.DateData,
                    CargarInfo = report.LoadInfo,
                    Garantia = report.Data.Guaranteed,
                    Notas = report.Grades,
                    Alfa = report.Data.Alfa,
                    Sv = report.Data.SV,
                    FactorK = report.Data.KFactor,
                    CantMediciones = Math.Round(report.QtyMeasurements, 0),
                    ClaveNorma = report.Rule,
                    FechaPrueba = report.TestDate,
                    Npplp = Math.Round(report.Data.AvgSoundPressureLevel, 0),
                    Prlw = Math.Round(report.Data.SoundPowerLevel, 0),
                    MedAyd = report.MediAyd

                };

                _ = data is null ? _dbContext.SplInfoGeneralNras.Add(infoGenNRA) : _dbContext.SplInfoGeneralNras.Update(infoGenNRA);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenNRA.IdRep;

                List<SplInfoDetalleNra> listdetails = new();
                int seccionAntDes = 0;
                int seccionCooling = 0;

                if (report.KeyTest.ToUpper().Equals("OCT"))
                {
                    int posANT = 1;
                    foreach (MatrixNRATests obj in report.Data.NRATestsDetailsOcts.MatrixNRAAntTests)
                    {

                        listdetails.Add(new SplInfoDetalleNra()
                        {
                            IdRep = idCarga,
                            Renglon = posANT,
                            Seccion = obj.Section,
                            Pos = obj.Position,
                            Altura = obj.Height,
                            TipoInfo = obj.TypeInformation,
                            DbaMedido = obj.Dba,
                            DbaCorr = null,
                            D125 = obj.Decibels125,
                            D315 = obj.Decibels315,
                            D250 = obj.Decibels250,
                            D1000 = obj.Decibels1000,
                            D10000 = obj.Decibels10000,
                            D2000 = obj.Decibels2000,
                            D4000 = obj.Decibels4000,
                            D500 = obj.Decibels500,
                            D63 = obj.Decibels63,
                            D8000 = obj.Decibels8000
                        });
                        posANT++;
                    }

                    foreach (MatrixNRATests obj in report.Data.NRATestsDetailsOcts.MatrixNRADesTests)
                    {
                        seccionAntDes = obj.Section;
                        listdetails.Add(new SplInfoDetalleNra()
                        {
                            IdRep = idCarga,
                            Renglon = posANT,
                            Seccion = obj.Section,
                            Pos = obj.Position,
                            Altura = obj.Height,
                            TipoInfo = obj.TypeInformation,
                            DbaMedido = obj.Dba,
                            DbaCorr = null,
                            D125 = obj.Decibels125,
                            D315 = obj.Decibels315,
                            D250 = obj.Decibels250,
                            D1000 = obj.Decibels1000,
                            D10000 = obj.Decibels10000,
                            D2000 = obj.Decibels2000,
                            D4000 = obj.Decibels4000,
                            D500 = obj.Decibels500,
                            D63 = obj.Decibels63,
                            D8000 = obj.Decibels8000
                        });
                        posANT++;
                    }

                    int PosCoolingType = 1;

                    foreach (MatrixNRATests obj in report.Data.NRATestsDetailsOcts.MatrixNRACoolingTypeTests)
                    {
                        seccionCooling = obj.Section;
                        listdetails.Add(new SplInfoDetalleNra()
                        {
                            IdRep = idCarga,
                            Renglon = PosCoolingType,
                            Seccion = obj.Section,
                            Pos = obj.Position,
                            Altura = obj.Height,
                            TipoInfo = obj.TypeInformation,
                            DbaMedido = obj.Dba,
                            DbaCorr = null,
                            D125 = obj.Decibels125,
                            D315 = obj.Decibels315,
                            D250 = obj.Decibels250,
                            D1000 = obj.Decibels1000,
                            D10000 = obj.Decibels10000,
                            D2000 = obj.Decibels2000,
                            D4000 = obj.Decibels4000,
                            D500 = obj.Decibels500,
                            D63 = obj.Decibels63,
                            D8000 = obj.Decibels8000

                        });
                        PosCoolingType++;

                    }

                    listdetails.Add(new SplInfoDetalleNra()
                    {
                        IdRep = idCarga,
                        Renglon = posANT,
                        Seccion = seccionAntDes,
                        Pos = "",
                        Altura = "",
                        TipoInfo = "AmbProm",
                        DbaMedido = report.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Dba,
                        DbaCorr = null,
                        D125 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Decibels125,
                        D315 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Decibels315,
                        D250 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Decibels250,
                        D1000 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Decibels1000,
                        D10000 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Decibels10000,
                        D2000 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Decibels2000,
                        D4000 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Decibels4000,
                        D500 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Decibels500,
                        D63 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Decibels63,
                        D8000 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbProm.Decibels8000

                    });

                    listdetails.Add(new SplInfoDetalleNra()
                    {
                        IdRep = idCarga,
                        Renglon = PosCoolingType,
                        Seccion = seccionCooling,
                        Pos = "",
                        Altura = "",
                        TipoInfo = "Amb+Trans",
                        DbaMedido = report.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Dba,
                        DbaCorr = null,
                        D125 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Decibels125,
                        D315 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Decibels315,
                        D250 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Decibels250,
                        D1000 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Decibels1000,
                        D10000 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Decibels10000,
                        D2000 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Decibels2000,
                        D4000 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Decibels4000,
                        D500 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Decibels500,
                        D63 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Decibels63,
                        D8000 = report.Data.NRATestsDetailsOcts.MatrixNRAAmbTrans.Decibels8000

                    });

                }
                else
                {

                    int posAnt = 1;
                    int seccion = 0;
                    foreach (MatrixNRATests obj in report.Data.NRATestsDetailsRuis.MatrixNRAAntTests)
                    {

                        listdetails.Add(new SplInfoDetalleNra()
                        {
                            IdRep = idCarga,
                            Renglon = posAnt,
                            Seccion = obj.Section,
                            Pos = obj.Position,
                            Altura = obj.Height,
                            TipoInfo = obj.TypeInformation,
                            DbaMedido = obj.Dba,
                            DbaCorr = null,
                            D125 = null,
                            D315 = null,
                            D250 = null,
                            D1000 = null,
                            D10000 = null,
                            D2000 = null,
                            D4000 = null,
                            D500 = null,
                            D63 = null,
                            D8000 = null

                        });
                        seccion = obj.Section;
                        posAnt++;

                    }

                    foreach (MatrixNRATests obj in report.Data.NRATestsDetailsRuis.MatrixNRADesTests)
                    {
                        //int pos = report.Data.NRATestsDetailsRuis.MatrixNRADesTests.IndexOf(obj) + 1;
                        //renglonAntDes = pos;
                        seccionAntDes = obj.Section;
                        listdetails.Add(new SplInfoDetalleNra()
                        {
                            IdRep = idCarga,
                            Renglon = posAnt,
                            Seccion = obj.Section,
                            Pos = obj.Position,
                            Altura = obj.Height,
                            TipoInfo = obj.TypeInformation,
                            DbaMedido = obj.Dba,
                            DbaCorr = null,
                            D125 = null,
                            D315 = null,
                            D250 = null,
                            D1000 = null,
                            D10000 = null,
                            D2000 = null,
                            D4000 = null,
                            D500 = null,
                            D63 = null,
                            D8000 = null

                        });
                        seccion = obj.Section;
                        posAnt++;

                    }


                    listdetails.Add(new SplInfoDetalleNra()
                    {
                        IdRep = idCarga,
                        Renglon = posAnt,
                        Seccion = 1,
                        Pos = "",
                        Altura = "",
                        TipoInfo = "AmbienteProm",
                        DbaMedido = report.Data.NRATestsDetailsRuis.AverageAmb,
                        DbaCorr = null,
                        D125 = null,
                        D315 = null,
                        D250 = null,
                        D1000 = null,
                        D10000 = null,
                        D2000 = null,
                        D4000 = null,
                        D500 = null,
                        D63 = null,
                        D8000 = null

                    });
                    posAnt++;
                    listdetails.Add(new SplInfoDetalleNra()
                    {
                        IdRep = idCarga,
                        Renglon = posAnt,
                        Seccion = 1,
                        Pos = "",
                        Altura = "",
                        TipoInfo = "Amb+Trans",
                        DbaMedido = report.Data.NRATestsDetailsRuis.AmbTrans,
                        DbaCorr = null,
                        D125 = null,
                        D315 = null,
                        D250 = null,
                        D1000 = null,
                        D10000 = null,
                        D2000 = null,
                        D4000 = null,
                        D500 = null,
                        D63 = null,
                        D8000 = null

                    });



                    int pos13 = 1;

                    foreach (MatrixNRATests obj in report.Data.NRATestsDetailsRuis.MatrixNRA13Tests)
                    {

                        listdetails.Add(new SplInfoDetalleNra()
                        {
                            IdRep = idCarga,
                            Renglon = pos13,
                            Seccion = obj.Section,
                            Pos = obj.Position,
                            Altura = obj.Height,
                            TipoInfo = obj.TypeInformation,
                            DbaMedido = obj.Dba,
                            DbaCorr = obj.DbaCor,
                            D125 = null,
                            D315 = null,
                            D250 = null,
                            D1000 = null,
                            D10000 = null,
                            D2000 = null,
                            D4000 = null,
                            D500 = null,
                            D63 = null,
                            D8000 = null

                        });
                        seccion = obj.Section;
                        pos13++;

                    }

                    foreach (MatrixNRATests obj in report.Data.NRATestsDetailsRuis.MatrixNRA23Tests)
                    {

                        listdetails.Add(new SplInfoDetalleNra()
                        {
                            IdRep = idCarga,
                            Renglon = pos13,
                            Seccion = obj.Section,
                            Pos = obj.Position,
                            Altura = obj.Height,
                            TipoInfo = obj.TypeInformation,
                            DbaMedido = obj.Dba,
                            DbaCorr = obj.DbaCor,
                            D125 = null,
                            D315 = null,
                            D250 = null,
                            D1000 = null,
                            D10000 = null,
                            D2000 = null,
                            D4000 = null,
                            D500 = null,
                            D63 = null,
                            D8000 = null

                        });
                        pos13++;
                        seccion = obj.Section;

                    }



                }

                SplInfoDetalleNra datadetail = _dbContext.SplInfoDetalleNras.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleNras.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleNras.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<long> SaveInfoDPRReport(DPRTestsGeneral report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                SplInfoGeneralDpr data = _dbContext.SplInfoGeneralDprs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdLoad);
                decimal idCarga;

                SplInfoGeneralDpr infoGenDPR = new()
                {

                    IdRep = report.IdLoad,
                    FechaRep = report.LoadDate,
                    NoPrueba = report.TestNumber,
                    NoSerie = report.SerialNumber,
                    ClaveIdioma = report.LanguageKey,
                    Cliente = report.Customer,
                    Capacidad = report.Capacity,
                    Resultado = report.Result,
                    NombreArchivo = report.NameFile,
                    Archivo = report.File,
                    TipoReporte = report.TypeReport,
                    ClavePrueba = report.KeyTest,

                    FechaPrueba = report.Date,
                    DescMayMv = report.DescMayMv,
                    DescMayPc = report.DescMayPc,
                    Frecuencia = report.Frequency,
                    IncMaxPc = report.IncMaxPc,
                    Intervalo = report.Interval,

                    NivelHora = report.TimeLevel,
                    NivelRealce = report.OutputLevel,

                    NumeroCiclo = report.NoCycles,
                    PosAt = report.Pos_At ?? " ",
                    PosBt = report.Pos_Bt ?? " ",
                    PosTer = report.Pos_Ter ?? " ",
                    TerminalesPrueba = report.TerminalsTest,
                    TiempoTotal = report.TotalTime,
                    TipoMedicion = report.MeasurementType,

                    Notas = report.Grades,
                    OtroCiclo = report.OtherCycles,
                    TensionPrueba = report.VoltageTest,

                    Creadopor = report.Creadopor,
                    Comentario = report.Comment,

                    Fechacreacion = DateTime.Now,
                    Modificadopor = report.Modificadopor,
                    Fechamodificacion = report.Fechamodificacion,

                };

                _ = data is null ? _dbContext.SplInfoGeneralDprs.Add(infoGenDPR) : _dbContext.SplInfoGeneralDprs.Update(infoGenDPR);

                _ = _dbContext.SaveChanges();

                idCarga = infoGenDPR.IdRep;

                List<SplInfoDetalleDpr> listdetails = new();

                foreach (DPRTestsDetails obj in report.DPRTest.DPRTestsDetails)
                {
                    int pos = report.DPRTest.DPRTestsDetails.IndexOf(obj) + 1;

                    decimal Terminal1 = 0;
                    decimal Terminal2 = 0;
                    decimal Terminal3 = 0;
                    decimal Terminal4 = 0;
                    decimal Terminal5 = 0;
                    decimal Terminal6 = 0;

                    foreach (DPRTerminals item in obj.DPRTerminals)
                    {

                        int pos2 = obj.DPRTerminals.IndexOf(item) + 1;
                        switch (pos2)
                        {
                            case 1:
                                if (report.MeasurementType.ToUpper().Equals("PICOLUMNS"))
                                {
                                    Terminal1 = Convert.ToDecimal(item.pC);

                                }
                                else if (report.MeasurementType.ToUpper().Equals("MICROVOLTS"))
                                {
                                    Terminal1 = Convert.ToDecimal(item.µV);
                                }
                                else
                                {

                                    Terminal1 = Convert.ToDecimal(item.pC);

                                    Terminal2 = Convert.ToDecimal(item.µV);

                                }

                                break;
                            case 2:
                                if (report.MeasurementType.ToUpper().Equals("PICOLUMNS"))
                                {
                                    Terminal2 = Convert.ToDecimal(item.pC);

                                }
                                else if (report.MeasurementType.ToUpper().Equals("MICROVOLTS"))
                                {
                                    Terminal2 = Convert.ToDecimal(item.µV);
                                }
                                else
                                {

                                    Terminal3 = Convert.ToDecimal(item.pC);

                                    Terminal4 = Convert.ToDecimal(item.µV);

                                }
                                break;
                            case 3:
                                if (report.MeasurementType.ToUpper().Equals("PICOLUMNS"))
                                {
                                    Terminal3 = Convert.ToDecimal(item.pC);

                                }
                                else if (report.MeasurementType.ToUpper().Equals("MICROVOLTS"))
                                {
                                    Terminal3 = Convert.ToDecimal(item.µV);
                                }
                                else
                                {

                                    Terminal5 = Convert.ToDecimal(item.pC);

                                    Terminal6 = Convert.ToDecimal(item.µV);

                                }
                                break;

                            default:
                                // code block
                                break;
                        }
                    }
                    listdetails.Add(new SplInfoDetalleDpr()
                    {
                        IdRep = idCarga,
                        Renglon = pos + 1,
                        Tension = obj.Voltage,
                        Tiempo = obj.Time,

                        Terminal1Mv = Terminal1,
                        Terminal1Pc = Terminal2,
                        Terminal2Mv = Terminal3,
                        Terminal2Pc = Terminal4,
                        Terminal3Mv = Terminal5,
                        Terminal3Pc = Terminal6,

                    });

                }

                SplInfoDetalleDpr datadetail = _dbContext.SplInfoDetalleDprs.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                if (datadetail is null)
                    _dbContext.SplInfoDetalleDprs.AddRange(listdetails);
                else
                    _dbContext.SplInfoDetalleDprs.UpdateRange(listdetails);

                _ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public Task<long> SaveInfoETDReport(ETDReport report)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {

                SplInfoGeneralEtd data = _dbContext.SplInfoGeneralEtds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == report.IdRep);
                decimal idCarga;

                //SplInfoGeneralEtd infoGenETD = new()
                //{

                //    IdRep = report.IdRep,
                //    FechaRep = report.FechaRep,
                //    NoPrueba = report.NoPrueba,
                //    NoSerie = report.NoSerie,
                //    ClaveIdioma = report.ClaveIdioma,
                //    Cliente = report.Cliente,
                //    Capacidad = report.Capacidad,
                //    Resultado = report.Resultado,
                //    NombreArchivo = report.NombreArchivo,
                //    Archivo = report.Archivo,
                //    TipoReporte = report.TipoReporte,
                //    ClavePrueba = report.ClavePrueba,
                //    BtDifCap = report.BtDifCap,
                //    CapacidadBt = report.CapacidadBt,
                //    CapacidadTer = report.CapacidadTer,
                //    DevanadoSplit = report.DevanadoSplit,
                //    FactorAltitud = report.FactorAltitud,
                //    FactorKw = report.FactorKw,
                //    IdCorte = report.IdCorte,
                //    IdReg = report.IdReg,
                //    TerCapRed = report.TerCapRed,
                //    Terciario2b = report.Terciario2b,
                //    TipoRegresion = report.TipoRegresion,
                //    UltimaHora = report.UltimaHora,
                //    Creadopor = report.Creadopor,
                //    Comentario = report.Comentario,
                //    Fechacreacion = report.Fechacreacion,
                //    Modificadopor = report.Modificadopor,
                //    Fechamodificacion = report.Fechamodificacion,

                //};

                //_ = data is null ? _dbContext.SplInfoGeneralEtds.Add(infoGenETD) : _dbContext.SplInfoGeneralEtds.Update(infoGenETD);

                //_ = _dbContext.SaveChanges();

                //idCarga = infoGenETD.IdRep;

                List<SplInfoDetalleEtd> listdetails = new();
                List<SplInfoSeccionEtd> sectiondetails = new();
                List<SplInfoGraficaEtd> graficadetails = new();

                foreach (ETDTests obj in report.ETDTestsGeneral.ETDTests)
                {
                    int pos = report.ETDTestsGeneral.ETDTests.IndexOf(obj) + 1;

                    //sectiondetails.Add(new SplInfoSeccionEtd()
                    //{
                    //    AltitudeF1 = obj.AltitudeF1,
                    //    AltitudeF2 = obj.AltitudeF2,
                    //    AwrLim = obj.AwrLim,
                    //    Capacidad = obj.Capacidad,
                    //    CoolingType = obj.CoolingType,
                    //    Corriente = obj.Corriente,
                    //    ElevPromDev = obj.ElevPromDev,
                    //    ElevPtoMasCal = obj.ElevPtoMasCal,
                    //    FactorK = obj.FactorK,
                    //    FechaPrueba = obj.FechaPrueba,
                    //    GradienteDev = obj.GradienteDev,
                    //    GradienteLim = obj.GradienteLim,
                    //    HsrLim = obj.HsrLim,
                    //    IdRep = obj.IdRep,
                    //    NivelTension = obj.NivelTension,
                    //    OverElevation = obj.OverElevation,
                    //    Perdidas = obj.Perdidas,
                    //    PosAt = obj.PosAt,
                    //    PosBt = obj.PosBt,
                    //    PosTer = obj.PosTer,
                    //    ResistCorte = obj.ResistCorte,
                    //    ResistTcero = obj.ResistTcero,
                    //    Resultado = obj.Resultado,
                    //    Seccion = obj.Seccion,
                    //    Sobrecarga = obj.Sobrecarga,
                    //    TempDev = obj.TempDev,
                    //    TempPromAceite = obj.TempPromAceite,
                    //    TempResistCorte = obj.TempResistCorte,
                    //    Terminal = obj.Terminal,
                    //    TorLimite = obj.TorLimite,
                    //    UmResistencia = obj.UmResistencia,

                    //});

                    foreach (ETDTestsDetails o in obj.ETDTestsDetails)
                    {
                        listdetails.Add(new SplInfoDetalleEtd()
                        {
                            AmbienteProm = o.AmbienteProm,
                            Aor = o.Aor,
                            Bor = o.Bor,
                            ElevAceiteInf = o.ElevAceiteInf,
                            ElevAceiteProm = o.ElevAceiteProm,
                            ElevAceiteSup = o.ElevAceiteSup,
                            FechaHora = o.FechaHora,
                            IdRep = o.IdRep,
                            PromRadInf = o.PromRadInf,
                            PromRadSup = o.PromRadSup,
                            Renglon = o.Renglon,
                            Resistencia = o.Resistencia,
                            Seccion = o.Seccion,
                            TempTapa = o.TempTapa,
                            Tiempo = o.Tiempo,
                            Tor = o.Tor

                        });
                    }
                    foreach (GraphicETDTests u in obj.GraphicETDTests)
                    {
                        graficadetails.Add(new SplInfoGraficaEtd()
                        {
                            IdRep = u.IdRep,
                            Renglon = u.Renglon,
                            Seccion = u.Seccion,
                            ValorX = u.ValorX,
                            ValorY = u.ValorY,

                        }); ;

                    }
                }

                //SplInfoSeccionEtd datasec = _dbContext.SplInfoSeccionEtds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                //if (datasec is null)
                //    _dbContext.SplInfoSeccionEtds.AddRange(sectiondetails);
                //else
                //    _dbContext.SplInfoSeccionEtds.UpdateRange(sectiondetails);

                //_ = _dbContext.SaveChanges();

                //SplInfoDetalleEtd datadetail = _dbContext.SplInfoDetalleEtds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                //if (datadetail is null)
                //    _dbContext.SplInfoDetalleEtds.AddRange(listdetails);
                //else
                //    _dbContext.SplInfoDetalleEtds.UpdateRange(listdetails);

                //_ = _dbContext.SaveChanges();

                //SplInfoGraficaEtd dataGrafi = _dbContext.SplInfoGraficaEtds.AsNoTracking().FirstOrDefault(CE => CE.IdRep == idCarga);

                //if (dataGrafi is null)
                //    _dbContext.SplInfoGraficaEtds.AddRange(graficadetails);
                //else
                //    _dbContext.SplInfoGraficaEtds.UpdateRange(graficadetails);

                //_ = _dbContext.SaveChanges();

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (DbUpdateException e)
            {
                transaction.Rollback();
                StringBuilder sb = new();
                _ = sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry eve in e.Entries)
                {
                    _ = sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    break;
                }
                throw new ArgumentException("No se pudo realizar el guardado " + e.Message + sb.ToString() + e.InnerException.Message);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message, ex.InnerException);

            }
        }

        public async Task<List<ConsolidatedReport>> GetConsolidatedReport(string pNoSerie, string pLenguage)
        {
            List<SqlParameter> parametros = new()
            {
                //if (serial != )
                Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, pNoSerie),
                Methods.CrearSqlParameter("@V_CLAVE_IDIOMA", SqlDbType.VarChar, pLenguage)
            };

            //EJECUTAR CONSULTA
            DataSet ds = Methods.EjecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_REPORTE_CONSOLIDADO] (@V_NO_SERIE,@V_CLAVE_IDIOMA)", parametros, "Configuration", _dbContext.Database.GetConnectionString());

            return await Task.FromResult(ds.Tables["Configuration"].AsEnumerable().Select(result => new ConsolidatedReport()
            {

                ID_REP = Convert.ToInt64(result["ID_REP"].ToString()),
                TIPO_REPORTE = result["IdReporte"].ToString(),
                NOMBRE_REPORTE = result["reporte"].ToString(),
                ID_PRUEBA = result["IdPrueba"].ToString(),
                PRUEBA = result["prueba"].ToString(),
                FECHA = string.IsNullOrEmpty(result["fecha"].ToString()) ? null : Convert.ToDateTime(result["fecha"].ToString()),
                FILTROS = result["filtros"].ToString(),
                COMENTARIOS = result["comentarios"].ToString(),
                AGRUPACION = result["AGRUPACION"].ToString(),
                AGRUPACION_EN = result["AGRUPACION_EN"].ToString(),
                DESCRIPCION_EN = result["DESCRIPCION_EN"].ToString(),

            }).ToList());
        }

        public async Task<List<ConsolidatedReport>> GetTestedReport(string pNoSerie)
        {
            List<SqlParameter> parametros = new()
            {
                //if (serial != )
                Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, pNoSerie)
            };

            //EJECUTAR CONSULTA
            DataSet ds = Methods.EjecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_REPORTE_PRUEBAS] (@V_NO_SERIE)", parametros, "Configuration", _dbContext.Database.GetConnectionString());

            return await Task.FromResult(ds.Tables["Configuration"].AsEnumerable().Select(result => new ConsolidatedReport()
            {

                ID_REP = Convert.ToInt64(result["ID_REP"].ToString()),
                IDIOMA = result["Idioma"].ToString(),
                RESULTADO = Convert.ToBoolean(result["Resultado"].ToString()),
                TIPO_REPORTE = result["IdReporte"].ToString(),
                NOMBRE_REPORTE = result["reporte"].ToString(),
                ID_PRUEBA = result["IdPrueba"].ToString(),
                PRUEBA = result["prueba"].ToString(),
                FECHA = string.IsNullOrEmpty(result["fecha"].ToString()) ? null : Convert.ToDateTime(result["fecha"].ToString()),
                FILTROS = result["filtros"].ToString(),
                COMENTARIOS = result["comentarios"].ToString(),
                AGRUPACION = result["AGRUPACION"].ToString(),
                AGRUPACION_EN = result["AGRUPACION_EN"].ToString(),
                DESCRIPCION_EN = result["DESCRIPCION_EN"].ToString(),

            }).ToList());
        }

        public async Task<List<TypeConsolidatedReport>> GetTypeConsolidatedReport(string pNoSerie, string pLenguage)
        {
            List<SqlParameter> parametros = new()
            {
                //if (serial != )
                Methods.CrearSqlParameter("@V_NO_SERIE", SqlDbType.VarChar, pNoSerie),
                Methods.CrearSqlParameter("@V_CLAVE_IDIOMA", SqlDbType.VarChar, pLenguage)
            };

            //EJECUTAR CONSULTA
            DataSet ds = Methods.EjecutarConsultaOFuncion("SELECT * FROM [dbo].[FN_GET_TYPEINFORMATION_CONSOLIDADO] (@V_NO_SERIE,@V_CLAVE_IDIOMA)", parametros, "Configuration", _dbContext.Database.GetConnectionString());

            return await Task.FromResult(ds.Tables["Configuration"].AsEnumerable().Select(result => new TypeConsolidatedReport()
            {

                CargadorTapa = result["CargadorTapa"] is DBNull ? null : result["CargadorTapa"].ToString(),
                Tipo = result["Tipo"] is DBNull ? null : result["Tipo"].ToString(),
                OperationId = result["OperationId"] is DBNull ? 0 : Convert.ToDecimal(result["OperationId"].ToString())

            }).ToList());
        }

        public Task<long> SaveInfoETDReport(ETDTestsGeneral report) => throw new NotImplementedException();

        #endregion
    }

    public class tipo
    {
        public int Capacity { get; set; }
    }
}
