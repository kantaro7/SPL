namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.DPR;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDprClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultDPRTestsDTO>> CalculateTestDPR(DPRTestsGeneralDTO dtos);

        Task<ApiResponse<long>> SaveReport(DPRTestsGeneralDTO dto);
    }
}
