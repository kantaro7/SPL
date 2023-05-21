namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICemClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<long>> SaveReport(CEMTestsGeneralDTO dto);

        Task<ApiResponse<CEMTestsGeneralDTO>> GetInfoReportCEM(string nroSerie, string keyTest, bool result);
    }
}
