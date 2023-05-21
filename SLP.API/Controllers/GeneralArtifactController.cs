using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SPL.Artifact.Api.DTOs;
using SPL.Artifact.Api.DTOs.Artifactdesign;
using SPL.Domain;
using SPL.Domain.SPL.Artifact.ArtifactDesign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace SPL.Artifact.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GeneralArtifactController : ControllerBase
    {
        #region Fields

        private readonly IMediator _meditor;
        private readonly ILogger<GeneralArtifactController> _logger;
        private readonly IMapper _mapper;

        #endregion
   
        public GeneralArtifactController(ILogger<GeneralArtifactController> logger, IMapper mapper, IMediator meditor)
        {
            _logger = logger;
            _mapper = mapper;
            _meditor = meditor;
        }


        [HttpGet("checkBoqTerciary/{nroSerie}")]
        [ProducesResponseType(typeof(InformationArtifactDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> GetBoqTerciary(string nroSerie)
        {
            try
            {
                _logger.LogInformation("{0}: '{1}'", nameof(GetBoqTerciary), nroSerie);

                var result = await _meditor.Send(new Application.Queries.Artifactdesign.GetBoqTerciaryQuery(nroSerie));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetBoqTerciary));
                throw;
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(InformationArtifactDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InformationArtifactDto>> GetGeneralArtifact(string nroSerie)
        {
            try
            {
                _logger.LogInformation("{0}: '{1}'", nameof(GetGeneralArtifact), nroSerie);

                var result = await _meditor.Send(new SPL.Artifact.Application.Queries.Artifactdesign.GetArtifactDesignQuery(nroSerie));
           
                return new JsonResult(this._mapper.Map<InformationArtifactDto>(result));
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "Error en '{0}'", nameof(GetGeneralArtifact));
                throw;
            }
        }

        [HttpGet("checkNumberOrder/{nroSerie}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CheckNumberOrder(string nroSerie)
        {
            try
            {

                var result = await _meditor.Send(new SPL.Artifact.Application.Queries.Artifactdesign.CheckNumberOrderQuery(nroSerie));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(CheckNumberOrder));
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveInformationArtifact([FromBody] InformationArtifactDto viewModel)
        {
          

            try
            {

                var result = await _meditor.Send(new SPL.Artifact.Application.Commands.Artifactdesign.SaveArtifactDesignCommand(this._mapper.Map<InformationArtifact>(viewModel)));

                 return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(SaveInformationArtifact));
                throw;
            }
        }

        [HttpPut("updateGeneralArtifact")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateGeneralArtifact( GeneralArtifactDto viewModel)
        {
           

            try
            {
                var result = await _meditor.Send(new SPL.Artifact.Application.Commands.Artifactdesign.UpdateGeneralArtifactCommand(this._mapper.Map<GeneralArtifact>(viewModel)));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(UpdateGeneralArtifact));
                throw;
            }
        }

        [HttpPut("updateNozzlesArtifact")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateNozzlesArtifact([FromBody] List<NozzlesArtifactDto> viewModel)
        {

            try
            {
                var result = await _meditor.Send(new SPL.Artifact.Application.Commands.Artifactdesign.UpdateNozzlesArtifactCommand(this._mapper.Map<List<NozzlesArtifact>>(viewModel)));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(UpdateNozzlesArtifact));
                throw;
            }
        }

        [HttpPut("updateChangingTablesArtifact")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateChangingTablesArtifact([FromBody] AllChangingTablesArtifactDto viewModel)
        {
            

            try
            {
                var result = await _meditor.Send(new SPL.Artifact.Application.Commands.Artifactdesign.UpdateChangingTablesArtifactCommand(this._mapper.Map<AllChangingTablesArtifact>(viewModel)));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(UpdateChangingTablesArtifact));
                throw;
            }
        }

        [HttpPut("updateCharacteristicsArtifact")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateCharacteristicsArtifact([FromBody] AllCharacteristicsArtifactDto viewModel)
        {

            try
            {
                var result = await _meditor.Send(new SPL.Artifact.Application.Commands.Artifactdesign.UpdateCharacteristicsArtifactCommand(this._mapper.Map<AllCharacteristicsArtifact>(viewModel)));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(UpdateCharacteristicsArtifact));
                throw;
            }

          
        }

        [HttpPut("updateLightningRodArtifact")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateLightningRodArtifact([FromBody] List<LightningRodArtifactDto> viewModel)
        {
           

            try
            {
                var result = await _meditor.Send(new SPL.Artifact.Application.Commands.Artifactdesign.UpdateLightningRodArtifactCommand(this._mapper.Map<List<LightningRodArtifact>>(viewModel)));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(UpdateLightningRodArtifact));
                throw;
            }

        }

        [HttpPut("updateRulesArtifact")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateRulesArtifact([FromBody] List<RulesArtifactDto> viewModel)
        {
            

            try
            {
                var result = await _meditor.Send(new SPL.Artifact.Application.Commands.Artifactdesign.UpdateRulesArtifactCommand(this._mapper.Map<List<RulesArtifact>>(viewModel)));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(UpdateRulesArtifact));
                throw;
            }
        }

        [HttpPut("updateWarrantiesArtifact")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateWarrantiesArtifact([FromBody] WarrantiesArtifactDto viewModel)
        {
         

            try
            {
                var result = await _meditor.Send(new SPL.Artifact.Application.Commands.Artifactdesign.UpdateWarrantiesArtifactCommand(this._mapper.Map<WarrantiesArtifact>(viewModel)));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(UpdateWarrantiesArtifact));
                throw;
            }
        }

        [HttpPut("updateLabTestsArtifact")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateLabTestsArtifact([FromBody] LabTestsArtifactDto viewModel)
        {
          

            try
            {
                var result = await _meditor.Send(new SPL.Artifact.Application.Commands.Artifactdesign.UpdateLabTestsArtifactCommand(this._mapper.Map<LabTestsArtifact>(viewModel)));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(UpdateLabTestsArtifact));
                throw;
            }
        }

        [HttpGet("GetResistDesign")]
        [ProducesResponseType(typeof(ApiResponse<List<ResistDesignDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponse<List<ResistDesignDto>>>> GetResistDesign([FromQuery] string NroSerie, string UnitOfMeasurement, string TestConnection, decimal Temperature, string IdSection, decimal Order)
        {
            try
            {
                _logger.LogInformation("{0}: '{1}'", nameof(GetResistDesign), NroSerie, UnitOfMeasurement, TestConnection, Temperature, IdSection, Order);

                var result = await _meditor.Send(new SPL.Artifact.Application.Queries.Artifactdesign.GetResistDesignQuery(NroSerie,UnitOfMeasurement,TestConnection,Temperature,IdSection,Order));

                return new JsonResult(new ApiResponse<List<ResistDesignDto>>(result.Code, result.Description, this._mapper.Map<List<ResistDesignDto>>(result.Structure)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetResistDesign));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }


        [HttpGet("GetResistDesignCustom")]
        [ProducesResponseType(typeof(ApiResponse<List<ResistDesignDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponse<List<ResistDesignDto>>>> GetResistDesignCustom([FromQuery] string NroSerie, string UnitOfMeasurement, string TestConnection, decimal Temperature, string IdSection, decimal Order)
        {
            try
            {
                _logger.LogInformation("{0}: '{1}'", nameof(GetResistDesign), NroSerie, UnitOfMeasurement, TestConnection, Temperature, IdSection, Order);

                var result = await _meditor.Send(new SPL.Artifact.Application.Queries.Artifactdesign.GetResistDesignCustomQuery(NroSerie, UnitOfMeasurement, TestConnection, Temperature, IdSection, Order));

                return new JsonResult(new ApiResponse<List<ResistDesignDto>>(result.Code, result.Description, this._mapper.Map<List<ResistDesignDto>>(result.Structure)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetResistDesign));
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("SaveResistDesign")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveResistDesign([FromBody] List<ResistDesignDto> viewModel)
        {

            try
            {

                var result = await _meditor.Send(new SPL.Artifact.Application.Commands.Artifactdesign.SaveResistDesignCommand(this._mapper.Map<List<ResistDesign>>(viewModel)));

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(SaveResistDesign));
                throw;
            }
        }

        //[HttpGet("getBillNeutro/{nroSerie}")]
        //[ProducesResponseType(typeof(NBAINeutroDto), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> GetBillNeutro(string nroSerie)
        //{
        //    try
        //    {

        //        var result = await _meditor.Send(new SPL.Artifact.Application.Queries.Artifactdesign.GetBillNeutroQuery(nroSerie));

        //        return new JsonResult(this._mapper.Map<NBAINeutroDto>(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error en '{0}'", nameof(GetBillNeutro));
        //        throw;
        //    }
        //}

        //[HttpGet("getConnectionTypes/{nroSerie}")]
        //[ProducesResponseType(typeof(ConnectionTypesDto), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> GetConnectionTypes(string nroSerie)
        //{
        //    try
        //    {

        //        var result = await _meditor.Send(new SPL.Artifact.Application.Queries.Artifactdesign.GetConnectionTypesQuery(nroSerie));

        //        return new JsonResult(this._mapper.Map<ConnectionTypesDto>(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error en '{0}'", nameof(GetConnectionTypes));
        //        throw;
        //    }
        //}

        //[HttpGet("getDerivations/{nroSerie}")]
        //[ProducesResponseType(typeof(DerivationsDto), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> GetDerivations(string nroSerie)
        //{
        //    try
        //    {

        //        var result = await _meditor.Send(new SPL.Artifact.Application.Queries.Artifactdesign.GetDerivationsQuery(nroSerie));

        //        return new JsonResult(this._mapper.Map<DerivationsDto>(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error en '{0}'", nameof(GetDerivations));
        //        throw;
        //    }
        //}

        //[HttpGet("getNBAIBilKv/{nroSerie}")]
        //[ProducesResponseType(typeof(NBAIBilKvDto), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> GetNBAIBilKv(string nroSerie)
        //{
        //    try
        //    {

        //        var result = await _meditor.Send(new SPL.Artifact.Application.Queries.Artifactdesign.GetNBAIBilKvQuery(nroSerie));
        //        return new JsonResult(this._mapper.Map<NBAIBilKvDto>(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error en '{0}'", nameof(GetNBAIBilKv));
        //        throw;
        //    }
        //}

        //[HttpGet("getTaps/{nroSerie}")]
        //[ProducesResponseType(typeof(TapsDto), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> GetTaps(string nroSerie)
        //{
        //    try
        //    {

        //        var result = await _meditor.Send(new SPL.Artifact.Application.Queries.Artifactdesign.GetTapsQuery(nroSerie));

        //        return new JsonResult(this._mapper.Map<TapsDto>(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error en '{0}'", nameof(GetTaps));
        //        throw;
        //    }
        //}

        //[HttpGet("getVoltageKV/{nroSerie}")]
        //[ProducesResponseType(typeof(VoltageKVDto), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> GetVoltageKV(string nroSerie)
        //{
        //    try
        //    {

        //        var result = await _meditor.Send(new SPL.Artifact.Application.Queries.Artifactdesign.GetVoltageKVQuery(nroSerie));

        //        return new JsonResult(this._mapper.Map<VoltageKVDto>(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error en '{0}'", nameof(GetVoltageKV));
        //        throw;
        //    }
        //}

        //[HttpGet("getTapBaan/{nroSerie}")]
        //[ProducesResponseType(typeof(TapBaanDto), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> GetTapBaan(string nroSerie)
        //{
        //    try
        //    {

        //        var result = await _meditor.Send(new SPL.Artifact.Application.Queries.Artifactdesign.GetTapBaanQuery(nroSerie));

        //        return new JsonResult(this._mapper.Map<TapBaanDto>(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error en '{0}'", nameof(GetTapBaan));
        //        throw;
        //    }
        //}

    }
}
