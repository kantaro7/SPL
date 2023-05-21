namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFpaClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultFPATestsDTO>> CalculateTestFPA(FPATestsDTO dto);

        Task<ApiResponse<long>> SaveReport(FPATestsGeneralDTO dto);

        Task<ApiResponse<FPATestsGeneralDTO>> GetInfoReportFPA(string nroSerie, string keyTest, string lenguage, string unitType, decimal frecuency, bool result);
    }
}
