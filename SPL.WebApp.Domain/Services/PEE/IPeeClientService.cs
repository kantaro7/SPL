namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPeeClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultPEETestsDTO>> CalculateTestPEE(PEETestsDTO dto);

        Task<ApiResponse<long>> SaveReport(PEETestsGeneralDTO dto);

        Task<ApiResponse<PEETestsGeneralDTO>> GetInfoReportPEE(string nroSerie, string keyTest, bool result);
    }
}
