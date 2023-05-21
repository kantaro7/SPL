namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IArfClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultTDPTestsDTO>> CalculateTestTdp(TDPTestsGeneralDTO dtos);

        Task<ApiResponse<long>> SaveReport(ARFTestsGeneralDTO dto);
    }
}
