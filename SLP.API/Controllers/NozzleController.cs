namespace SPL.Artifact.Api.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using global::AutoMapper;

    using MediatR;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using SPL.Artifact.Api.DTOs.Nozzles;
    using SPL.Domain;
    using SPL.Domain.SPL.Artifact.Nozzles;
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NozzleController : ControllerBase
    {
        #region Fields

        private readonly IMediator _meditor;
        private readonly ILogger<NozzleController> _logger;
        private readonly IMapper _mapper;

        #endregion

        public NozzleController(ILogger<NozzleController> logger, IMapper mapper, IMediator meditor)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._meditor = meditor;
        }

        [HttpGet("getRecordNozzleInformation/{nroSerie}")]
        [ProducesResponseType(typeof(NozzlesByDesignDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<NozzlesByDesignDto>> GetRecordNozzleInformation(string nroSerie)
        {
            try
            {
                this._logger.LogInformation("{0}: '{1}'", nameof(GetRecordNozzleInformation), nroSerie);

                ApiResponse<NozzlesByDesign> result = await this._meditor.Send(new SPL.Artifact.Application.Queries.Nozzles.GetRecordNozzleInformationQuery(nroSerie));

                return new JsonResult(new ApiResponse<NozzlesByDesignDto>(result.Code, result.Description, this._mapper.Map<NozzlesByDesignDto>(result.Structure)));

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error en '{0}'", nameof(GetRecordNozzleInformation));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("saveRecordNozzleInformation")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveRecordNozzleInformation([FromBody] NozzlesByDesignDto viewModel)
        {
            try
            {
                ApiResponse<long> result = await this._meditor.Send(new SPL.Artifact.Application.Commands.Nozzles.SaveRecordNozzleInformationCommand(this._mapper.Map<NozzlesByDesign>(viewModel)));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error en '{0}'", nameof(SaveRecordNozzleInformation));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }
    }
}
