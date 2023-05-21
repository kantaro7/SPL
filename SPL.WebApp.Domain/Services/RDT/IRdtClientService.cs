namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRdtClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultRDTTestsDetailsDTO>> CalculateTestRDT(RDTTestsDTO dto);

        Task<ApiResponse<long>> SaveReport(RDTReportDTO dto);
    }
}
