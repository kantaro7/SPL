namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;

    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    public interface IArtifactClientService
    {
        #region Artifact Records Methods
        Task<decimal> CheckBoqTerciary(string nroSerie);

        Task<InformationArtifactDTO> GetArtifact(string nroSerie);

        Task<InformationArtifactDTO> GetArtifactSIDCO(string nroSerie);

        Task<HttpStatusCode> AddArtifactToSPL(InformationArtifactDTO dto);

        Task<HttpStatusCode> UpdateArtifactToSPL(InformationArtifactDTO dto, ArtifactUpdate tabUpdate, string NameUserM, string NameUserC, DateTime FechaCreacion);

        Task<bool> CheckNumberOrder(string noSerie);
        #endregion

        #region Plate Tension Methods

        Task<ApiResponse<CharacteristicsPlaneTensionDTO>> GetCharacteristics(string nroSerie);

        Task<ApiResponse<TapBaanDTO>> GetTapBaanPlateTension(string nroSerie);

        Task<ApiResponse<List<PlateTensionDTO>>> GetPlateTension(string unit, string typeVoltage);

        public Task<ApiResponse<List<PlateTensionDTO>>> GetTensionOriginalPlate(string unit, string typeVoltage);

        Task<ApiResponse<long>> AddTension(List<PlateTensionDTO> dto, bool isNewTension);

        #endregion

        #region  Base Template

        Task<ApiResponse<long>> AddBaseTemplate(BaseTemplateDTO dto);


        Task<ApiResponse<long>> AddBaseTemplateConsolidatedReport(BaseTemplateConsolidatedReportDTO dto);
        #endregion

        #region Resistance Design

        Task<ApiResponse<long>> AddResistanceDesign(List<ResistDesignDTO> dto);

        Task<ApiResponse<BaseTemplateConsolidatedReportDTO>> GetBaseTemplateConsolidatedReport(string idioma);

        #endregion
    }
}
