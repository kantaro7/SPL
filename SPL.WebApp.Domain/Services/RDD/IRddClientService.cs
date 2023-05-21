namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRddClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultRDDTestsDTO>> CalculateTestRDD(RDDTestsGeneralDTO dto);

        Task<ApiResponse<long>> SaveReport(RDDTestsGeneralDTO dto);

        Task<ApiResponse<RDDTestsGeneralDTO>> GetInfoReportRDD(string nroSerie, string keyTest, string lenguage, string unitType, decimal frecuency, bool result);
    }
}
