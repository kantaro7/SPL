namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFpcClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultFPCTestsDTO>> CalculateTestFPC(List<FPCTestsDTO> dtos);

        Task<ApiResponse<long>> SaveReport(FPCTestsGeneralDTO dto);

        Task<ApiResponse<FPCTestsGeneralDTO>> GetInfoReportFPC(string nroSerie, string keyTest, string lenguage, string unitType, decimal frecuency, bool result);
    }
}
