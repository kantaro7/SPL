namespace SPL.Artifact.Api.Controllers
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

    using SPL.Artifact.Api.DTOs.Artifactdesign;
    using SPL.Artifact.Api.DTOs.PlateTension;
    using SPL.Domain;
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PlateTensionController : ControllerBase
    {
        #region Fields

        private readonly IMediator _meditor;
        private readonly ILogger<GeneralArtifactController> _logger;
        private readonly IMapper _mapper;

        #endregion

        public PlateTensionController(ILogger<GeneralArtifactController> logger, IMapper mapper, IMediator meditor)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._meditor = meditor;
        }

        [HttpGet("getCharacteristics/{nroSerie}")]
        [ProducesResponseType(typeof(CharacteristicsPlaneTensionDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CharacteristicsPlaneTensionDto>> GetCharacteristics(string nroSerie)
        {
            try
            {
                this._logger.LogInformation("{0}: '{1}'", nameof(GetCharacteristics), nroSerie);

                ApiResponse<Domain.SPL.Artifact.ArtifactDesign.InfoCarLocal> result = await this._meditor.Send(new SPL.Artifact.Application.Queries.PlateTension.GetCharacterisQuery(nroSerie));

                return new JsonResult(new ApiResponse<CharacteristicsPlaneTensionDto>(result.Code, result.Description, this._mapper.Map<CharacteristicsPlaneTensionDto>(result.Structure)));

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error en '{0}'", nameof(GetCharacteristics));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getTapBaan/{nroSerie}")]
        [ProducesResponseType(typeof(TapBaanDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponse<TapBaanDto>>> GetTapBaan(string nroSerie)
        {
            try
            {
                this._logger.LogInformation("{0}: '{1}'", nameof(GetTapBaan), nroSerie);

                ApiResponse<Domain.SPL.Artifact.ArtifactDesign.TapBaan> result = await this._meditor.Send(new SPL.Artifact.Application.Queries.PlateTension.GetTapBaanQuery(nroSerie));

                return new JsonResult(new ApiResponse<TapBaanDto>(result.Code, result.Description, this._mapper.Map<TapBaanDto>(result.Structure)));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error en '{0}'", nameof(GetTapBaan));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getPlateTension/{unit}/{typeVoltage}")]
        [ProducesResponseType(typeof(ApiResponse<List<PlateTensionDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<PlateTensionDto>>> GetPlateTension(string unit, string typeVoltage)
        {
            try
            {
                ApiResponse<List<Domain.SPL.Artifact.PlateTension.PlateTension>> result = await this._meditor.Send(new SPL.Artifact.Application.Queries.PlateTension.GetPlateTensionQuery(unit,
                    typeVoltage));

                return new JsonResult(new ApiResponse<List<PlateTensionDto>>(result.Code, result.Description, this._mapper.Map<List<PlateTensionDto>>(result.Structure)));

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error en '{0}'", nameof(GetPlateTension));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpGet("getTensionOriginalPlate/{unit}/{typeVoltage}")]
        [ProducesResponseType(typeof(ApiResponse<List<PlateTensionDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<PlateTensionDto>>> GetTensionOriginalPlate(string unit, string typeVoltage)
        {
            try
            {
                ApiResponse<List<Domain.SPL.Artifact.PlateTension.PlateTension>> result = await this._meditor.Send(new SPL.Artifact.Application.Queries.PlateTension.GetTensionOriginalPlateQuery(unit,
                    typeVoltage));

                return new JsonResult(new ApiResponse<List<PlateTensionDto>>(result.Code, result.Description, this._mapper.Map<List<PlateTensionDto>>(result.Structure)));

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error en '{0}'", nameof(GetPlateTension));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("/{StatusDelete}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SavePlateTension([FromBody] List<PlateTensionDto> viewModel, short StatusDelete)
        {
            try
            {
                ApiResponse<long> result = await this._meditor.Send(new SPL.Artifact.Application.Commands.PlateTension.SavePlateTensionCommand(this._mapper.Map<List<Domain.SPL.Artifact.PlateTension.PlateTension>>(viewModel), Convert.ToBoolean(StatusDelete)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, this._mapper.Map<long>(result.Structure)));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error en '{0}'", nameof(SavePlateTension));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }
    }
}
