namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRodClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultRODTestsDTO>> CalculateTestROD(List<RODTestsDTO> dtos);

        Task<ApiResponse<long>> SaveReport(RODTestsGeneralDTO dto);
        Task<ApiResponse<RODTestsGeneralDTO>> GetInfoReportROD(string nroSerie , string keyTest, string testConnection, string windingMaterial, string unitOfMeasurement, bool result);
    }
}
