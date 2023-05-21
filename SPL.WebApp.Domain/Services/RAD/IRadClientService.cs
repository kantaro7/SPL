namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRadClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultRADTestsDTO>> CalculateTestRAD(RADTestsDTO dto);

        Task<ApiResponse<long>> SaveReport(RADReportDTO dto);
    }
}
