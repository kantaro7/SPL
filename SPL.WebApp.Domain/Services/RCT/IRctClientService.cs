namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRctClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string nroSerie);
          Task<ApiResponse<RCTTestsGeneralDTO>> GetInfoReport(string nroSerie, string keyTest, bool result);
         Task<ApiResponse<ResultRCTTestsDTO>> CalculateTestRCT(List<RCTTestsDTO> dtos);
        Task<ApiResponse<long>> SaveReport(RCTTestsGeneralDTO dto);
    }
}
