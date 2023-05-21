namespace SPL.WebApp.Domain.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;

    public interface INozzleMarkService
    {
        Task<ApiResponse<List<NozzleMarksDTO>>> GetNozzleBrands(long idMark);

        Task<ApiResponse<long>> DeleteNozzleBrands(NozzleMarksDTO viewModel);

        Task<ApiResponse<long>> SaveNozzleBrands(NozzleMarksDTO viewModel);
    }
}
