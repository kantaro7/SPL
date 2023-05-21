namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPimClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultPIMTestsDTO>> CalculateTestPIM(PIMTestsDTO dto);

        Task<ApiResponse<long>> SaveReport(PIMTestsGeneralDTO dto);

        Task<ApiResponse<PIMTestsGeneralDTO>> GetInfoReportPIM(string nroSerie, string keyTest, bool result);
    }
}
