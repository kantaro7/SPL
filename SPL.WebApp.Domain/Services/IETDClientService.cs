namespace SPL.WebApp.Domain.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs.ETD;

    public interface IETDClientService
    {
        Task<ApiResponse<StabilizationDesignDataDTO>> GetStabilizationDesignData(string nroSerie);

        Task<ApiResponse<long>> SaveStabilizationDesignData(StabilizationDesignDataDTO data);

        Task<ApiResponse<List<CorrectionFactorKWTypeCoolingDTO>>> GetCorrectionFactorkWTypeCooling();

        Task<ApiResponse<long>> SaveCorrectionFactorkWTypeCooling(List<CorrectionFactorKWTypeCoolingDTO> dto);

        Task<ApiResponse<List<StabilizationDataDTO>>> GetStabilizationData(string nroSerie, bool? status = null, bool? stabilized = null);

        Task<ApiResponse<ResultStabilizationDataTestsDTO>> CalculeTestStabilizationData(StabilizationDataDTO dto);

        Task<ApiResponse<long>> CloseStabilizationData(string nroSerie, decimal idReg);

        Task<ApiResponse<long>> SaveStabilizationData(StabilizationDataDTO dto);

        Task<ApiResponse<HeaderCuttingDataDTO>> GetInfoHeaderCuttingData(decimal idCut);

        Task<ApiResponse<List<HeaderCuttingDataDTO>>> GetCuttingDatas(string nroSerie);

        Task<ApiResponse<ResultCuttingDataTestsDTO>> CalculateCuttingData(HeaderCuttingDataDTO dto);

        Task<ApiResponse<long>> SaveCuttingData(HeaderCuttingDataDTO dto);
    }
}
