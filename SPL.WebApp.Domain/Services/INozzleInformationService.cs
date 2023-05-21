namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;

    using System.Threading.Tasks;

    public interface INozzleInformationService
    {
        Task<ApiResponse<NozzlesByDesignDTO>> GetRecordNozzleInformation(string numeroSerie);
        Task<ApiResponse<long>> SaveRecordNozzleInformation(NozzlesByDesignDTO viewModel);
    }
}
