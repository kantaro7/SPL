namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITapClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultTAPTestsDTO>> CalculateTestTAP(TAPTestsDTO dto);

        Task<ApiResponse<long>> SaveReport(TAPTestsGeneralDTO dto);
    }
}
