namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPrdClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultPRDTestsDTO>> CalculateTestPRD(PRDTestsDTO dto);

        Task<ApiResponse<long>> SaveReport(PRDTestsGeneralDTO dto);
    }
}
