namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.NRA;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INraClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultNRATestsDTO>> CalculateTestNra(NRATestsGeneralDTO dtos);

        Task<ApiResponse<long>> SaveReport(NRATestsGeneralDTO dto);
    }
}
