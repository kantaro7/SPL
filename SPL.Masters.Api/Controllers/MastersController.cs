namespace SPL.Masters.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using global::AutoMapper;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using SPL.Domain;
    using Domain.SPL.Masters;
    using Application.Queries.Configuration;
    using SPL.Masters.Api.DTOs.Artifactdesign;
    using SPL.Masters.Api.DTOs.Configuration;
    using SPL.Masters.Application.Queries.Artifactdesign;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MastersController : ControllerBase
    {
        #region Fields
        private readonly IMediator _meditor;
        private readonly ILogger<MastersController> _logger;
        private readonly IMapper _mapper;
        #endregion

        public MastersController(ILogger<MastersController> logger, IMapper mapper, IMediator meditor)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._meditor = meditor;
        }

        [HttpGet("getCatSidco")]
        [ProducesResponseType(typeof(List<CatSidcoInformationDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCatSidco()
        {
            try
            {
                return new JsonResult(this._mapper.Map<List<CatSidcoInformationDto>>(await this._meditor.Send(new GetCatSidcoQuery())));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetCatSidco));
                throw;
            }
        }

        [HttpGet("getCatSidcoOthers")]
        [ProducesResponseType(typeof(List<CatSidcoOtherInformationDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCatSidcoOthers()
        {
            try
            {
                return new JsonResult(this._mapper.Map<List<CatSidcoOtherInformationDto>>(await this._meditor.Send(new GetCatSidcoOtherQuery())));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetCatSidcoOthers));
                throw;
            }
        }

        [HttpGet("getUnitTypes")]
        [ProducesResponseType(typeof(List<GeneralPropertiesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetUnitTypes()
        {
            try
            {
                return new JsonResult(this._mapper.Map<List<GeneralPropertiesDto>>(await this._meditor.Send(new GetUnitTypesQuery())));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetUnitTypes));
                throw;
            }
        }

        [HttpGet("getRuleEquivalents")]
        [ProducesResponseType(typeof(List<GeneralPropertiesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetRuleEquivalents()
        {
            try
            {
                return new JsonResult(this._mapper.Map<List<GeneralPropertiesDto>>(await this._meditor.Send(new GetRuleEquivalentsQuery())));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetRuleEquivalents));
                throw;
            }
        }

        [HttpGet("getLenguageEquivalents")]
        [ProducesResponseType(typeof(List<GeneralPropertiesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetLenguageEquivalents()
        {
            try
            {
                return new JsonResult(this._mapper.Map<List<GeneralPropertiesDto>>(await this._meditor.Send(new GetLanguageEquivalentsQuery())));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetLenguageEquivalents));
                throw;
            }
        }

        [HttpGet("getEquivalentsAngularDisplacement")]
        [ProducesResponseType(typeof(List<GeneralPropertiesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetEquivalentsAngularDisplacement()
        {
            try
            {
                return new JsonResult(this._mapper.Map<List<GeneralPropertiesDto>>(await this._meditor.Send(new GetEquivalentsAngularDisplacementQuery())));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetEquivalentsAngularDisplacement));
                throw;
            }
        }

        [HttpGet("getRulesRep/{pRule}/{pLenguage}")]
        [ProducesResponseType(typeof(List<RulesRepDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetRulesRep(string pRule, string pLenguage)
        {
            try
            {
                return new JsonResult(this._mapper.Map<List<RulesRepDto>>(await this._meditor.Send(new GetRulesRepQuery(pRule, pLenguage))));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetRulesRep));
                throw;
            }
        }

        [HttpGet("getConfigurationFiles/{idModule}")]
        [ProducesResponseType(typeof(ApiResponse<List<FileWeightDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationFiles(long idModule)
        {
            try
            {
                ApiResponse<List<FileWeight>> result = await this._meditor.Send(new GetConfigurationFilesQuery(idModule));
                return new JsonResult(new ApiResponse<List<FileWeightDto>>(result.Code, result.Description, this._mapper.Map<List<FileWeightDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getThirdWinding")]
        [ProducesResponseType(typeof(ApiResponse<List<GeneralPropertiesDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetThirdWinding()
        {
            try
            {
                ApiResponse<List<GeneralProperties>> result = await this._meditor.Send(new GetThirdWindingQuery());
                return new JsonResult(new ApiResponse<List<GeneralPropertiesDto>>(result.Code, result.Description, this._mapper.Map<List<GeneralPropertiesDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }
    }
}
