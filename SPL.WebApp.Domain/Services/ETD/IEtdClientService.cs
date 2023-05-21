namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ETD;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEtdClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultETDTestsDTO>> CalculateTestETD(ETDTestsGeneralDTO dto);

        Task<ApiResponse<long>> SaveReport(ETDReportDTO dto);

        Task<ApiResponse<ETDTestsGeneralDTO>> GetInfoReportETD(string nroSerie, string keyTest, bool result);
    }
}
