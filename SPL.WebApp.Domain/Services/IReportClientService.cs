namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReportClientService
    {
        Task<ApiResponse<IEnumerable<ReportsDTO>>> GetReportTypes();
        Task<ApiResponse<ReportPDFDto>> GetPDFReport(long code, string typeReport);
        Task<ApiResponse<List<ConsolidatedReportDTO>>> GetConsolidatedReport(string noSerie, string languaje);
        Task<ApiResponse<List<TypeConsolidatedReportDTO>>> GetTypeSectionConsolidatedReport(string noSerie, string languaje);
        Task<ApiResponse<CheckInfoRODDTO>> CheckInfoRod(string noSerie, string windingMaterial, string atPositions, string btPositions, string terPositions,
            bool isAT, bool isBT, bool isTer);    
        
        Task<ApiResponse<CheckInfoRODDTO>> CheckInfoPce(string noSerie, string capacity, string atPositions, string btPositions, string terPositions,  bool isAT, bool isBT, bool isTer);

        Task<ApiResponse<IEnumerable<PCIParameters>>> GetAAsync(string noSerie, string windingMaterial, string capacity, string atPositions, string btPositions, string terPositions, bool isAT, bool isBT, bool isTer);

        Task<ApiResponse<List<ConsolidatedReportDTO>>> GetTestedReport(string noSerie);
    }
}
