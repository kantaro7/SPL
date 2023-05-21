namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRanClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultRANTestsDTO>> CalculateTestRAN(RANTestsDetailsDTO dto);

        Task<ApiResponse<long>> SaveReport(RANReportDTO dto);
    }
}
