namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFpbClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultFPBTestsDTO>> CalculateTestFPB(List<FPBTestsDTO> dtos);

        Task<ApiResponse<long>> SaveReport(FPBTestsGeneralDTO dto);
    }
}
