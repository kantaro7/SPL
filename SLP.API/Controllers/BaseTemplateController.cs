using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SPL.Artifact.Api.DTOs.BaseTemplate;
using SPL.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SPL.Artifact.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BaseTemplateController : ControllerBase
    {
        #region Fields

        private readonly IMediator _meditor;
        private readonly ILogger<BaseTemplateController> _logger;
        private readonly IMapper _mapper;

        #endregion

        public BaseTemplateController(ILogger<BaseTemplateController> logger, IMapper mapper, IMediator meditor)
        {
            _logger = logger;
            _mapper = mapper;
            _meditor = meditor;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveBaseTemplate([FromBody] BaseTemplateDto viewModel)
        {
           

            try
            {
                var result = await _meditor.Send(new SPL.Artifact.Application.Commands.BaseTemplate.SaveBaseTemplateCommand(this._mapper.Map<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>(viewModel)));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, this._mapper.Map<long>(result.Structure)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(SaveBaseTemplate));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));

            }

        }

        [HttpGet("{typeReport}/{keyTest}/{keyLanguage}/{nroColumns}")]
        [ProducesResponseType(typeof(ApiResponse<BaseTemplateDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BaseTemplateDto>> GetBaseTemplate(string typeReport, string keyTest, string keyLanguage, int nroColumns)
        {
            try
            {
                var result = await _meditor.Send(new SPL.Artifact.Application.Queries.BaseTemplate.GetBaseTemplateQuery(typeReport,
                    keyTest,keyLanguage, nroColumns));

                return new JsonResult(new ApiResponse<BaseTemplateDto>(result.Code, result.Description, this._mapper.Map<BaseTemplateDto>(result.Structure)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetBaseTemplate));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
          
            }
        }

        [HttpPost("saveConsolidatedReportTemplate")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveConsolidatedReportTemplate([FromBody] BaseTemplateConsolidatedReportDto viewModel)
        {


            try
            {
                var result = await _meditor.Send(new SPL.Artifact.Application.Commands.BaseTemplate.SaveBaseTemplateConsolidatedReportCommand(this._mapper.Map<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplateConsolidatedReport>(viewModel)));

                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, this._mapper.Map<long>(result.Structure)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(SaveBaseTemplate));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));

            }

        }

        [HttpGet("getConsolidatedReportTemplate/{keyLanguage}")]
        [ProducesResponseType(typeof(ApiResponse<BaseTemplateConsolidatedReportDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BaseTemplateConsolidatedReportDto>> GetConsolidatedReportTemplate(string keyLanguage)
        {
            try
            {
                var result = await _meditor.Send(new SPL.Artifact.Application.Queries.BaseTemplate.GetBaseTemplateConsolidatedReportQuery( keyLanguage));

                return new JsonResult(new ApiResponse<BaseTemplateConsolidatedReportDto>(result.Code, result.Description, this._mapper.Map<BaseTemplateConsolidatedReportDto>(result.Structure)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetBaseTemplate));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));

            }
        }

    }
}

