namespace SPL.Configuration.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using global::AutoMapper;

    using MediatR;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using SPL.Configuration.Api.DTOs.Configuration;
    using SPL.Configuration.Api.DTOs.Filters;
    using SPL.Configuration.Application.Commands.Configuration;
    using SPL.Configuration.Application.Queries.Configuration;
    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        #region Fields
        private readonly IMediator _meditor;
        private readonly ILogger<ConfigurationController> _logger;
        private readonly IMapper _mapper;
        #endregion

        public ConfigurationController(ILogger<ConfigurationController> logger, IMapper mapper, IMediator meditor)
        {
            _logger = logger;
            _mapper = mapper;
            _meditor = meditor;
        }

        [HttpGet("getCorrectionFactorSpecification/{Specification}/{Temperature}/{CorrectionFactor}")]
        [ProducesResponseType(typeof(IEnumerable<CorrectionFactorSpecificationDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCorrectionFactorSpecification(string Specification, decimal Temperature, decimal CorrectionFactor)
        {
            try
            {
                ApiResponse<List<CorrectionFactorSpecification>> result = await _meditor.Send(new GetCorrectionFactorSpecificationQuery(Specification,
                        Temperature, CorrectionFactor));
                return new JsonResult(new ApiResponse<IEnumerable<CorrectionFactorSpecificationDto>>(result.Code, result.Description, _mapper.Map<IEnumerable<CorrectionFactorSpecificationDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveCorrectionFactorSpecification")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveCorrectionFactorSpecification([FromBody] CorrectionFactorSpecificationDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new SaveCorrectionFactorSpecificationCommand(_mapper.Map<CorrectionFactorSpecification>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("deleteCorrectionFactorSpecification")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteCorrectionFactorSpecification([FromBody] CorrectionFactorSpecificationDto viewModel)
        {
            try
            {

                ApiResponse<long> result = await _meditor.Send(new DeleteCorrectionFactorSpecificationCommand(_mapper.Map<CorrectionFactorSpecification>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNozzleMarks/{pIdMark}/{pStatus}")]
        [ProducesResponseType(typeof(IEnumerable<NozzleMarksDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNozzleMarks([FromRoute] long pIdMark, bool pStatus)
        {
            try
            {
                ApiResponse<List<NozzleMarks>> result = await _meditor.Send(new GetNozzleMarksQuery(pIdMark, pStatus));
                return new JsonResult(new ApiResponse<IEnumerable<NozzleMarksDto>>(result.Code, result.Description, _mapper.Map<IEnumerable<NozzleMarksDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getTypeXMarksNozzle/{pIdMark}/{pStatus}")]
        [ProducesResponseType(typeof(IEnumerable<TypesNozzleMarksDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTypeXMarksNozzle([FromRoute] long pIdMark, bool pStatus)
        {
            try
            {
                ApiResponse<List<TypesNozzleMarks>> result = await _meditor.Send(new GetTypeXMarksNozzleQuery(pIdMark, pStatus));
                return new JsonResult(new ApiResponse<IEnumerable<TypesNozzleMarksDto>>(result.Code, result.Description, _mapper.Map<IEnumerable<TypesNozzleMarksDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getCorrectionFactorsXMarksXTypes")]
        [ProducesResponseType(typeof(IEnumerable<CorrectionFactorsXMarksXTypesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCorrectionFactorsXMarksXTypes()
        {
            try
            {
                ApiResponse<List<CorrectionFactorsXMarksXTypes>> result = await _meditor.Send(new GetCorrectionFactorsXMarksXTypesQuery());
                return new JsonResult(new ApiResponse<IEnumerable<CorrectionFactorsXMarksXTypesDto>>(result.Code, result.Description, _mapper.Map<IEnumerable<CorrectionFactorsXMarksXTypesDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveCorrectionFactorsXMarksXTypes")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> saveCorrectionFactorsXMarksXTypes([FromBody] CorrectionFactorsXMarksXTypesDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new SaveCorrectionFactorsXMarksXTypesCommand(_mapper.Map<CorrectionFactorsXMarksXTypes>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("deleteCorrectionFactorsXMarksXTypes")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteCorrectionFactorsXMarksXTypes([FromBody] CorrectionFactorsXMarksXTypesDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new DeleteCorrectionFactorsXMarksXTypesCommand(_mapper.Map<CorrectionFactorsXMarksXTypes>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getCorrectionFactorsDesc")]
        [ProducesResponseType(typeof(CorrectionFactorsDescDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCorrectionFactorsDesc([FromQuery] string pSpecification, string pKeyLenguage)
        {
            try
            {
                ApiResponse<CorrectionFactorsDesc> result = await _meditor.Send(new GetCorrectionFactorsDescQuery(pSpecification, pKeyLenguage));
                return new JsonResult(new ApiResponse<CorrectionFactorsDescDto>(result.Code, result.Description, _mapper.Map<CorrectionFactorsDescDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNozzleTypesByBrand/{pIdMark}")]
        [ProducesResponseType(typeof(IEnumerable<NozzleMarksDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNozzleTypesByBrand([FromRoute] long pIdMark)
        {
            try
            {
                ApiResponse<List<TypesNozzleMarks>> result = await _meditor.Send(new GetNozzleTypesByBrandQuery(pIdMark));
                return new JsonResult(new ApiResponse<IEnumerable<TypesNozzleMarksDto>>(result.Code, result.Description, _mapper.Map<IEnumerable<TypesNozzleMarksDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("deleteNozzleTypesByBrand")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteNozzleTypesByBrand([FromBody] TypesNozzleMarksDto viewModel)
        {
            try
            {

                ApiResponse<long> result = await _meditor.Send(new DeleteNozzleTypesByBrandCommand(_mapper.Map<TypesNozzleMarks>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveNozzleTypesByBrand")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveNozzleTypesByBrand([FromBody] TypesNozzleMarksDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new SaveNozzleTypesByBrandCommand(_mapper.Map<TypesNozzleMarks>(viewModel)));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getNozzleBrands/{pIdMark}")]
        [ProducesResponseType(typeof(IEnumerable<NozzleMarksDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetNozzleBrands([FromRoute] long pIdMark)
        {
            try
            {
                ApiResponse<List<NozzleMarks>> result = await _meditor.Send(new GetNozzleBrandsQuery(pIdMark));
                return new JsonResult(new ApiResponse<IEnumerable<NozzleMarksDto>>(result.Code, result.Description, _mapper.Map<IEnumerable<NozzleMarksDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("deleteNozzleBrands")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteNozzleBrands([FromBody] NozzleMarksDto viewModel)
        {
            try
            {

                ApiResponse<long> result = await _meditor.Send(new DeleteNozzleBrandsCommand(_mapper.Map<NozzleMarks>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveNozzleBrands")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveNozzleBrands([FromBody] NozzleMarksDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new SaveNozzleBrandsCommand(_mapper.Map<NozzleMarks>(viewModel)));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getValidationTestsISZ")]
        [ProducesResponseType(typeof(ApiResponse<List<ValidationTestsIszDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetValidationTestsISZ()
        {
            try
            {
                ApiResponse<List<ValidationTestsIsz>> result = await _meditor.Send(new GetValidationTestsISZ());

                return new JsonResult(new ApiResponse<List<ValidationTestsIszDto>>(result.Code, result.Description, _mapper.Map<List<ValidationTestsIszDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetValidationTestsISZ));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getInfoContGasCGD/{IdReport}/{KeyTests}/{TypeOil}")]
        [ProducesResponseType(typeof(IEnumerable<ContGasCGDDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInfoContGasCGD([FromRoute] string IdReport, string KeyTests, string TypeOil)
        {
            try
            {
                ApiResponse<List<ContGasCGD>> result = await _meditor.Send(new GetInfoContGasCGDQuery(IdReport, KeyTests, TypeOil));
                return new JsonResult(new ApiResponse<IEnumerable<ContGasCGDDto>>(result.Code, result.Description, _mapper.Map<IEnumerable<ContGasCGDDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveInfoContGasCGD")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveInfoContGasCGD([FromBody] ContGasCGDDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new SaveInfoContGasCGDCommand(_mapper.Map<ContGasCGD>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("deleteInfoContGasCGD")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteInfoContGasCGD([FromBody] ContGasCGDDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new DeleteInfoContGasCGDCommand(_mapper.Map<ContGasCGD>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getInformationOctaves")]
        [ProducesResponseType(typeof(IEnumerable<InformationOctavesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInformationOctaves([FromQuery] string NroSerie, string TypeInformation, string DateData)
        {
            try
            {
                ApiResponse<List<InformationOctaves>> result = await _meditor.Send(new GetInformationOctavesQuery(NroSerie, TypeInformation, DateData));

                return new JsonResult(new ApiResponse<List<InformationOctavesDto>>(result.Code, result.Description, _mapper.Map<List<InformationOctavesDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("importinformationoctaves")]
        [AllowAnonymous]
        [ValidateDTO]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> ImportInformationOctaves([FromBody] List<InformationOctavesDto> viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new ImportInformationOctavesCommand(_mapper.Map<List<InformationOctaves>>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("updateinformationoctaves")]
        [AllowAnonymous]
        [ValidateDTO]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateInformationOctaves([FromBody] List<InformationOctavesDto> viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new UpdateInformationOctavesCommand(_mapper.Map<List<InformationOctaves>>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getStabilizationDesignData/{NroSerie}")]
        [ProducesResponseType(typeof(StabilizationDesignDataDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetStabilizationDesignData(string NroSerie)
        {
            try
            {
                ApiResponse<StabilizationDesignData> result = await _meditor.Send(new GetStabilizationDesignQuery(NroSerie));
                return new JsonResult(new ApiResponse<StabilizationDesignDataDto>(result.Code, result.Description, _mapper.Map<StabilizationDesignDataDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveStabilizationDesignData")]
        [AllowAnonymous]
        [ValidateDTO]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> saveStabilizationDesignData([FromBody] StabilizationDesignDataDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new SaveStabilizationDesignDataCommand(_mapper.Map<StabilizationDesignData>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getCorrectionFactorkWTypeCooling")]
        [ProducesResponseType(typeof(IEnumerable<CorrectionFactorkWTypeCoolingDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCorrectionFactorkWTypeCooling()
        {
            try
            {
                ApiResponse<List<CorrectionFactorkWTypeCooling>> result = await _meditor.Send(new GetCorrectionFactorkWTypeCoolingQuery());
                return new JsonResult(new ApiResponse<IEnumerable<CorrectionFactorkWTypeCoolingDto>>(result.Code, result.Description, _mapper.Map<IEnumerable<CorrectionFactorkWTypeCoolingDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveCorrectionFactorkWTypeCooling")]
        [AllowAnonymous]
        [ValidateDTO]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> saveCorrectionFactorkWTypeCooling([FromBody] List<CorrectionFactorkWTypeCoolingDto> viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new SaveCorrectionFactorkWTypeCoolingCommand(_mapper.Map<List<CorrectionFactorkWTypeCooling>>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getStabilizationData")]
        [ProducesResponseType(typeof(List<StabilizationDataDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetStabilizationData([FromQuery] string NroSerie, bool? Status, bool? Stabilized)
        {
            try
            {
                ApiResponse<List<StabilizationData>> result = await _meditor.Send(new GetStabilizationQuery(NroSerie, Status, Stabilized));
                return new JsonResult(new ApiResponse<List<StabilizationDataDto>>(result.Code, result.Description, _mapper.Map<List<StabilizationDataDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveStabilizationData")]
        [AllowAnonymous]
        [ValidateDTO]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> saveStabilizationData([FromBody] StabilizationDataDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new SaveStabilizationDataCommand(_mapper.Map<StabilizationData>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getInformationLaboratories")]
        [ProducesResponseType(typeof(List<InformationLaboratoriesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInformationLaboratories()
        {
            try
            {
                ApiResponse<List<InformationLaboratories>> result = await _meditor.Send(new GetInformationLaboratoriesQuery());
                return new JsonResult(new ApiResponse<List<InformationLaboratoriesDto>>(result.Code, result.Description, _mapper.Map<List<InformationLaboratoriesDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getCuttingDatas/{NroSerie}")]
        [ProducesResponseType(typeof(HeaderCuttingDataDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCuttingDatas(string NroSerie)
        {
            try
            {
                ApiResponse<List<HeaderCuttingData>> result = await _meditor.Send(new GetCuttingDatasQuery(NroSerie));

                return new JsonResult(new ApiResponse<List<HeaderCuttingDataDto>>(result.Code, result.Description, _mapper.Map<List<HeaderCuttingDataDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getInfoHeaderCuttingData/{IdCut}")]
        [ProducesResponseType(typeof(HeaderCuttingDataDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInfoHeaderCuttingData(decimal IdCut)
        {
            try
            {
                ApiResponse<HeaderCuttingData> result = await _meditor.Send(new GetInfoHeaderCuttingDataQuery(IdCut));

                return new JsonResult(new ApiResponse<HeaderCuttingDataDto>(result.Code, result.Description, _mapper.Map<HeaderCuttingDataDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveCuttingData")]
        [AllowAnonymous]
        [ValidateDTO]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> saveCuttingData([FromBody] HeaderCuttingDataDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new SaveCuttingDataCommand(_mapper.Map<HeaderCuttingData>(viewModel)));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("closeStabilizationData")]
        [AllowAnonymous]
        [ValidateDTO]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> closeStabilizationData([FromBody] string NroSerie, decimal IdReg)
        {
            try
            {
                ApiResponse<long> result = await _meditor.Send(new CloseStabilizationDataCommand(NroSerie, IdReg));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, _mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }
    }
}
