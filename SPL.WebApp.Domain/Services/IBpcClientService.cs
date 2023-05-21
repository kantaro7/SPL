namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBpcClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<long>> SaveReport(BPCTestsGeneralDTO dto);

        Task<ApiResponse<BPCTestsGeneralDTO>> GetInfoReportBPC(string nroSerie, string keyTest, bool result);
    }
}
