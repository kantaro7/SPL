namespace SPL.Reports.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using global::AutoMapper;

    using MediatR;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

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
    using SPL.Reports.Api.DTOs.Reports;
    using SPL.Reports.Api.DTOs.Reports.ARF;
    using SPL.Reports.Api.DTOs.Reports.BPC;
    using SPL.Reports.Api.DTOs.Reports.CEM;
    using SPL.Reports.Api.DTOs.Reports.CGD;
    using SPL.Reports.Api.DTOs.Reports.DPR;
    using SPL.Reports.Api.DTOs.Reports.ETD;
    using SPL.Reports.Api.DTOs.Reports.FPA;
    using SPL.Reports.Api.DTOs.Reports.FPB;
    using SPL.Reports.Api.DTOs.Reports.FPC;
    using SPL.Reports.Api.DTOs.Reports.IND;
    using SPL.Reports.Api.DTOs.Reports.ISZ;
    using SPL.Reports.Api.DTOs.Reports.NRA;
    using SPL.Reports.Api.DTOs.Reports.PCE;
    using SPL.Reports.Api.DTOs.Reports.PCI;
    using SPL.Reports.Api.DTOs.Reports.PEE;
    using SPL.Reports.Api.DTOs.Reports.PIM;
    using SPL.Reports.Api.DTOs.Reports.PIR;
    using SPL.Reports.Api.DTOs.Reports.PRD;
    using SPL.Reports.Api.DTOs.Reports.RDD;
    using SPL.Reports.Api.DTOs.Reports.ROD;
    using SPL.Reports.Api.DTOs.Reports.RYE;
    using SPL.Reports.Api.DTOs.Reports.TAP;
    using SPL.Reports.Api.DTOs.Reports.TDP;
    using SPL.Reports.Api.DTOs.Reports.TIN;
    using SPL.Reports.Api.Filters;
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
        #region Fields
        private readonly IMediator _meditor;
        private readonly ILogger<ReportsController> _logger;
        private readonly IMapper _mapper;
        #endregion

        public ReportsController(ILogger<ReportsController> logger, IMapper mapper, IMediator meditor)
        {
            _logger = logger;
            _mapper = mapper;
            _meditor = meditor;
        }

        [HttpGet("{typeReport}")]
        [ProducesResponseType(typeof(ApiResponse<List<ReportsDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetReportsQuery(string typeReport)
        {
            try
            {
                ApiResponse<List<Reports>> result = await _meditor.Send(new Application.Queries.Reports.GetReportsQuery(typeReport));
                return new JsonResult(new ApiResponse<List<ReportsDto>>(result.Code, result.Description, _mapper.Map<List<ReportsDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getResultDetailsReports/{nroSerie}/{typeReport}")]
        [ProducesResponseType(typeof(ApiResponse<List<InfoGeneralTypesReportsDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetResultDetailsReports(string nroSerie, string typeReport)
        {
            try
            {
                ApiResponse<List<InfoGeneralTypesReports>> result = await _meditor.Send(new Application.Queries.Reports.GetGeneralTypesReportsQuery(nroSerie, typeReport));

                return new JsonResult(new ApiResponse<List<InfoGeneralTypesReportsDto>>(result.Code, result.Description, _mapper.Map<List<InfoGeneralTypesReportsDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getFilterReports/{typeReport}")]
        [ProducesResponseType(typeof(ApiResponse<List<FilterReportsDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetFilterReports(string nroSerie, string typeReport)
        {
            try
            {
                ApiResponse<List<FilterReports>> result = await _meditor.Send(new Application.Queries.Reports.GetFilterReportsQuery(typeReport));
                return new JsonResult(new ApiResponse<List<FilterReportsDto>>(result.Code, result.Description, _mapper.Map<List<FilterReportsDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getConfigurationReports/{typeReport}/{keyTest}/{numberColumns}")]
        [ProducesResponseType(typeof(ApiResponse<List<ConfigurationReportsDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReports(string typeReport, string keyTest, int numberColumns)
        {
            try
            {
                ApiResponse<List<ConfigurationReports>> result = await _meditor.Send(new Application.Queries.Reports.GetConfigurationReportsQuery(typeReport, keyTest, numberColumns));
                return new JsonResult(new ApiResponse<List<ConfigurationReportsDto>>(result.Code, result.Description, _mapper.Map<List<ConfigurationReportsDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getColumsConfigRDT/{keyTest}/{dAngular}/{rule}/{lenguage}")]
        [ProducesResponseType(typeof(ApiResponse<List<ColumnTitleRDTReportsDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetColumsConfigRDT(string keyTest, string dAngular, string rule, string lenguage)
        {
            try
            {
                ApiResponse<List<ColumnTitleRDTReports>> result = await _meditor.Send(new Application.Queries.Reports.GetColumsConfigRDTQuery(keyTest, dAngular, rule, lenguage));
                return new JsonResult(new ApiResponse<List<ColumnTitleRDTReportsDto>>(result.Code, result.Description, _mapper.Map<List<ColumnTitleRDTReportsDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getColumsConfiRAD/{typeUnit}/{thirdWinding}/{lenguage}")]
        [ProducesResponseType(typeof(ApiResponse<List<ColumnTitleRADReportsDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetColumsConfiRAD(string typeUnit, string thirdWinding, string lenguage)
        {
            try
            {
                ApiResponse<List<ColumnTitleRADReports>> result = await _meditor.Send(new Application.Queries.Reports.GetColumsConfigRADQuery(typeUnit, lenguage, thirdWinding));
                return new JsonResult(new ApiResponse<List<ColumnTitleRADReportsDto>>(result.Code, result.Description, _mapper.Map<List<ColumnTitleRADReportsDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getTitSeriresParallel/{clave}/{lenguage}")]
        [ProducesResponseType(typeof(ApiResponse<TitSeriresParallelReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTitSeriresParallel(string clave, string lenguage)
        {
            try
            {
                ApiResponse<TitSeriresParallelReports> result = await _meditor.Send(new Application.Queries.Reports.GetTitSeriresParallelQuery(clave, lenguage));
                return new JsonResult(new ApiResponse<TitSeriresParallelReportsDto>(result.Code, result.Description, _mapper.Map<TitSeriresParallelReportsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getCorrectionFactor/{clave}/{lenguage}")]
        [ProducesResponseType(typeof(ApiResponse<CorrectionFactorReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCorrectionFactor(string clave, string lenguage)
        {
            try
            {
                ApiResponse<CorrectionFactorReports> result = await _meditor.Send(new Application.Queries.Reports.GetCorrectionFactorQuery(clave, lenguage));
                return new JsonResult(new ApiResponse<CorrectionFactorReportsDto>(result.Code, result.Description, _mapper.Map<CorrectionFactorReportsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveRADReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveRADReport(RADReportDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportRADCommand(_mapper.Map<RADReport>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getConsolidatedReport/{noSerie}/{languaje}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<List<ConsolidatedReportDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConsolidatedReport(string noSerie, string languaje)
        {
            try
            {
                ApiResponse<List<ConsolidatedReport>> result = await _meditor.Send(new Application.Queries.Reports.GetConsolidatedReportQuery(noSerie, languaje));
                return new JsonResult(new ApiResponse<List<ConsolidatedReportDto>>(result.Code, result.Description, _mapper.Map<List<ConsolidatedReportDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("checkInfoROD/{noSerie}/{windingMaterial}/{atPositions}/{btPositions}/{terPositions}/{isAT}/{isBT}/{isTer}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<CheckInfoROD>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CheckInfoROD(string noSerie, string windingMaterial, string atPositions, string btPositions, string terPositions,
            bool isAT, bool isBT, bool isTer)
        {
            try
            {
                ApiResponse<CheckInfoROD> result = await _meditor.Send(new Application.Queries.Reports.CheckInfoRODQuery(noSerie, windingMaterial, atPositions,
                    btPositions, terPositions, isAT, isBT, isTer));

                return new JsonResult(new ApiResponse<CheckInfoRODDto>(result.Code, result.Description, _mapper.Map<CheckInfoRODDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("checkInfoPce/{noSerie}/{capacity}/{atPositions}/{btPositions}/{terPositions}/{isAT}/{isBT}/{isTer}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<CheckInfoROD>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CheckInfoPce(string noSerie, string capacity, string atPositions, string btPositions, string terPositions,
       bool isAT, bool isBT, bool isTer)
        {
            try
            {
                ApiResponse<CheckInfoROD> result = await _meditor.Send(new Application.Queries.Reports.CheckInfoPCEQuery(noSerie, capacity, atPositions,
                    btPositions, terPositions, isAT, isBT, isTer));

                return new JsonResult(new ApiResponse<CheckInfoRODDto>(result.Code, result.Description, _mapper.Map<CheckInfoRODDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getTypeSectionConsolidatedReport/{noSerie}/{languaje}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<List<TypeConsolidatedReportDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTypeSectionConsolidatedReport(string noSerie, string languaje)
        {
            try
            {
                ApiResponse<List<TypeConsolidatedReport>> result = await _meditor.Send(new Application.Queries.Reports.GetTypeConsolidatedReportQuery(noSerie, languaje));
                return new JsonResult(new ApiResponse<List<TypeConsolidatedReportDto>>(result.Code, result.Description, _mapper.Map<List<TypeConsolidatedReportDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getTestedReport/{noSerie}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<List<ConsolidatedReportDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTestedReport(string noSerie)
        {
            try
            {
                ApiResponse<List<ConsolidatedReport>> result = await _meditor.Send(new Application.Queries.Reports.GetTestedReportQuery(noSerie));
                return new JsonResult(new ApiResponse<List<ConsolidatedReportDto>>(result.Code, result.Description, _mapper.Map<List<ConsolidatedReportDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveRDTReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveRDTReport(RDTReportDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportRDTCommand(_mapper.Map<RDTReport>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getColumsConfigFPC")]
        [ProducesResponseType(typeof(ApiResponse<List<ColumnTitleFPCReportsDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetColumsConfiFPC([FromQuery] string typeUnit, string lenguage)
        {
            try
            {
                ApiResponse<List<ColumnTitleFPCReports>> result = await _meditor.Send(new Application.Queries.Reports.GetColumsConfigFPCQuery(typeUnit, lenguage));
                return new JsonResult(new ApiResponse<List<ColumnTitleFPCReportsDto>>(result.Code, result.Description, _mapper.Map<List<ColumnTitleFPCReportsDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveRANReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveRANReport(RANReportDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportRANCommand(_mapper.Map<RANReport>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveFPCReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveFPCReport(FPCTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportFPCCommand(_mapper.Map<FPCTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveRCTReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveRCTReport(RCTTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportRCTCommand(_mapper.Map<RCTTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }
        [HttpPost("saveRODReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveRODReport(RODTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportRODCommand(_mapper.Map<RODTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveFPBReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveFPBReport(FPBTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportFPBCommand(_mapper.Map<FPBTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getPDFReports/{code}/{typeReport}")]
        [ProducesResponseType(typeof(ApiResponse<ReportPDFDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetPDFReports(long code, string typeReport)
        {
            try
            {
                ApiResponse<ReportPDF> result = await _meditor.Send(new Application.Queries.Reports.GetPDFReportQuery(code, typeReport));
                return new JsonResult(new ApiResponse<ReportPDFDto>(result.Code, result.Description, _mapper.Map<ReportPDFDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getInfoReportPCE/{NroSerie}/{KeyTest}/{Result}")]
        [ProducesResponseType(typeof(ApiResponse<PCETestsGeneralDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInfoReportPCE(string NroSerie, string KeyTest, bool Result)
        {
            try
            {
                ApiResponse<PCETestsGeneral> result = await _meditor.Send(new Application.Queries.Reports.GetInfoReportPCEQuery(NroSerie, KeyTest, Result));
                return new JsonResult(new ApiResponse<PCETestsGeneralDto>(result.Code, result.Description, _mapper.Map<PCETestsGeneralDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getInfoReportROD/{NroSerie}/{KeyTest}/{TestConnection}/{WindingMaterial}/{UnitOfMeasurement}/{Result}")]
        [ProducesResponseType(typeof(ApiResponse<RODTestsGeneralDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInfoReportROD(string NroSerie, string KeyTest, string TestConnection, string WindingMaterial, string UnitOfMeasurement, bool Result)
        {
            try
            {
                ApiResponse<RODTestsGeneral> result = await _meditor.Send(new Application.Queries.Reports.GetInfoReportRODQuery(NroSerie, KeyTest, TestConnection, WindingMaterial, UnitOfMeasurement, Result));
                return new JsonResult(new ApiResponse<RODTestsGeneralDto>(result.Code, result.Description, _mapper.Map<RODTestsGeneralDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getA/{noSerie}/{windingMaterial}/{capacity}/{atPositions}/{btPositions}/{terPositions}/{isAT}/{isBT}/{isTer}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PCIParameters>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PCIParameters>>> GetA(string noSerie, string windingMaterial, string capacity, string atPositions, string btPositions, string terPositions, bool isAT, bool isBT, bool isTer)
        {
            try
            {
                ApiResponse<IEnumerable<PCIParameters>> parameters = await _meditor.Send(new Application.Queries.Reports.GetAQuery(
                    noSerie,
                    windingMaterial,
                    capacity,
                    atPositions,
                    btPositions,
                    terPositions,
                    isAT,
                    isBT,
                    isTer));

                return Ok(parameters);
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("savePCIReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SavePCIReport(PCITestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportPCICommand(_mapper.Map<PCITestGeneral>(viewModel)));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("savePCEReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SavePCEReport(PCETestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportPCECommand(_mapper.Map<PCETestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("savePLRReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SavePLRReport(PLRTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportPLRCommand(_mapper.Map<PLRTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("savePRDReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SavePRDReport(PRDTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportPRDCommand(_mapper.Map<PRDTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("savePEEReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SavePEEReport(PEETestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportPEECommand(_mapper.Map<PEETestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveISZReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveISZReport(ISZTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportISZCommand(_mapper.Map<ISZTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveRYEReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveRYEReport(RYETestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportRYECommand(_mapper.Map<RYETestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("GetInfoReportPCI/{NroSerie}/{KeyTest}/{Result}")]
        [ProducesResponseType(typeof(ApiResponse<PCITestsGeneralDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInfoReportPCI(string NroSerie, string KeyTest, bool Result)
        {
            try
            {
                ApiResponse<PCITestGeneral> result = await _meditor.Send(new Application.Queries.Reports.GetInfoReportPCIQuery(NroSerie, KeyTest, Result));
                return new JsonResult(new ApiResponse<PCITestsGeneralDto>(result.Code, result.Description, _mapper.Map<PCITestsGeneralDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("GetInfoReportPEE/{NroSerie}/{KeyTest}/{Result}")]
        [ProducesResponseType(typeof(ApiResponse<PEETestsGeneralDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInfoReportPEE(string NroSerie, string KeyTest, bool Result)
        {
            try
            {
                ApiResponse<PEETestsGeneral> result = await _meditor.Send(new Application.Queries.Reports.GetInfoReportPEEQuery(NroSerie, KeyTest, Result));
                return new JsonResult(new ApiResponse<PEETestsGeneralDto>(result.Code, result.Description, _mapper.Map<PEETestsGeneralDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("savePIRReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SavePIRReport(PIRTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportPIRCommand(_mapper.Map<PIRTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("savePIMReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SavePIMReport(PIMTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportPIMCommand(_mapper.Map<PIMTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getInfoReportFPC/{NroSerie}/{KeyTest}/{Lenguage}/{UnitType}/{Frecuency}/{Result}")]
        [ProducesResponseType(typeof(ApiResponse<FPCTestsGeneralDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInfoReportFPC(string NroSerie, string KeyTest, string Lenguage, string UnitType, decimal Frecuency, bool Result)
        {
            try
            {
                ApiResponse<FPCTestsGeneral> result = await _meditor.Send(new Application.Queries.Reports.GetInfoReportFPCQuery(NroSerie, KeyTest, Lenguage, UnitType, Frecuency, Result));

                return new JsonResult(new ApiResponse<FPCTestsGeneralDto>(result.Code, result.Description, _mapper.Map<FPCTestsGeneralDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveTINReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveTINReport(TINTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportTINCommand(_mapper.Map<TINTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveTAPReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveTAPReport(TAPTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportTAPCommand(_mapper.Map<TAPTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getColumsConfigCEM/{typeTrafoId}/{keyLenguage}/{pos}/{posSecundary}/{noSerieNormal}")]
        [ProducesResponseType(typeof(ApiResponse<List<ColumnTitleCEMReportsDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetColumsConfigCEM(decimal typeTrafoId, string keyLenguage, string pos, string posSecundary, string noSerieNormal)
        {
            try
            {
                ApiResponse<List<ColumnTitleCEMReports>> result = await _meditor.Send(new Application.Queries.Reports.GetColumsConfigCEMQuery(typeTrafoId, keyLenguage, pos, posSecundary, noSerieNormal));
                return new JsonResult(new ApiResponse<List<ColumnTitleCEMReportsDto>>(result.Code, result.Description, _mapper.Map<List<ColumnTitleCEMReportsDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveCEMReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveCEMReport(CEMTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportCEMCommand(_mapper.Map<CEMTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveTDPReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        //[ValidateDTO]
        public async Task<ActionResult> SaveTDPReport(TDPTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportTDPCommand(_mapper.Map<TDPTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveCGDReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveCGDReport(CGDTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportCGDCommand(_mapper.Map<CGDTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveARFReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveARFReport(ARFTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportARFCommand(_mapper.Map<ARFTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveRDDReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveRDDReport(RDDTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportRDDCommand(_mapper.Map<RDDTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getInfoReportCGD/{NroSerie}/{KeyTests}/{Result}")]
        [ProducesResponseType(typeof(ApiResponse<CGDTestsGeneralDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInfoReportCGD(string NroSerie, string KeyTests, bool Result)
        {
            try
            {
                ApiResponse<CGDTestsGeneral> result = await _meditor.Send(new Application.Queries.Reports.GetInfoReportCGDQuery(NroSerie, KeyTests, Result));
                return new JsonResult(new ApiResponse<CGDTestsGeneralDto>(result.Code, result.Description, _mapper.Map<CGDTestsGeneralDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getInfoReportRCT/{NroSerie}/{KeyTests}/{Result}")]
        [ProducesResponseType(typeof(ApiResponse<RCTTestsGeneralDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInfoReportRCT(string NroSerie, string KeyTests, bool Result)
        {
            try
            {
                ApiResponse<RCTTestsGeneral> result = await _meditor.Send(new Application.Queries.Reports.GetInfoReportRCTQuery(NroSerie, KeyTests, Result));
                return new JsonResult(new ApiResponse<RCTTestsGeneralDto>(result.Code, result.Description, _mapper.Map<RCTTestsGeneralDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveFPAReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveFPAReport(FPATestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportFPACommand(_mapper.Map<FPATestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveINDReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveINDReport(INDTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportINDCommand(_mapper.Map<INDTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveBPCReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveBPCReport(BPCTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportBPCCommand(_mapper.Map<BPCTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveNRAReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveNRAReport(NRATestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportNRACommand(_mapper.Map<NRATestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveDPRReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [ValidateDTO]
        public async Task<ActionResult> SaveDPRReport(DPRTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportDPRCommand(_mapper.Map<DPRTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveETDReport")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveETDReport(ETDReportDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Artifact.Application.Commands.Reports.SaveReportETDCommand(_mapper.Map<ETDReport>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {

                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("GetConfigFileETD")]
        [ProducesResponseType(typeof(ApiResponse<List<ETDConfigFileReport>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigFileETD()
        {
            try
            {
                ApiResponse<List<ETDConfigFileReport>> result = await _meditor.Send(new Application.Queries.Reports.GetConfigFileETDQuery());
                return new JsonResult(new ApiResponse<List<ETDConfigFileReportDto>>(result.Code, result.Description, _mapper.Map<List<ETDConfigFileReportDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }
    }
}
