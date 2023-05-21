namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRyeClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultRYETestsDTO>> CalculateTestRYE(OutRYETestsDTO dto);

        Task<ApiResponse<long>> SaveReport(RYETestsGeneralDTO dto);
    }
}
