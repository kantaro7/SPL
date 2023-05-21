using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SIDCO.API.DTOs;
using SIDCO.Domain.Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Serilog;

using static System.Net.Mime.MediaTypeNames;

namespace SIDCO.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SidcoController : ControllerBase
    {
        #region Fields

        private readonly IMediator _meditor;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private readonly Serilog.ILogger _loggerS;
        private readonly IMapper _mapper;

        #endregion
   
        public SidcoController(Microsoft.Extensions.Logging.ILogger<SidcoController> logger, Serilog.ILogger loggerS, IMapper mapper, IMediator meditor)
        {
            _loggerS = loggerS;
            _logger = logger;
            _mapper = mapper;
            _meditor = meditor;

        }

    
        [HttpGet]
        [ProducesResponseType(typeof(InformationArtifactDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetGeneralArtifact(string nroSerie)
        {
            try
            {
                _logger.LogInformation("{0}: '{1}'", nameof(GetGeneralArtifact), nroSerie);

                var result = await _meditor.Send(new SIDCO.Application.Queries.Artifactdesign.GetArtifactDesignQuery(nroSerie));
           
                return new JsonResult(this._mapper.Map<InformationArtifactDto>(result));
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "Error en '{0}'", nameof(GetGeneralArtifact));
                throw;
            }
        }

        [Route("getLenguages")]
        [HttpGet]
        [ProducesResponseType(typeof(List<GeneralPropertiesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetLenguages()
        {
            try
            {

                var result = await _meditor.Send(new SIDCO.Application.Queries.Artifactdesign.GetLenguagesQuery());

                return new JsonResult(this._mapper.Map<List<GeneralPropertiesDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetLenguages));
                throw;
            }
        }

        [Route("getAngularDisplacement")]
        [HttpGet]
        [ProducesResponseType(typeof(List<GeneralPropertiesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAngularDisplacement()
        {
            try
            {
                var result = await _meditor.Send(new SIDCO.Application.Queries.Artifactdesign.GetAngularDisplacementQuery());

                return new JsonResult(this._mapper.Map<List<GeneralPropertiesDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetAngularDisplacement));
                throw;
            }
        }

        [Route("getApplicableRule")]
        [HttpGet]
        [ProducesResponseType(typeof(List<GeneralPropertiesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetApplicableRule()
        {
            try
            {
                var result = await _meditor.Send(new SIDCO.Application.Queries.Artifactdesign.GetApplicableRuleQuery());

                return new JsonResult(this._mapper.Map<List<GeneralPropertiesDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetApplicableRule));
                throw;
            }
        }

        [Route("getApplications")]
        [HttpGet]
        [ProducesResponseType(typeof(List<GeneralPropertiesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetApplications()
        {
            try
            {
                var result = await _meditor.Send(new SIDCO.Application.Queries.Artifactdesign.GetApplicationsQuery());

                return new JsonResult(this._mapper.Map<List<GeneralPropertiesDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetApplications));
                throw;
            }
        }

        [Route("getTypeTransformers")]
        [HttpGet]
        [ProducesResponseType(typeof(List<GeneralPropertiesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTypeTransformers()
        {
            try
            {
                var result = await _meditor.Send(new SIDCO.Application.Queries.Artifactdesign.GetTypeTransformersQuery());

                return new JsonResult(this._mapper.Map<List<GeneralPropertiesDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetTypeTransformers));
                this._loggerS.Error(ex, "EEROR");
                throw;
            }
        }

        [Route("getOperativeConditions")]
        [HttpGet]
        [ProducesResponseType(typeof(List<GeneralPropertiesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetOperativeConditions()
        {
            try
            {
                var result = await _meditor.Send(new SIDCO.Application.Queries.Artifactdesign.GetOperativeConditionsQuery());

                return new JsonResult(this._mapper.Map<List<GeneralPropertiesDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetTypeTransformers));
                throw;
            }
        }

        [Route("getConnectionTypes")]
        [HttpGet]
        [ProducesResponseType(typeof(List<GeneralPropertiesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConnectionTypes()
        {
            try
            {
                var result = await _meditor.Send(new SIDCO.Application.Queries.Artifactdesign.GetConnectionTypesQuery());

                return new JsonResult(this._mapper.Map<List<GeneralPropertiesDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetTypeTransformers));
                throw;
            }
        }

    }
}
