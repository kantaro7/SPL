namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPlrClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultPLRTestsDTO>> CalculateTestPLR(PLRTestsDTO dtos);

        Task<ApiResponse<long>> SaveReport(PLRTestsGeneralDTO dto);
    }
}
