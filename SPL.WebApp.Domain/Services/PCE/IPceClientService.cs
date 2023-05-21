namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPceClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultPCETestsDTO>> CalculateTestPCE(List<PCETestsDTO> dtos);

        Task<ApiResponse<long>> SaveReport(PCETestsGeneralDTO dto);

        Task<ApiResponse<PCETestsGeneralDTO>> GetInfoReportPCE(string nroSerie, string keyTest, bool result);
    }
}
