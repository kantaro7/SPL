namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPciClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<PCITestResponseDTO>> CalculateTestPCI(PCIInputTestDTO dtos);

        Task<ApiResponse<long>> SaveReport(PCITestGeneralDTO dto);

        Task<ApiResponse<PCITestGeneralDTO>> GetInfoReportPCI(string nroSerie, string keyTest, bool result);
    }
}
