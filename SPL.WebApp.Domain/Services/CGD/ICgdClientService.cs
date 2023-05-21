namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICgdClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultCGDTestsDTO>> CalculateTestCGD(List<CGDTestsDTO> dto);

        Task<ApiResponse<long>> SaveReport(CGDTestsGeneralDTO dto);
    }
}
