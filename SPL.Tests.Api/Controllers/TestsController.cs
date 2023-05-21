namespace SPL.Tests.Api.Controllers
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
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.CGD;
    using SPL.Domain.SPL.Tests.DPR;
    using SPL.Domain.SPL.Tests.ETD;
    using SPL.Domain.SPL.Tests.FPA;
    using SPL.Domain.SPL.Tests.FPB;
    using SPL.Domain.SPL.Tests.FPC;
    using SPL.Domain.SPL.Tests.ISZ;
    using SPL.Domain.SPL.Tests.NRA;
    using SPL.Domain.SPL.Tests.PCE;
    using SPL.Domain.SPL.Tests.PCI;
    using SPL.Domain.SPL.Tests.PEE;
    using SPL.Domain.SPL.Tests.PLR;
    using SPL.Domain.SPL.Tests.PRD;
    using SPL.Domain.SPL.Tests.RCT;
    using SPL.Domain.SPL.Tests.RDD;
    using SPL.Domain.SPL.Tests.ROD;
    using SPL.Domain.SPL.Tests.TAP;
    using SPL.Domain.SPL.Tests.TDP;
    using SPL.Tests.Api.DTOs.Tests;
    using SPL.Tests.Api.DTOs.Tests.CGD;
    using SPL.Tests.Api.DTOs.Tests.CuttingData;
    using SPL.Tests.Api.DTOs.Tests.DPR;
    using SPL.Tests.Api.DTOs.Tests.ETD;
    using SPL.Tests.Api.DTOs.Tests.FPA;
    using SPL.Tests.Api.DTOs.Tests.FPB;
    using SPL.Tests.Api.DTOs.Tests.FPC;
    using SPL.Tests.Api.DTOs.Tests.ISZ;
    using SPL.Tests.Api.DTOs.Tests.NRA;
    using SPL.Tests.Api.DTOs.Tests.PCE;
    using SPL.Tests.Api.DTOs.Tests.PCI;
    using SPL.Tests.Api.DTOs.Tests.PEE;
    using SPL.Tests.Api.DTOs.Tests.PLR;
    using SPL.Tests.Api.DTOs.Tests.PRD;
    using SPL.Tests.Api.DTOs.Tests.RCT;
    using SPL.Tests.Api.DTOs.Tests.RDD;
    using SPL.Tests.Api.DTOs.Tests.ROD;
    using SPL.Tests.Api.DTOs.Tests.RYE;
    using SPL.Tests.Api.DTOs.Tests.TAP;
    using SPL.Tests.Api.DTOs.Tests.TDP;
    using SPL.Tests.Application.Commands.Tests;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TestsController : ControllerBase
    {
        #region Fields
        private readonly IMediator _meditor;
        private readonly ILogger<TestsController> _logger;
        private readonly IMapper _mapper;
        #endregion

        public TestsController(ILogger<TestsController> logger, IMapper mapper, IMediator meditor)
        {
            _logger = logger;
            _mapper = mapper;
            _meditor = meditor;
        }

        [HttpGet("{typeReport}/{keyTest}")]
        [ProducesResponseType(typeof(ApiResponse<List<TestsDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTestsQuery(string typeReport, string keyTest)
        {
            try
            {
                ApiResponse<List<Tests>> result = await _meditor.Send(new Application.Queries.Tests.GetTestsQuery(typeReport, keyTest));

                return new JsonResult(new ApiResponse<List<TestsDto>>(result.Code, result.Description, _mapper.Map<List<TestsDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetTestsQuery));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextRDT/{nroSerie}/{keyTest}/{dAngular}/{rule}/{lenguage}")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextRDT(string nroSerie, string keyTest, string dAngular, string rule, string lenguage)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextRDTQuery(nroSerie, keyTest, dAngular, rule, lenguage));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextRDT));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextRAD/{nroSerie}/{keyTest}/{typeUnit}/{thirdWinding}/{lenguage}")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextRAD(string nroSerie, string keyTest, string typeUnit, string thirdWinding, string lenguage)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextRADQuery(nroSerie, keyTest, typeUnit, thirdWinding, lenguage));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextRAD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("calculeTestRAD")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestRAD([FromBody] RADTestsDto viewModel)
        {

            try
            {
                ApiResponse<ResultRADTests> result = await _meditor.Send(new Application.Commands.Tests.RADTestsCommand(_mapper.Map<RADTests>(viewModel)));
                return new JsonResult(new ApiResponse<ResultRADTestsDto>(result.Code, result.Description, _mapper.Map<ResultRADTestsDto>(result.Structure)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestRAD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestRDT")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestRDT([FromBody] RDTTestsDto viewModel)
        {

            try
            {
                ApiResponse<ResultRDTTestsDetails> result = await _meditor.Send(new Application.Commands.Tests.RDTTestsCommand(_mapper.Map<RDTTests>(viewModel)));
                return new JsonResult(new ApiResponse<ResultRDTTestsDetailsDto>(result.Code, result.Description, _mapper.Map<ResultRDTTestsDetailsDto>(result.Structure)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestRDT));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextRAN")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextRAN([FromQuery] string nroSerie, string keyTest, string lenguage, int numberMeasurements)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextRANQuery(nroSerie, keyTest, lenguage, numberMeasurements));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextRAN));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("GetNumNextFPC")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextFPC([FromQuery] string nroSerie, string keyTest, string typeUnit, string specification, string frequency, string lenguage)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextFPCQuery(nroSerie, keyTest, typeUnit, specification, frequency, lenguage));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextFPC));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("GetNumNextROD")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextROD([FromQuery] string noSerie, string keyTest, string lenguage, string connection, string unitType, string material, string unitOfMeasurement)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextRODQuery(noSerie, keyTest, lenguage, connection, unitType, material, unitOfMeasurement));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextROD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestRAN")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<RANTestsDetailsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestRAN([FromBody] List<RANTestsDetailsDto> viewModel)
        {
            try
            {
                ApiResponse<ResultRANTests> result = await _meditor.Send(new Application.Commands.Tests.RANTestsCommand(_mapper.Map<List<RANTestsDetails>>(viewModel)));

                return new JsonResult(new ApiResponse<ResultRANTestsDto>(result.Code, result.Description, _mapper.Map<ResultRANTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestRAN));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestFPC")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<FPCTestsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestFPC([FromBody] List<FPCTestsDto> viewModel)
        {
            try
            {
                ApiResponse<ResultFPCTests> result = await _meditor.Send(new Application.Commands.Tests.FPCTestsCommand(_mapper.Map<List<FPCTests>>(viewModel)));

                return new JsonResult(new ApiResponse<ResultFPCTestsDto>(result.Code, result.Description, _mapper.Map<ResultFPCTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestFPC));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("GetNumNextFPB")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextFPB([FromQuery] string nroSerie, string keyTest, string lenguage, string tangentDelta)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextFPBQuery(nroSerie, keyTest, lenguage, tangentDelta));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextFPB));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("GetNumNextRCT")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextRCT([FromQuery] string nroSerie, string keyTest, string lenguage, string unitOfMeasurement, string tertiary, decimal testvoltage)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextRCTQuery(nroSerie, keyTest, lenguage, unitOfMeasurement, tertiary, testvoltage));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextRCT));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestFPB")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<FPBTestsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestFPB([FromBody] List<FPBTestsDto> viewModel)
        {
            try
            {
                ApiResponse<ResultFPBTests> result = await _meditor.Send(new Application.Commands.Tests.FPBTestsCommand(_mapper.Map<List<FPBTests>>(viewModel)));

                return new JsonResult(new ApiResponse<ResultFPBTestsDto>(result.Code, result.Description, _mapper.Map<ResultFPBTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestFPB));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestRCT")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultRCTTestsDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestRCT([FromBody] List<RCTTestsDto> viewModel)
        {
            try
            {
                ApiResponse<ResultRCTTests> result = await _meditor.Send(new Application.Commands.Tests.RCTTestsCommand(_mapper.Map<List<RCTTests>>(viewModel)));

                return new JsonResult(new ApiResponse<ResultRCTTestsDto>(result.Code, result.Description, _mapper.Map<ResultRCTTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestRCT));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestROD")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<RODTestsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestROD([FromBody] List<RODTestsDto> viewModel)
        {
            try
            {
                ApiResponse<ResultRODTests> result = await _meditor.Send(new Application.Commands.Tests.RODTestsCommand(_mapper.Map<List<RODTests>>(viewModel)));

                return new JsonResult(new ApiResponse<ResultRODTestsDto>(result.Code, result.Description, _mapper.Map<ResultRODTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestROD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextPCE")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextPCE([FromQuery] string nroSerie, string keyTest, string lenguage, string energizedWinding, decimal lostWarranty, string umGarPerD, decimal successGuarantee, string umGarExcitacion)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextPCEQuery(nroSerie, keyTest, lenguage, energizedWinding));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextPCE));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextPCI")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> getNumNextPCI([FromQuery] string nroSerie, string keyTest, string lenguage, string windingMaterial, bool capRedBaja, bool autotransformer, bool monofasico, decimal overElevation, string posPi, string posSec)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextPCIQuery(nroSerie, keyTest, lenguage, windingMaterial, capRedBaja, autotransformer, monofasico, overElevation, posPi, posSec));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(getNumNextPCI));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestPCI")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultPCITestDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestPCI([FromBody] PCITestsCommand command)
        {
            try
            {
                ApiResponse<PCITestResponse> result = await _meditor.Send(command);

                return new JsonResult(new ApiResponse<ResultPCITestDto>(result.Code, result.Description, _mapper.Map<ResultPCITestDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestPCI));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("calculeTestPCE")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultPCETestsDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestPCE([FromBody] List<PCETestsDto> viewModel)
        {
            try
            {
                ApiResponse<ResultPCETests> result = await _meditor.Send(new Application.Commands.Tests.PCETestsCommand(_mapper.Map<List<PCETests>>(viewModel)));

                return new JsonResult(new ApiResponse<ResultPCETestsDto>(result.Code, result.Description, _mapper.Map<ResultPCETestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestPCE));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextPRD")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> getNumNextPRD([FromQuery] string nroSerie, string keyTest, string lenguage, decimal nominalVoltage)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextPRDQuery(nroSerie, keyTest, lenguage, nominalVoltage));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(getNumNextPRD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextPLR")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextPLR([FromQuery] string nroSerie, string keyTest, string lenguage, decimal rldnt, decimal nominalVoltage, int amountOfTensions)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextPLRQuery(nroSerie, keyTest, lenguage, rldnt, nominalVoltage, amountOfTensions));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextPLR));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestPLR")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultPLRTestsDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestPLR([FromBody] PLRTestsDto viewModel)
        {
            try
            {
                ApiResponse<ResultPLRTests> result = await _meditor.Send(new Application.Commands.Tests.PLRTestsCommand(_mapper.Map<PLRTests>(viewModel)));

                return new JsonResult(new ApiResponse<ResultPLRTestsDto>(result.Code, result.Description, _mapper.Map<ResultPLRTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestPLR));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestPRD")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultPLRTestsDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestPRD([FromBody] PRDTestsDto viewModel)
        {
            try
            {
                ApiResponse<ResultPRDTests> result = await _meditor.Send(new Application.Commands.Tests.PRDTestsCommand(_mapper.Map<PRDTests>(viewModel)));

                return new JsonResult(new ApiResponse<ResultPRDTestsDto>(result.Code, result.Description, _mapper.Map<ResultPRDTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestPRD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextPEE")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextPEE([FromQuery] string nroSerie, string keyTest, string lenguage)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextPEEQuery(nroSerie, keyTest, lenguage));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextPEE));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestPEE")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultPEETestsDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestPEE([FromBody] PEETestsDto viewModel)
        {
            try
            {
                ApiResponse<ResultPEETests> result = await _meditor.Send(new Application.Commands.Tests.PEETestsCommand(_mapper.Map<Domain.SPL.Tests.PEE.PEETests>(viewModel)));

                return new JsonResult(new ApiResponse<ResultPEETestsDto>(result.Code, result.Description, _mapper.Map<ResultPEETestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestPRD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextISZ")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextISZ([FromQuery] string nroSerie, string keyTest, string lenguage,
           decimal degreesCor, string posAT, string posBT, string posTER, string windingEnergized, int qtyNeutral, decimal impedanceGar, string materialWinding)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextISZQuery(nroSerie, keyTest, lenguage, degreesCor, posAT, posBT, posTER, windingEnergized, qtyNeutral, impedanceGar, materialWinding));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextISZ));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextRYE")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextRYE([FromQuery] string nroSerie, string keyTest, string lenguage,
            int noConnectionsAT, int noConnectionsBT, int noConnectionsTER, decimal voltageAT, decimal voltageBT, decimal voltageTER, string coolingType)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextRYEQuery(nroSerie, keyTest, lenguage, noConnectionsAT, noConnectionsBT, noConnectionsTER, voltageAT, voltageBT, voltageTER, coolingType));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextISZ));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestISZ")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultISZTestsDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestISZ([FromBody] OutISZTestsDto viewModel)
        {
            try
            {
                ApiResponse<ResultISZTests> result = await _meditor.Send(new Application.Commands.Tests.ISZTestsCommand(_mapper.Map<OutISZTests>(viewModel)));

                return new JsonResult(new ApiResponse<ResultISZTestsDto>(result.Code, result.Description, _mapper.Map<ResultISZTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestISZ));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestRYE")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultRYETestsDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestRYE([FromBody] OutRYETestsDto viewModel)
        {
            try
            {
                ApiResponse<ResultRYETests> result = await _meditor.Send(new Application.Commands.Tests.RYETestsCommand(_mapper.Map<OutRYETests>(viewModel)));

                return new JsonResult(new ApiResponse<ResultRYETestsDto>(result.Code, result.Description, _mapper.Map<ResultRYETestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestRYE));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextPIM")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextPIM([FromQuery] string nroSerie, string keyTest, string lenguage,
          string connection, string applyLow, string voltageLevel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextPIMQuery(nroSerie, keyTest, lenguage, connection, applyLow, voltageLevel));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextPIM));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextPIR")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextPIR([FromQuery] string nroSerie, string keyTest, string lenguage,
          string connection, string includesTertiary, string voltageLevel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextPIRQuery(nroSerie, keyTest, lenguage, connection, includesTertiary, voltageLevel));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextPIM));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextTAP")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextTAP([FromQuery] string nroSerie, string keyTest, string lenguage,
      string unitType, int noConnectionAT, int noConnectionBT, int noConnectionTER, string idCapAT, string idCapBT, string idCapTer)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextTAPQuery(nroSerie, keyTest, lenguage, unitType, noConnectionAT, noConnectionBT, noConnectionTER, idCapAT, idCapBT, idCapTer));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextPIM));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextTIN")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextTIN([FromQuery] string nroSerie, string keyTest, string lenguage,
        string connection, decimal voltage)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextTINQuery(nroSerie, keyTest, lenguage, connection, voltage));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextPIM));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextCEM")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextCEM([FromQuery] string nroSerie, string keyTest, string lenguage,
       string idPosPrimary, string posPrimary, string idPosSecundary, string posSecundary, decimal testsVoltage)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextCEMQuery(nroSerie, keyTest, lenguage, idPosPrimary, posPrimary, idPosSecundary, posSecundary, testsVoltage));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextPIM));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("CalculeTestTAP")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultTAPTestsDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestTAP([FromBody] TAPTestsDto viewModel)
        {
            try
            {
                ApiResponse<ResultTAPTests> result = await _meditor.Send(new Application.Commands.Tests.TAPTestsCommand(_mapper.Map<TAPTests>(viewModel)));

                return new JsonResult(new ApiResponse<ResultTAPTestsDto>(result.Code, result.Description, _mapper.Map<ResultTAPTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestTAP));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextCGD")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextCGD([FromQuery] string nroSerie, string keyTest, string lenguage, string typeOil)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextCGDQuery(nroSerie, keyTest, lenguage, typeOil));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextCGD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextTDP")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextTDP([FromQuery] string nroSerie, string keyTest, string lenguage,
int noCycles, int totalTime, int interval, decimal timeLevel, decimal outputLevel, int descMayPc, int descMayMv, int incMaxPc, string voltageLevels, string measurementType, string terminalsTest)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextTDPQuery(nroSerie, keyTest, lenguage, noCycles, totalTime, interval, timeLevel, outputLevel, descMayPc, descMayMv, incMaxPc, voltageLevels, measurementType, terminalsTest));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextTDP));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextRDD")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextRDD([FromQuery] string nroSerie, string keyTest, string lenguage,
string config_Winding, string connection, decimal porc_Z, decimal porc_Jx)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextRDDQuery(nroSerie, keyTest, lenguage, config_Winding, connection, porc_Z, porc_Jx));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextRDD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextARF")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextARF([FromQuery] string nroSerie, string keyTest, string lenguage,
string team, string tertiary2Low, string tertiaryDisp, string levelsVoltage)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextARFQuery(nroSerie, keyTest, lenguage, team, tertiary2Low, tertiaryDisp, levelsVoltage));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextARF));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextIND")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextIND([FromQuery] string nroSerie, string keyTest, string lenguage,
string tcPurchased)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextINDQuery(nroSerie, keyTest, lenguage, tcPurchased));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextIND));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextFPA")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextFPA([FromQuery] string nroSerie, string keyTest, string lenguage, string typeOil)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextFPAQuery(nroSerie, keyTest, lenguage, typeOil));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextFPA));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextBPC")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextBPC([FromQuery] string nroSerie, string keyTest, string lenguage)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextBPCQuery(nroSerie, keyTest, lenguage));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextBPC));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("calculeTestTDP")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestTDP([FromBody] TDPTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<ResultTDPTests> result = await _meditor.Send(new Application.Commands.Tests.TDPTestsCommand(_mapper.Map<TDPTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<ResultTDPTestsDto>(result.Code, result.Description, _mapper.Map<ResultTDPTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestTDP));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("calculeTestFPA")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestFPA([FromBody] FPATestsDto viewModel)
        {
            try
            {
                ApiResponse<ResultFPATests> result = await _meditor.Send(new Application.Commands.Tests.FPATestsCommand(_mapper.Map<FPATests>(viewModel)));
                return new JsonResult(new ApiResponse<ResultFPATestsDto>(result.Code, result.Description, _mapper.Map<ResultFPATestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestFPA));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("calculeTestRDD")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestRDD([FromBody] RDDTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<ResultRDDTests> result = await _meditor.Send(new Application.Commands.Tests.RDDTestsCommand(_mapper.Map<RDDTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<ResultRDDTestsDto>(result.Code, result.Description, _mapper.Map<ResultRDDTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestRDD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("calculeTestCGD")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestCGD([FromBody] List<CGDTestsDto> viewModel)
        {
            try
            {
                ApiResponse<ResultCGDTests> result = await _meditor.Send(new Application.Commands.Tests.CGDTestsCommand(_mapper.Map<List<CGDTests>>(viewModel)));
                return new JsonResult(new ApiResponse<ResultCGDTestsDto>(result.Code, result.Description, _mapper.Map<ResultCGDTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestCGD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("GetNumNextNRA")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextNRA([FromQuery] string nroSerie, string keyTest, string language, string laboratory, string rule, string feeding, string coolingType)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextNRAQuery(nroSerie, keyTest, language, laboratory, rule, feeding, coolingType));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextNRA));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("GetNumNextETD")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextETD([FromQuery] string nroSerie, string keyTest, string language, bool typeRegression, bool btDifCap, decimal capacityBt, string tertiary2B, bool terCapRed, decimal capacityTer, string windingSplit)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextETDQuery(nroSerie, keyTest, language, typeRegression, btDifCap, capacityBt, tertiary2B, terCapRed, capacityTer, windingSplit));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextETD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNumNextDPR")]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNumNextDPR([FromQuery] string nroSerie, string keyTest, string lenguage, int noCycles,
int totalTime, int interval, decimal timeLevel, decimal outputLevel, int descMayPc, int descMayMv, int incMaxPc, string measurementType, string terminalsTest)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new Application.Queries.Tests.GetNumNextDPRQuery(nroSerie, keyTest, lenguage, noCycles, totalTime, interval, timeLevel, outputLevel, descMayPc, descMayMv, incMaxPc, measurementType, terminalsTest));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetNumNextDPR));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("calculeTestNRA")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestNRA([FromBody] NRATestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<ResultNRATests> result = await _meditor.Send(new Application.Commands.Tests.NRATestsCommand(_mapper.Map<NRATestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<ResultNRATestsDto>(result.Code, result.Description, _mapper.Map<ResultNRATestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestNRA));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("calculeTestStabilizationData")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestStabilizationData([FromBody] StabilizationDataDto viewModel)
        {
            try
            {
                ApiResponse<ResultStabilizationDataTests> result = await _meditor.Send(new Application.Commands.Tests.StabilizationDataTestsCommand(_mapper.Map<StabilizationData>(viewModel)));
                return new JsonResult(new ApiResponse<ResultStabilizationDataTestsDto>(result.Code, result.Description, _mapper.Map<ResultStabilizationDataTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestStabilizationData));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("calculeTestDPR")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestDPR([FromBody] DPRTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<ResultDPRTests> result = await _meditor.Send(new Application.Commands.Tests.DPRTestsCommand(_mapper.Map<DPRTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<ResultDPRTestsDto>(result.Code, result.Description, _mapper.Map<ResultDPRTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestDPR));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("calculeTestETD")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestETD([FromBody] ETDTestsGeneralDto viewModel)
        {
            try
            {
                ApiResponse<ResultETDTests> result = await _meditor.Send(new Application.Commands.Tests.ETDTestsCommand(_mapper.Map<ETDTestsGeneral>(viewModel)));
                return new JsonResult(new ApiResponse<ResultETDTestsDto>(result.Code, result.Description, _mapper.Map<ResultETDTestsDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestETD));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("calculeTestCuttingData")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<ResultCuttingDataTestsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CalculeTestCuttingData([FromBody] HeaderCuttingDataDto viewModel)
        {

            try
            {
                ApiResponse<ResultCuttingDataTests> result = await _meditor.Send(new Application.Commands.Tests.CuttingDataCommand(_mapper.Map<HeaderCuttingData>(viewModel)));
                return new JsonResult(new ApiResponse<ResultCuttingDataTestsDto>(result.Code, result.Description, _mapper.Map<ResultCuttingDataTestsDto>(result.Structure)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CalculeTestCuttingData));
                return new JsonResult(new ApiResponse<ResultCuttingDataTestsDto>(Enums.EnumsGen.Error, ex.Message, null));
            }
        }
    }
}
